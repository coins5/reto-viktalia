using Clientes.Application.Clientes.DTOs;
using Clientes.Application.Common.Interfaces;
using Clientes.Application.Common.Models;
using Clientes.Domain.Entities;
using Clientes.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Clientes.Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly ClientesDbContext _context;

    public ClienteRepository(ClientesDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Cliente cliente, CancellationToken cancellationToken = default)
    {
        await _context.Clientes.AddAsync(cliente, cancellationToken);
    }

    public Task<Cliente?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return _context.Clientes.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);
    }

    public async Task<bool> ExistsByRucAsync(string ruc, long? excludeId = null, CancellationToken cancellationToken = default)
    {
        var normalized = ruc.Trim();
        var count = await _context.Clientes
            .Where(c =>
                c.Ruc == normalized &&
                (!excludeId.HasValue || c.Id != excludeId.Value) &&
                !c.IsDeleted)
            .CountAsync(cancellationToken);

        return count > 0;
    }

    public async Task<PagedResult<Cliente>> SearchAsync(ClienteQueryParameters parameters, CancellationToken cancellationToken = default)
    {
        var query = _context.Clientes
            .Where(c => !c.IsDeleted)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(parameters.Ruc))
        {
            var ruc = parameters.Ruc.Trim();
            query = query.Where(c => c.Ruc.StartsWith(ruc));
        }

        if (!string.IsNullOrWhiteSpace(parameters.RazonSocial))
        {
            var razon = parameters.RazonSocial.Trim().ToUpperInvariant();
            query = query.Where(c => EF.Functions.Like(c.RazonSocial.ToUpper(), $"%{razon}%"));
        }

        query = ApplySorting(query, parameters);

        var total = await query.CountAsync(cancellationToken);
        var page = parameters.Page;
        var pageSize = parameters.PageSize;

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(total / (double)pageSize);
        var paging = new PagingMetadata(page, pageSize, total, totalPages);
        var sort = new SortMetadata(parameters.SortBy, parameters.SortDir);

        return new PagedResult<Cliente>(items, paging, sort);
    }

    public Task UpdateAsync(Cliente cliente, CancellationToken cancellationToken = default)
    {
        _context.Clientes.Update(cliente);
        return Task.CompletedTask;
    }

    private static IQueryable<Cliente> ApplySorting(IQueryable<Cliente> query, ClienteQueryParameters parameters)
    {
        if (string.IsNullOrWhiteSpace(parameters.SortBy))
        {
            return query.OrderByDescending(c => c.CreatedAt);
        }

        var sortBy = parameters.SortBy.Trim().ToLowerInvariant();
        var descending = string.Equals(parameters.SortDir, "desc", StringComparison.OrdinalIgnoreCase);

        return sortBy switch
        {
            "ruc" => descending ? query.OrderByDescending(c => c.Ruc) : query.OrderBy(c => c.Ruc),
            "razonsocial" => descending ? query.OrderByDescending(c => c.RazonSocial) : query.OrderBy(c => c.RazonSocial),
            "createdat" => descending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt),
            _ => query.OrderByDescending(c => c.CreatedAt)
        };
    }
}
