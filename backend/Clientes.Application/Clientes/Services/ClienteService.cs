using AutoMapper;
using Clientes.Application.Clientes.DTOs;
using Clientes.Application.Common.Exceptions;
using Clientes.Application.Common.Interfaces;
using Clientes.Application.Common.Models;
using Clientes.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Clientes.Application.Clientes.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMapper _mapper;
    private readonly ILogger<ClienteService> _logger;

    public ClienteService(
        IClienteRepository clienteRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        IMapper mapper,
        ILogger<ClienteService> logger)
    {
        _clienteRepository = clienteRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ClienteDto> CreateAsync(CreateClienteRequest request, CancellationToken cancellationToken = default)
    {
        var normalizedRuc = request.Ruc.Trim();

        if (await _clienteRepository.ExistsByRucAsync(normalizedRuc, null, cancellationToken))
        {
            _logger.LogWarning("Intento de crear cliente duplicado con RUC {Ruc}", normalizedRuc);
            throw new ConflictException($"Ya existe un cliente con el RUC {normalizedRuc}.");
        }

        var cliente = _mapper.Map<Cliente>(request);
        var timestamp = _dateTimeProvider.UtcNow;
        cliente.MarkCreated(timestamp);

        await _clienteRepository.AddAsync(cliente, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Cliente {ClienteId} creado correctamente", cliente.Id);
        return _mapper.Map<ClienteDto>(cliente);
    }

    public async Task<ClienteDto> UpdateAsync(long id, UpdateClienteRequest request, CancellationToken cancellationToken = default)
    {
        var cliente = await _clienteRepository.GetByIdAsync(id, cancellationToken)
                       ?? throw new NotFoundException("Cliente", id);

        var normalizedRuc = request.Ruc.Trim();
        if (await _clienteRepository.ExistsByRucAsync(normalizedRuc, id, cancellationToken))
        {
            _logger.LogWarning("Intento de actualizar cliente {ClienteId} con RUC en uso {Ruc}", id, normalizedRuc);
            throw new ConflictException($"Ya existe un cliente con el RUC {normalizedRuc}.");
        }

        var timestamp = _dateTimeProvider.UtcNow;
        cliente.Update(
            normalizedRuc,
            request.RazonSocial,
            request.TelefonoContacto,
            request.CorreoContacto,
            request.Direccion,
            timestamp);

        await _clienteRepository.UpdateAsync(cliente, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Cliente {ClienteId} actualizado", cliente.Id);
        return _mapper.Map<ClienteDto>(cliente);
    }

    public async Task<ClienteDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var cliente = await _clienteRepository.GetByIdAsync(id, cancellationToken)
                       ?? throw new NotFoundException("Cliente", id);

        return _mapper.Map<ClienteDto>(cliente);
    }

    public async Task<PagedResult<ClienteDto>> SearchAsync(ClienteQueryParameters parameters, CancellationToken cancellationToken = default)
    {
        var result = await _clienteRepository.SearchAsync(parameters, cancellationToken);
        var mapped = result.Data.Select(_mapper.Map<ClienteDto>).ToArray();
        return new PagedResult<ClienteDto>(mapped, result.Paging, result.Sort);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var cliente = await _clienteRepository.GetByIdAsync(id, cancellationToken)
                       ?? throw new NotFoundException("Cliente", id);

        var timestamp = _dateTimeProvider.UtcNow;
        cliente.MarkDeleted(timestamp);

        await _clienteRepository.UpdateAsync(cliente, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Cliente {ClienteId} eliminado lógicamente", cliente.Id);
    }
}
