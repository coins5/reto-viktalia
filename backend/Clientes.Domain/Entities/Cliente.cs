using Clientes.Domain.Common;

namespace Clientes.Domain.Entities;

public class Cliente : AuditableEntity
{
    public string Ruc { get; private set; } = null!;
    public string RazonSocial { get; private set; } = null!;
    public string? TelefonoContacto { get; private set; }
    public string? CorreoContacto { get; private set; }
    public string? Direccion { get; private set; }

    private Cliente()
    {
    }

    public Cliente(
        string ruc,
        string razonSocial,
        string? telefonoContacto,
        string? correoContacto,
        string? direccion)
    {
        UpdateCore(ruc, razonSocial);
        UpdateContact(telefonoContacto, correoContacto, direccion);
    }

    public void Update(
        string ruc,
        string razonSocial,
        string? telefonoContacto,
        string? correoContacto,
        string? direccion,
        DateTimeOffset timestamp)
    {
        UpdateCore(ruc, razonSocial);
        UpdateContact(telefonoContacto, correoContacto, direccion);
        MarkUpdated(timestamp);
    }

    private void UpdateCore(string ruc, string razonSocial)
    {
        Ruc = ruc.Trim();
        RazonSocial = razonSocial.Trim();
    }

    private void UpdateContact(string? telefono, string? correo, string? direccion)
    {
        TelefonoContacto = string.IsNullOrWhiteSpace(telefono) ? null : telefono.Trim();
        CorreoContacto = string.IsNullOrWhiteSpace(correo) ? null : correo.Trim();
        Direccion = string.IsNullOrWhiteSpace(direccion) ? null : direccion.Trim();
    }
}
