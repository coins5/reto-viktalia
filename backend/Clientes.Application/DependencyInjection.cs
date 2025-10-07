using System.Reflection;
using Clientes.Application.Clientes.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Clientes.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddScoped<IClienteService, ClienteService>();
        return services;
    }
}
