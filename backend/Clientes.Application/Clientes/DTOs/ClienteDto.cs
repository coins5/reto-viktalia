namespace Clientes.Application.Clientes.DTOs;

public record ClienteDto(
    long Id,
    string Ruc,
    string RazonSocial,
    string? TelefonoContacto,
    string? CorreoContacto,
    string? Direccion,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
