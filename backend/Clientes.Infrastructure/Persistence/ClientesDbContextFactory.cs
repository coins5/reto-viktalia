using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Clientes.Infrastructure.Persistence;

public class ClientesDbContextFactory : IDesignTimeDbContextFactory<ClientesDbContext>
{
    public ClientesDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("Oracle")
                               ?? Environment.GetEnvironmentVariable("ORACLE_CONNECTION_STRING")
                               ?? "User Id=clientes_app;Password=clientes_app;Data Source=localhost:1521/XEPDB1;";

        var optionsBuilder = new DbContextOptionsBuilder<ClientesDbContext>();
        optionsBuilder.UseOracle(connectionString, options =>
        {
            options.MigrationsAssembly(typeof(ClientesDbContext).Assembly.FullName);
        });

        return new ClientesDbContext(optionsBuilder.Options);
    }
}
