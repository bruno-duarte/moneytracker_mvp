# MoneyTracker MVP (skeleton)

This repository contains a skeleton .NET 8 solution for the _Money Tracker_ MVP with DDD layers and a simple Kafka microservice.

## Projects

- `MoneyTracker.Api` - main API containing Domain, Application, Infrastructure and API layers.
- `MoneyTracker.EventsService` - background worker that produces and consumes Kafka events.
- `MoneyTracker.Tests` - minimal unit tests.

## How to run (basic)

1. Install the .NET 8 SDK.
2. Start services with Docker Compose:
   ```bash
   docker compose up -d
   ```
   This will start PostgreSQL, Zookeeper and Kafka.
3. Configure connection strings in `src/MoneyTracker.Api/appsettings.Development.json`.
4. Restore and build:
   ```bash
   dotnet restore
   dotnet build
   ```
5. Apply EF Core migrations (from `src/MoneyTracker.Api`):
   ```bash
   dotnet tool install --global dotnet-ef
   dotnet ef migrations add Initial -p src/MoneyTracker.Api -s src/MoneyTracker.Api
   dotnet ef database update -p src/MoneyTracker.Api -s src/MoneyTracker.Api
   ```
6. Run the API:
   ```bash
   dotnet run --project src/MoneyTracker.Api
   ```
   Swagger UI will be available at `http://localhost:5000/swagger`.

## Notes

- This is a starting skeleton. Implement business rules, add real migrations, and customize as needed.
