namespace Clientes.Application.Clientes.DTOs;

public class ClienteQueryParameters
{
    private const int MaxPageSize = 100;
    private int _page = 1;
    private int _pageSize = 10;

    public int Page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set
        {
            if (value < 1)
            {
                _pageSize = 10;
                return;
            }

            _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }

    public string? SortBy { get; set; }
    public string? SortDir { get; set; }
    public string? Ruc { get; set; }
    public string? RazonSocial { get; set; }
}
