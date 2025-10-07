namespace Clientes.Application.Common.Models;

public class PagedResult<T>
{
    public IReadOnlyCollection<T> Data { get; }
    public PagingMetadata Paging { get; }
    public SortMetadata? Sort { get; }

    public PagedResult(IEnumerable<T> data, PagingMetadata paging, SortMetadata? sort = null)
    {
        Data = data.ToArray();
        Paging = paging;
        Sort = sort;
    }
}

public record PagingMetadata(int Page, int PageSize, int Total, int TotalPages);

public record SortMetadata(string? By, string? Dir);
