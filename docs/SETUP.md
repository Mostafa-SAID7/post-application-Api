# Setup & Getting Started

## Prerequisites

- **.NET 8 SDK** - [Download](https://dotnet.microsoft.com/download)
- **IDE** - Visual Studio 2022, VS Code, or Rider
- **Git** - For version control

## Quick Start

```bash
# Clone & enter directory
git clone <repository-url>
cd PostApplication

# Restore dependencies
dotnet restore

# Build
dotnet build

# Run
dotnet run --project Post.Api
```

Application starts at: `https://localhost:5250`

## Database

### Automatic Setup
Database auto-initializes on first run with sample data:
- 5 Categories
- 10 Tags  
- 10 Sample Posts

### Migrations

```bash
# Create migration
dotnet ef migrations add MigrationName --project Post.Infrastructure

# Apply migration
dotnet ef database update --project Post.Infrastructure

# List migrations
dotnet ef migrations list --project Post.Infrastructure

# Remove last migration
dotnet ef migrations remove --project Post.Infrastructure
```

### Configuration

Edit `appsettings.json` connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=posts.db"
  }
}
```

**Switch to SQL Server:**
Update connection string and edit `InfrastructureServicesExtension.cs` EF Core provider.

## Configuration Files

- `appsettings.json` - Main settings
- `appsettings.Development.json` - Development overrides
- `launchSettings.json` - Run profiles

## Run Modes

```bash
# Development
dotnet run --project Post.Api

# Release
dotnet run --configuration Release --project Post.Api

# Publish
dotnet publish -c Release -o ./publish
```

## Environment Variables

```bash
# Windows (cmd)
set ASPNETCORE_ENVIRONMENT=Development

# Windows (PowerShell)
$env:ASPNETCORE_ENVIRONMENT="Development"

# Linux/macOS
export ASPNETCORE_ENVIRONMENT=Development
```

## Troubleshooting

| Issue | Solution |
|-------|----------|
| Build fails | `dotnet clean && dotnet restore && dotnet build` |
| Database error | Delete `posts.db` and rerun: `dotnet ef database update` |
| Port in use | Change port in `launchSettings.json` |
| EF tools missing | `dotnet tool install --global dotnet-ef` |

## Development Tips

**Request Logging** (auto-enabled in Development):
```
[INF] HTTP GET /api/posts → 200 (45ms)
```

**EF Core Query Logging** in `appsettings.Development.json`:
```json
{
  "Logging": {
    "LogLevel": {
      "Microsoft.EntityFrameworkCore.Database.Command": "Information"
    }
  }
}
```

**Cache** - Queries cached for 5 minutes. Restart app to clear.

## Next Steps

- [Architecture](./ARCHITECTURE.md) - Project design
- [API Reference](./API.md) - Endpoints
- [Contributing](./CONTRIBUTING.md) - Development standards
