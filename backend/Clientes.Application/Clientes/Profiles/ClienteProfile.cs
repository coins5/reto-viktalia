using AutoMapper;
using Clientes.Application.Clientes.DTOs;
using Clientes.Domain.Entities;

namespace Clientes.Application.Clientes.Profiles;

public class ClienteProfile : Profile
{
    public ClienteProfile()
    {
        CreateMap<CreateClienteRequest, Cliente>()
            .ConstructUsing(src => new Cliente(
                src.Ruc.Trim(),
                src.RazonSocial.Trim(),
                Normalize(src.TelefonoContacto),
                Normalize(src.CorreoContacto),
                Normalize(src.Direccion)));

        CreateMap<Cliente, ClienteDto>();
    }

    private static string? Normalize(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
