# Post Application - Clean Architecture with CQRS

A modern ASP.NET Core 8 application demonstrating clean architecture principles with CQRS pattern, built with SQLite and comprehensive features.

## Architecture

- **Domain Layer** - Core business entities and value objects
- **Application Layer** - Commands, Queries, Specifications, and Business Logic
- **Infrastructure Layer** - Data persistence, repositories, and EF Core
- **API Layer** - REST endpoints with Swagger documentation

## Features

✅ **Clean Architecture** - Strict layer separation with dependency injection
✅ **CQRS Pattern** - Query and Command separation for optimal scalability
✅ **Advanced Search** - Complex filtering, sorting, and pagination
✅ **Soft Delete** - Data preservation with logical deletion
✅ **Specification Pattern** - Reusable query logic with expression trees
✅ **Caching** - Memory caching with TTL for performance
✅ **Validation** - FluentValidation integration in MediatR pipeline
✅ **Exception Handling** - Global exception filter with custom exceptions
✅ **Request Logging** - HTTP request/response logging middleware
✅ **AutoMapper** - DTO mapping automation
✅ **Swagger UI** - Interactive API documentation

## Technology Stack

- **Framework**: ASP.NET Core 8
- **Database**: SQLite (configurable to SQL Server)
- **ORM**: Entity Framework Core
- **CQRS/Commands**: MediatR
- **Validation**: FluentValidation
- **Mapping**: AutoMapper
- **API Docs**: Swagger/OpenAPI

## Project Structure

```
PostApplication/
├── Post.Domain/                    # Core entities and value objects
│   ├── Entities/                   # Post, Category, Tag
│   └── ValueObjects/               # Slug
├── Post.Application/               # Business logic
│   ├── Features/Posts/
│   │   ├── Queries/               # GetPost, GetPosts, SearchPosts
│   │   ├── Commands/              # CreatePost, UpdatePost, DeletePost
│   │   ├── Responses/             # DTOs
│   │   └── Mapping/               # AutoMapper profiles
│   ├── Common/
│   │   ├── Behaviors/             # MediatR pipeline behaviors
│   │   ├── Specifications/        # Query specifications
│   │   ├── Interfaces/            # Repository contracts
│   │   └── Models/                # Pagination, Filtering, Sorting
├── Post.Infrastructure/            # Data access
│   ├── Repositories/              # Generic and specific implementations
│   ├── Persistence/               # DbContext and configurations
│   └── Seeds/                      # Database seeding
└── Post.Api/                       # REST API
    ├── Controllers/               # API endpoints
    ├── Extensions/                # DI registration
    ├── Filters/                   # Exception handling
    └── Middleware/                # Request logging
```

## API Endpoints

### Posts
- `GET /api/posts` - Get all posts
- `GET /api/posts/{id}` - Get post by ID
- `GET /api/posts/search` - Search posts with filters, sorting, pagination
- `POST /api/posts` - Create new post
- `PUT /api/posts/{id}` - Update post
- `DELETE /api/posts/{id}` - Delete post (soft delete)

### Search Filters
- `searchTerm` - Search in title and content
- `categoryId` - Filter by category
- `tagIds` - Filter by tags (multiple)
- `createdAfter` - Filter by date range (start)
- `createdBefore` - Filter by date range (end)
- `minViewCount` - Filter by minimum view count
- `sortBy` - Sort field (created, updated, viewCount)
- `sortDirection` - 0=Ascending, 1=Descending
- `pageNumber` - Page number (default: 1)
- `pageSize` - Page size (default: 10)

## Getting Started

### Prerequisites
- .NET 8 SDK
- Visual Studio 2022 or VS Code

### Build
```bash
dotnet build --configuration Release
```

### Run
```bash
dotnet run --configuration Release --project Post.Api
```

### Access Swagger UI
Navigate to: `https://localhost:5250/swagger`

## Database

### Initialize Database
The database is automatically created and seeded on application startup with:
- 5 Categories
- 10 Tags
- 10 Sample Posts

### Migrations
```bash
dotnet ef migrations add InitialCreate --project Post.Infrastructure
dotnet ef database update --project Post.Infrastructure
```

## MediatR Pipeline Behaviors

The application implements comprehensive pipeline behaviors in order:
1. **LoggingBehavior** - Request/response logging with correlation ID
2. **CachingBehavior** - Query result caching (5-minute TTL)
3. **AuthorizationBehavior** - Claims-based authorization (placeholder)
4. **ValidationBehavior** - FluentValidation integration
5. **PerformanceBehavior** - Slow query detection (>500ms warning)
6. **TransactionBehavior** - Automatic transactions for commands
7. **ExceptionBehavior** - Exception logging and categorization

## Response Format

All responses follow a consistent structure with proper HTTP status codes:

### Success Response (200 OK)
```json
{
  "id": "guid",
  "title": "string",
  "content": "string",
  "summary": "string",
  "slug": "string",
  "viewCount": 0,
  "categoryId": "guid",
  "createdAt": "2026-07-09T00:00:00Z",
  "updatedAt": "2026-07-09T00:00:00Z"
}
```

### Paginated Search Response
```json
{
  "items": [...],
  "totalCount": 100,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 10,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

### Error Response (400/404/500)
```json
{
  "type": "https://api.example.com/errors/not-found",
  "title": "Resource not found",
  "status": 404,
  "detail": "Post not found",
  "instance": "/api/posts/invalid-id"
}
```

## Key Design Decisions

### CQRS Pattern
Queries and Commands are completely separated:
- **Queries**: Read operations, optimized for retrieval, can be cached
- **Commands**: Write operations, include validation, trigger transactions

### Specification Pattern
Complex query logic is encapsulated in specification classes:
- Reusable across repositories
- Composable filtering with expression trees
- Supports includes, ordering, and paging

### Soft Delete
Posts support soft deletion:
- `IsDeleted` flag marks deleted records
- Queries automatically exclude soft-deleted records
- `GetAllIncludingDeletedAsync()` for admin operations

### Repository Pattern
Generic `IRepository<T>` with specific implementations:
- `IPostRepository` - Post-specific operations
- `ICategoryRepository` - Category-specific operations
- `ITagRepository` - Tag-specific operations

## Recent Changes (v1.1)

✨ **Clean Architecture Consolidation**
- Eliminated duplicate Request/Query classes
- Implemented proper CQRS with dedicated Command classes
- Consolidated response classes through inheritance
- Fixed all handler type references
- Updated controller to use Query objects for reads, Command objects for writes

## Contributing

This project demonstrates best practices in clean architecture and CQRS patterns. Feel free to use it as a reference for your own projects.

## License

MIT License - Feel free to use this as a template for your projects.
