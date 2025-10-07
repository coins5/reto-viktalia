using Clientes.Api.Models;
using Clientes.Application.Clientes.DTOs;
using Clientes.Application.Clientes.Services;
using Clientes.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Clientes.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clienteService;

    public ClientesController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    /// <summary>
    /// Crea un nuevo cliente.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ClienteDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateClienteRequest request, CancellationToken cancellationToken)
    {
        var cliente = await _clienteService.CreateAsync(request, cancellationToken);
        var response = ApiResponse<ClienteDto>.Success(cliente, HttpContext.TraceIdentifier);
        return CreatedAtRoute("GetClienteById", new { id = cliente.Id }, response);
    }

    /// <summary>
    /// Obtiene un cliente por identificador.
    /// </summary>
    [HttpGet("{id:long}", Name = "GetClienteById")]
    [ProducesResponseType(typeof(ApiResponse<ClienteDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] long id, CancellationToken cancellationToken)
    {
        var cliente = await _clienteService.GetByIdAsync(id, cancellationToken);
        var response = ApiResponse<ClienteDto>.Success(cliente, HttpContext.TraceIdentifier);
        return Ok(response);
    }

    /// <summary>
    /// Lista clientes con filtros, ordenamiento y paginación.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<ClienteDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync([FromQuery] ClienteQueryParameters parameters, CancellationToken cancellationToken)
    {
        var result = await _clienteService.SearchAsync(parameters, cancellationToken);
        var response = ApiResponse<IReadOnlyCollection<ClienteDto>>.Success(
            result.Data,
            HttpContext.TraceIdentifier,
            result.Paging,
            result.Sort);

        return Ok(response);
    }

    /// <summary>
    /// Actualiza un cliente existente.
    /// </summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<ClienteDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromBody] UpdateClienteRequest request, CancellationToken cancellationToken)
    {
        var cliente = await _clienteService.UpdateAsync(id, request, cancellationToken);
        var response = ApiResponse<ClienteDto>.Success(cliente, HttpContext.TraceIdentifier);
        return Ok(response);
    }

    /// <summary>
    /// Elimina (lógicamente) un cliente.
    /// </summary>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAsync([FromRoute] long id, CancellationToken cancellationToken)
    {
        await _clienteService.DeleteAsync(id, cancellationToken);
        var response = ApiResponse<object>.Success(null, HttpContext.TraceIdentifier);
        return Ok(response);
    }
}
