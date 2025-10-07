using Clientes.Application.Clientes.DTOs;
using Clientes.Application.Common.Models;
using Clientes.Domain.Entities;

namespace Clientes.Application.Common.Interfaces;

public interface IClienteRepository
{
    Task AddAsync(Cliente cliente, CancellationToken cancellationToken = default);
    Task<Cliente?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByRucAsync(string ruc, long? excludeId = null, CancellationToken cancellationToken = default);
    Task<PagedResult<Cliente>> SearchAsync(ClienteQueryParameters parameters, CancellationToken cancellationToken = default);
    Task UpdateAsync(Cliente cliente, CancellationToken cancellationToken = default);
}
