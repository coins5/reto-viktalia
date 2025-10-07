using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace Clientes.Api.Middleware;

public class CorrelationIdMiddleware
{
    public const string TraceHeaderName = "X-Trace-Id";
    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;

    public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var traceId = Activity.Current?.Id ?? context.TraceIdentifier ?? Guid.NewGuid().ToString("N");
        context.TraceIdentifier = traceId;
        context.Response.Headers[TraceHeaderName] = traceId;

        using (LogContext.PushProperty("TraceId", traceId))
        {
            _logger.LogDebug("Procesando request con TraceId {TraceId}", traceId);
            await _next(context);
        }
    }
}
