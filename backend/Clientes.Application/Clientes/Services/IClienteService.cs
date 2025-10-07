using Clientes.Application.Clientes.DTOs;
using Clientes.Application.Common.Models;

namespace Clientes.Application.Clientes.Services;

public interface IClienteService
{
    Task<ClienteDto> CreateAsync(CreateClienteRequest request, CancellationToken cancellationToken = default);
    Task<ClienteDto> UpdateAsync(long id, UpdateClienteRequest request, CancellationToken cancellationToken = default);
    Task<ClienteDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<PagedResult<ClienteDto>> SearchAsync(ClienteQueryParameters parameters, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
