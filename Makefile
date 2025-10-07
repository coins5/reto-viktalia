.PHONY: up down logs test-backend test-frontend lint format

up:
	docker compose up -d --build

down:
	docker compose down

logs:
	docker compose logs -f

test-backend:
	cd backend && dotnet test

test-frontend:
	cd clientes-web && npm test -- --watch=false

lint:
	cd clientes-web && npm run lint

format:
	cd clientes-web && npm run format
