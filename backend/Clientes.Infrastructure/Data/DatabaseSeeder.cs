using Clientes.Domain.Entities;
using Clientes.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Clientes.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ClientesDbContext context, ILogger logger, CancellationToken cancellationToken = default)
    {
        var existingCount = await context.Clientes.IgnoreQueryFilters().CountAsync(cancellationToken);
        if (existingCount > 0)
        {
            return;
        }

        var now = DateTimeOffset.UtcNow;

        var clientes = new List<Cliente>
        {
            new("20100070970", "Distribuidora Andina SAC", "012345678", "contacto@andina.com", "Av. Los Laureles 123, Lima"),
            new("20481234567", "Servicios Industriales del Sur SRL", "987654321", "ventas@servindusur.pe", "Jr. Piura 450, Arequipa"),
            new("20546789012", "Agroexportaciones del Pacífico SAC", "921456789", "info@agropacifico.pe", "Carretera Panamericana Sur km 75, Cañete"),
            new("20651237891", "Tecnologías Modernas SAC", "014567890", "soporte@tecmoderna.pe", "Av. Javier Prado Este 560, Lima"),
            new("20782345619", "Consultores Estratégicos SRL", "016789012", "contacto@consultoreses.com", "Malecón Balta 320, Miraflores"),
            new("20893456781", "Logística Integral del Norte SAC", "073456789", "logistica@linorte.pe", "Parque Industrial, Trujillo"),
            new("20904567812", "Salud Total Peru SAC", "015678903", "citas@saludtotal.pe", "Av. San Felipe 1020, Jesús María"),
            new("20123456780", "Constructora Horizonte SAC", "019876543", "proyectos@horizonte.pe", "Av. Universitaria 1789, San Miguel"),
            new("20234567891", "Energías Renovables Andinas SAC", "017654321", "energia@erandinas.pe", "Calle Amazonas 145, Cusco"),
            new("20345678912", "Retail Express SAC", "013456789", "ventas@retailexpress.pe", "Centro Comercial El Polo, Lima")
        };

        foreach (var cliente in clientes)
        {
            cliente.MarkCreated(now);
        }

        await context.Clientes.AddRangeAsync(clientes, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Seed de clientes inicial completado ({Count} registros).", clientes.Count);
    }
}
