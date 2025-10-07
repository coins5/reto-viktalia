using Clientes.Application.Common.Interfaces;
using Clientes.Infrastructure.Persistence;
using Clientes.Infrastructure.Repositories;
using Clientes.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Clientes.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var useInMemory = configuration.GetValue<bool>("UseInMemoryDatabase");
        var connectionString = configuration.GetConnectionString("Oracle");

        services.AddDbContext<ClientesDbContext>(options =>
        {
            if (useInMemory)
            {
                options.UseInMemoryDatabase("ClientesInMemoryDb");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new InvalidOperationException("La cadena de conexión 'Oracle' no está configurada.");
                }

                options.UseOracle(connectionString, oracleOptions =>
                {
                    oracleOptions.MigrationsAssembly(typeof(ClientesDbContext).Assembly.FullName);
                });
            }
        });

        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IDateTimeProvider, SystemDateTimeProvider>();
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ClientesDbContext>());

        return services;
    }
}
