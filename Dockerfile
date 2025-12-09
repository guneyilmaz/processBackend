# Use the official .NET 8 SDK image as build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY ProcessModuleBackend.sln ./

# Copy project files
COPY ProcessModule.Domain/ProcessModule.Domain.csproj ProcessModule.Domain/
COPY ProcessModule.Application/ProcessModule.Application.csproj ProcessModule.Application/
COPY ProcessModule.Infrastructure/ProcessModule.Infrastructure.csproj ProcessModule.Infrastructure/
COPY ProcessModule.WebAPI/ProcessModule.WebAPI.csproj ProcessModule.WebAPI/

# Restore dependencies
RUN dotnet restore

# Copy all source code
COPY . .

# Build the application
WORKDIR /src/ProcessModule.WebAPI
RUN dotnet build -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Use the official .NET 8 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create a non-root user
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

# Copy the published app
COPY --from=publish /app/publish .

# Expose port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

# Start the application
ENTRYPOINT ["dotnet", "ProcessModule.WebAPI.dll"]