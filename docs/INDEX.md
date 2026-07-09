# Documentation

**Start here:** Pick your role or task below.

## By Role

**🆕 New to Project?**
1. [Setup](./SETUP.md) - Get running
2. [Architecture](./ARCHITECTURE.md) - Understand design
3. [Features](./FEATURES.md) - What it does

**🔨 Adding a Feature?**
1. [Contributing](./CONTRIBUTING.md) - Standards
2. [Architecture](./ARCHITECTURE.md) - Patterns
3. [API](./API.md) - Endpoints

**🔌 Using This API?**
1. [API Reference](./API.md) - All endpoints
2. [Features](./FEATURES.md) - Capabilities
3. [Swagger UI](https://localhost:5250/swagger) - Test it

**🐛 Debugging?**
1. [Setup](./SETUP.md#troubleshooting) - Common issues
2. [Architecture](./ARCHITECTURE.md) - Data flow
3. [Contributing](./CONTRIBUTING.md) - Code structure

## Documentation Files

| File | Purpose |
|------|---------|
| **[SETUP.md](./SETUP.md)** | Install, configure, run, troubleshoot |
| **[FEATURES.md](./FEATURES.md)** | What it does, capabilities, models |
| **[ARCHITECTURE.md](./ARCHITECTURE.md)** | Design, patterns, structure |
| **[API.md](./API.md)** | Endpoints, request/response, examples |
| **[CONTRIBUTING.md](./CONTRIBUTING.md)** | Code standards, conventions, workflow |

## Quick Reference

### Commands
```bash
# Install & run
dotnet restore && dotnet build
dotnet run --project Post.Api

# Database
dotnet ef migrations add Name --project Post.Infrastructure
dotnet ef database update --project Post.Infrastructure
```

### URLs
- **API**: `https://localhost:5250/api`
- **Swagger**: `https://localhost:5250/swagger`
- **Database**: SQLite file (`posts.db`)

### Key Patterns
- **CQRS** - Queries (read) + Commands (write)
- **Repository** - Data access abstraction
- **Specification** - Reusable query logic
- **MediatR** - Request pipeline with behaviors

## Tech Stack

- ASP.NET Core 8
- Entity Framework Core + SQLite
- MediatR + FluentValidation
- AutoMapper
- Swagger/OpenAPI

## Core Features

✅ Clean Architecture | ✅ CQRS Pattern | ✅ Advanced Search
✅ Soft Delete | ✅ Specification Pattern | ✅ Caching
✅ Validation | ✅ Exception Handling | ✅ Request Logging
✅ AutoMapper | ✅ Swagger UI

## File Structure

```
docs/
├── INDEX.md              ← You are here
├── SETUP.md              ← Installation & configuration
├── FEATURES.md           ← Feature descriptions
├── ARCHITECTURE.md       ← System design
├── API.md                ← REST API reference
└── CONTRIBUTING.md       ← Development guidelines
```

Root also has:
- **README.md** - Project overview
- **CONTRIBUTING.md** - How to contribute
- **CHANGELOG.md** - Version history
- **SECURITY.md** - Security policy
- **LICENSE** - MIT License

## Quick Tasks

### Run Locally
See [Setup](./SETUP.md)

### Use the API
See [API Reference](./API.md)

### Add a Feature
See [Contributing](./CONTRIBUTING.md)

### Understand the Code
See [Architecture](./ARCHITECTURE.md)

### Fix an Issue
See [Setup Troubleshooting](./SETUP.md#troubleshooting)

---

**Need something specific?** Check the file that matches your task above.
