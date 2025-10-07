# Reto Viktalia – Gestión de Clientes

Monorepo que contiene:

- **Backend (.NET 9 + EF Core + Oracle)** – `backend/`
- **Frontend (Angular 17 + Angular Material)** – `clientes-web/`
- **Infraestructura Docker** con Oracle Free 23c, API y frontend – `docker-compose.yml`
- **Colección REST Client** – `docs/clientes.http`

## Requisitos

- Docker & Docker Compose
- .NET SDK 9 (si deseas ejecutar backend sin contenedor)
- Node.js 20 (si deseas ejecutar frontend sin contenedor)

## Levantar todo con Docker

```bash
make up
# API: http://localhost:8080/swagger
# Web: http://localhost:4200
```

Comandos útiles:

```bash
make down          # detener servicios
make logs          # logs de todos los contenedores
make test-backend  # dotnet test
make test-frontend # npm test --watch=false
```

## Ejecución manual (sin Docker)

1. **Oracle Free 23c** – arranca tu instancia (por ejemplo con `docker compose up oracle`).
2. **Backend**
   ```bash
   cd backend
   dotnet restore
   dotnet ef database update
   dotnet run --project Clientes.Api
   ```
3. **Frontend**
   ```bash
   cd clientes-web
   npm ci
   npm run start
   ```

## Estructura destacada

## Arquitectura

El backend sigue un enfoque de **Clean Architecture**:

- `backend/Clientes.Domain`: capa de dominio con entidades y reglas sin dependencias externas.
- `backend/Clientes.Application`: casos de uso, DTOs, validadores y servicios, dependiendo sólo del dominio y de interfaces.
- `backend/Clientes.Infrastructure`: implementaciones concretas (EF Core, Oracle, seeders, servicios de sistema), satisfaciendo las interfaces de aplicación.
- `backend/Clientes.Api`: capa más externa con controllers, middleware, configuraciones (Serilog, Swagger, CORS).

El frontend Angular consume los endpoints REST expuestos por la API y se mantiene desacoplado de las capas internas.


- `backend/Clientes.Api`: API con Clean Architecture, validaciones FluentValidation, AutoMapper y Serilog.
- `backend/Clientes.Infrastructure`: EF Core con proveedor Oracle Free 23c, migraciones y seeding.
- `clientes-web/src/app/clientes`: listado, formulario, guardas e interceptores.
- `backend/database/oracle/init.sql`: script DDL/seed ejecutado por Oracle Free.
- `docs/clientes.http`: ejemplos de consumo (VS Code REST Client / Hoppscotch).

## Tests

- `dotnet test` ejecuta unitarios + integración (InMemory provider).
- `npm test -- --watch=false` ejecuta unitarios Angular (Karma/Jasmine).

## Troubleshooting

- **Oracle tarda en inicializar**: `docker compose logs -f oracle` hasta ver estado `DATABASE IS READY TO USE!`.
- **Errores de conexión**: revisa la variable `ConnectionStrings__Oracle` en `docker-compose.yml` o variables de entorno.
- **Cambios sin Oracle**: establece `UseInMemoryDatabase=true` para usar provider InMemory (sólo desarrollo/pruebas).

## Licencia

MIT
