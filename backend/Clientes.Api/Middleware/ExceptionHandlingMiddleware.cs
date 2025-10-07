using System.Net;
using System.Text.Json;
using Clientes.Api.Models;
using Clientes.Application.Common.Exceptions;
using FluentValidation;

namespace Clientes.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var traceId = context.TraceIdentifier;
        HttpStatusCode statusCode;
        IEnumerable<ApiError> errors;

        switch (exception)
        {
            case ValidationException validationException:
                statusCode = HttpStatusCode.BadRequest;
                errors = validationException.Errors
                    .Select(error => new ApiError("VALIDATION_ERROR", error.ErrorMessage, error.PropertyName));
                break;
            case ApplicationExceptionBase applicationException:
                statusCode = applicationException.StatusCode;
                errors = new[] { new ApiError(applicationException.GetType().Name.ToUpperInvariant(), applicationException.Message) };
                break;
            default:
                statusCode = HttpStatusCode.InternalServerError;
                var message = _environment.IsDevelopment()
                    ? exception.Message
                    : "Ocurrió un error inesperado. Inténtelo nuevamente más tarde.";
                errors = new[] { new ApiError("UNEXPECTED_ERROR", message) };
                break;
        }

        _logger.LogError(exception, "Se produjo una excepción. TraceId: {TraceId}", traceId);

        var response = ApiResponse<object>.Failure(errors, traceId);
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, SerializerOptions));
    }
}
