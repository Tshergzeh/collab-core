# collab-core
A backend system (built in ASP.NET Core with C#) that powers a simple project management and collaboration platform

## TL;DR
- Clone repo
- Run `docker-compose up -d`
- Run `dotnet run --project src/CollabCore.Api`
- Visit https://localhost:7000/swagger

## Tech Stack
- ASP.Net Core 8.0 Web Api
- Entity Framework Core + SQL Server
- JWT Authentication
- Redis Caching
- MongoDB (Comments)
- Docker & Docker Compose

## Quick Start
1. Start dependencies: `docker-compose up -d`
2. Update connection strings in `appSettings.Development.json`
3. Run migrations: `dotnet ef database update --project src/CollabCore.Infrastructure --startup-project src/CollabCore.Api`
4. Run API: `dotnet run --project src/CollabCore.Api`
5. Open Swagger: https:localhost:7000
