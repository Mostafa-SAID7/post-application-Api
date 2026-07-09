# Post Application

A modern **ASP.NET Core 8** application demonstrating clean architecture principles with CQRS pattern, built with SQLite and comprehensive features.

## Quick Start

```bash
# Install dependencies
dotnet restore

# Run application
dotnet run --project Post.Api

# Access Swagger UI
https://localhost:5250/swagger
```

**Full setup guide**: See [Setup & Getting Started](./docs/SETUP.md)

## Key Features

✅ **Clean Architecture** - Strict layer separation  
✅ **CQRS Pattern** - Query and Command separation  
✅ **Advanced Search** - Filtering, sorting, pagination  
✅ **Soft Delete** - Data preservation  
✅ **Specification Pattern** - Reusable query logic  
✅ **Caching** - In-process caching with TTL  
✅ **Validation** - FluentValidation pipeline  
✅ **Exception Handling** - Global error filtering  
✅ **Request Logging** - HTTP middleware logging  
✅ **AutoMapper** - DTO mapping  
✅ **Swagger UI** - Interactive API docs  

## Technology Stack

- **Framework**: ASP.NET Core 8
- **Database**: SQLite (configurable)
- **ORM**: Entity Framework Core
- **CQRS**: MediatR
- **Validation**: FluentValidation
- **Mapping**: AutoMapper
- **Documentation**: Swagger/OpenAPI

## Documentation

All documentation is organized in the `/docs` folder:

| Document | Purpose |
|----------|---------|
| [**docs/INDEX.md**](./docs/INDEX.md) | Documentation overview and navigation |
| [**docs/SETUP.md**](./docs/SETUP.md) | Installation, configuration, troubleshooting |
| [**docs/FEATURES.md**](./docs/FEATURES.md) | Complete feature descriptions |
| [**docs/ARCHITECTURE.md**](./docs/ARCHITECTURE.md) | System design and patterns |
| [**docs/API.md**](./docs/API.md) | REST API reference and examples |
| [**docs/CONTRIBUTING.md**](./docs/CONTRIBUTING.md) | Development guidelines and conventions |

**Start here**: [Documentation Index](./docs/INDEX.md)

## Project Structure

```
PostApplication/
├── Post.Domain/                  # Entities and value objects
├── Post.Application/             # Business logic (Commands, Queries)
├── Post.Infrastructure/          # Data access (Repositories, DbContext)
├── Post.Api/                     # REST API (Controllers, Middleware)
└── docs/                         # Documentation
```

See [Architecture Guide](./docs/ARCHITECTURE.md) for detailed structure.

## API Endpoints

**Posts:**
- `GET /api/posts` - Get all posts (paginated)
- `GET /api/posts/{id}` - Get post by ID
- `GET /api/posts/search` - Search with filters, sorting, pagination
- `POST /api/posts` - Create new post
- `PUT /api/posts/{id}` - Update post
- `DELETE /api/posts/{id}` - Delete post (soft delete)

**Categories & Tags:**
- `GET /api/categories`, `GET /api/tags` - List resources
- `GET /api/categories/{id}`, `GET /api/tags/{id}` - Get by ID

See [API Reference](./docs/API.md) for complete endpoint documentation.

## Development

### Build
```bash
dotnet build
```

### Run Tests
```bash
dotnet test
```

### Database Migrations
```bash
dotnet ef migrations add MigrationName --project Post.Infrastructure
dotnet ef database update --project Post.Infrastructure
```

See [Setup Guide](./docs/SETUP.md) and [Contributing Guide](./docs/CONTRIBUTING.md) for more details.

## License

MIT License - Free to use as a template or reference project.
