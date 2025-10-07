namespace Clientes.Application.Clientes.DTOs;

public class CreateClienteRequest
{
    public string Ruc { get; set; } = string.Empty;
    public string RazonSocial { get; set; } = string.Empty;
    public string? TelefonoContacto { get; set; }
    public string? CorreoContacto { get; set; }
    public string? Direccion { get; set; }
}
