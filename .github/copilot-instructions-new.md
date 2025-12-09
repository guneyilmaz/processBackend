# .NET 8 Clean Architecture Backend Project

This project implements Clean Architecture with the following features:
- 🔹 Clean Architecture + Entity Framework Core
- 🔹 JWT Authentication
- 🔹 MSSQL & PostgreSQL support
- 🔹 Swagger / OpenAPI
- 🔹 Docker & Azure compatible
- 🔹 Angular integration ready

## Project Setup Complete ✅

All features have been successfully implemented:

- ✅ Clean Architecture structure (Domain, Application, Infrastructure, WebAPI)
- ✅ Entity Framework Core with MSSQL/PostgreSQL support
- ✅ JWT Authentication and Authorization
- ✅ Swagger/OpenAPI documentation with authentication
- ✅ Docker and Docker Compose configuration
- ✅ Azure deployment templates and CI/CD workflows
- ✅ CORS configuration for Angular integration
- ✅ Health check endpoints
- ✅ Repository Pattern and Unit of Work
- ✅ Comprehensive documentation

## Getting Started

1. Run `dotnet restore` to install dependencies
2. Update connection string in `appsettings.json`
3. Run `dotnet run` from ProcessModule.WebAPI directory
4. Visit http://localhost:5024 for Swagger UI

## API Endpoints

- **Authentication**: `/api/auth/login`, `/api/auth/register`
- **Processes**: `/api/process` (CRUD operations)
- **Health**: `/health`

## Docker

```bash
docker-compose up -d  # Full stack with database
```

## Azure Deployment

Use the provided ARM template `azure-deploy.json` for Azure Container Apps deployment.

The project is ready for production use!