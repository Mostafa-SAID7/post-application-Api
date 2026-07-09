# Architecture

## Overview

Post Application follows **Clean Architecture** with **CQRS pattern** for separation of concerns and scalability.

## Layers

| Layer | Responsibility | Technologies |
|-------|-----------------|---------------|
| **Domain** | Business entities, value objects | Pure C# |
| **Application** | CQRS, business logic, DTOs, mapping | MediatR, FluentValidation, AutoMapper |
| **Infrastructure** | Data persistence, repositories | EF Core, SQLite/SQL Server |
| **API** | REST endpoints, middleware, filters | ASP.NET Core 8 |

## Project Structure

```
Post.Domain/
├── Entities/          # Post, Category, Tag
└── ValueObjects/      # Slug

Post.Application/
├── Features/Posts/
│   ├── Queries/       # GetPost, GetPosts, SearchPosts
│   ├── Commands/      # CreatePost, UpdatePost, DeletePost
│   ├── Responses/     # PostDetailResponse, PostListResponse
│   └── Mapping/       # PostProfile
├── Common/
│   ├── Behaviors/     # MediatR pipeline
│   ├── Interfaces/    # IRepository, IUnitOfWork
│   ├── Models/        # PaginationParams
│   └── Specifications/# Query specs
└── Features/[Category|Tag]/  # Similar structure

Post.Infrastructure/
├── Repositories/      # Repository implementations
├── Persistence/
│   ├── AppDbContext.cs
│   └── Configurations/
└── Seeds/             # Database initialization

Post.Api/
├── Controllers/       # REST endpoints
├── Extensions/        # DI registration
├── Filters/           # Exception handling
└── Middleware/        # Request logging
```

## Key Patterns

### CQRS (Command Query Responsibility Segregation)
- **Queries**: Read-only, cacheable, no side effects
- **Commands**: Write operations, validated, transactional
- Complete separation prevents inconsistency

### Repository Pattern
- Generic `IRepository<T>` for CRUD operations
- Specific repositories: `IPostRepository`, `ICategoryRepository`, `ITagRepository`
- Abstraction over Entity Framework

### Specification Pattern
- Encapsulates complex queries
- Reusable filter logic
- Built with LINQ expression trees
- Supports sorting, paging, includes

### MediatR Pipeline
Request handlers with cross-cutting behaviors:
1. Logging
2. Caching (queries only)
3. Validation
4. Authorization
5. Performance monitoring
6. Transactions (commands only)
7. Exception handling

## Data Access Flow

### Query Path
```
Controller → Query → MediatR Pipeline
  ↓ (Logging, Caching Check)
  Query Handler → Repository with Specification
  ↓
  EF Core → Database
  ↓
  AutoMapper (Entity → DTO)
  ↓
  Response Object
```

### Command Path
```
Controller → Command → MediatR Pipeline
  ↓ (Validation, Authorization, Begin Transaction)
  Command Handler → Repository
  ↓
  EF Core → Database
  ↓
  UnitOfWork.SaveChanges() → Commit Transaction
  ↓
  AutoMapper (Entity → DTO)
  ↓
  Response Object
```

## Dependency Injection

Services registered in `Program.cs` via extensions:
- `ApplicationServicesExtension` - Application layer
- `InfrastructureServicesExtension` - Infrastructure layer

See [Contributing](./CONTRIBUTING.md) for extension patterns.

## Technology Decisions

| Decision | Reason |
|----------|--------|
| Clean Architecture | Testability, maintainability, flexibility |
| CQRS | Separate read/write concerns, optimized paths |
| MediatR | Decoupled request handling, pipeline behaviors |
| Specification Pattern | Reusable complex query logic |
| Entity Framework Core | LINQ queries, migrations, database agnostic |
| SQLite Default | Simple setup, file-based, easy to reset |
