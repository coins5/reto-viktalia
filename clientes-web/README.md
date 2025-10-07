# Frontend - Clientes Web

Aplicación Angular 17 con Angular Material para la gestión de clientes.

## Requisitos

- Node.js 20
- npm 10

## Instalación y scripts

```bash
npm ci
npm run start      # arranca en http://localhost:4200
npm run build      # compila modo producción
npm run test       # ejecuta pruebas unitarias (Karma + Jasmine)
npm run lint       # verifica reglas ESLint
npm run format     # formatea con Prettier
```

## Configuración

Variables en `src/environments/`:

- `environment.development.ts`: `apiUrl` apunta al backend en `http://localhost:8080/api`.
- `environment.ts` (producción): usa `/api` y depende del reverse proxy (nginx).

## Arquitectura

- `clientes/` contiene modelos, servicios, guardas y páginas (listado y formulario).
- Se usa `ClientesService` para consumir el microservicio con filtros, paginación y ordenamiento.
- `PendingChangesGuard` protege el formulario ante cambios no guardados.
- Interceptores globales:
  - `api-base-url.interceptor`: adjunta la base URL del API.
  - `error.interceptor`: muestra errores en `MatSnackBar` con `TraceId`.

## Docker

```bash
docker build -t clientes-web .
docker run -p 4200:4200 clientes-web
```

El contenedor nginx redirige `/api` hacia el servicio backend (`api:8080`).
