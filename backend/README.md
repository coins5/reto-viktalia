# Backend - Clientes API

API REST construida con .NET 9 y EF Core (Oracle) siguiendo un enfoque de Clean Architecture.

## Requisitos

- .NET SDK 9
- Docker (para Oracle) opcional en desarrollo local
- Oracle Free 23c (local o en contenedor)

## Configuración

1. Clona el repositorio y ubícate en la carpeta `backend`.
2. Ajusta la cadena de conexión en `appsettings.Development.json` si usas una instancia local.
3. Opcional: exporta las variables de entorno

```bash
export ConnectionStrings__Oracle="User Id=clientes_app;Password=clientes_app;Data Source=localhost:1521/FREEPDB1;"
export UseInMemoryDatabase=false
```

## Ejecución local

```bash
dotnet restore
dotnet ef database update
dotnet run --project Clientes.Api
```

La API quedará disponible en `http://localhost:8080` (configurable mediante `ASPNETCORE_URLS`).

## Pruebas

```bash
dotnet test
```

Se incluyen pruebas unitarias (FluentValidation y servicios) y de integración usando `WebApplicationFactory` e InMemory provider.

## Migraciones

- Crear nueva migración:

  ```bash
  dotnet ef migrations add NombreMigracion --project Clientes.Infrastructure/Clientes.Infrastructure.csproj --startup-project Clientes.Api/Clientes.Api.csproj --output-dir Persistence/Migrations
  ```

- Aplicar migraciones:

  ```bash
  dotnet ef database update --project Clientes.Infrastructure/Clientes.Infrastructure.csproj --startup-project Clientes.Api/Clientes.Api.csproj
  ```

## Logging y observabilidad

- Serilog configurado con sinks a consola y archivo (`logs/api-.log`).
- Middleware de excepciones + formato de respuesta `{ data, paging, sort, errors, traceId }`.
- Middleware de correlación expone `X-Trace-Id` para depuración cruzada con frontend.

## Troubleshooting

- **Oracle tarda en iniciar**: espera a que el contenedor esté `healthy` antes de levantar la API.
- **Errores de migración**: revisa credenciales y que el usuario tenga permisos `CREATE TABLE`/`CREATE SEQUENCE`.
- **Ejecución con InMemory**: establece `UseInMemoryDatabase=true` para entornos de prueba sin Oracle.
