using Clientes.Application.Common.Models;

namespace Clientes.Api.Models;

public class ApiResponse<T>
{
    public T? Data { get; init; }
    public PagingMetadata? Paging { get; init; }
    public SortMetadata? Sort { get; init; }
    public IEnumerable<ApiError>? Errors { get; init; }
    public string TraceId { get; init; } = string.Empty;

    public static ApiResponse<T> Success(
        T? data,
        string traceId,
        PagingMetadata? paging = null,
        SortMetadata? sort = null)
    {
        return new ApiResponse<T>
        {
            Data = data,
            Paging = paging,
            Sort = sort,
            TraceId = traceId
        };
    }

    public static ApiResponse<T> Failure(IEnumerable<ApiError> errors, string traceId)
    {
        return new ApiResponse<T>
        {
            Errors = errors,
            TraceId = traceId
        };
    }
}

public record ApiError(string Code, string Message, string? Field = null);
