# Features

## Core Capabilities

### Clean Architecture
Four-layer separation with downward dependencies only:
- **Domain**: Business entities and rules (no external dependencies)
- **Application**: Orchestration and DTOs
- **Infrastructure**: Data persistence (pluggable implementations)
- **API**: REST endpoints and middleware

### CQRS Pattern
- **Commands**: Write operations with validation and transactions
- **Queries**: Read operations with caching
- Separate models prevent data inconsistency

### Search & Filtering
Search with multiple simultaneous criteria:
- Full-text search (title + content)
- Filter by category, tags, date range, view count
- Sort by created date, updated date, or view count
- Configurable pagination

### Soft Delete
Logical deletion (records marked deleted, not removed):
- Maintains data integrity
- Automatic exclusion from queries
- Admin access to deleted records available

### Specification Pattern
Type-safe, reusable query logic:
```csharp
var spec = new PostSearchSpecification(searchTerm, categoryId);
var posts = await repository.GetAsync(spec);
```

### Intelligent Caching
- Query results cached for 5 minutes
- In-process .NET Memory Cache
- Auto-invalidated on create/update/delete

### Validation
- FluentValidation rules
- MediatR pipeline integration
- Custom business rule validation
- Detailed error messages

### Exception Handling
- Global exception filter
- Consistent error responses (RFC 7807)
- Appropriate HTTP status codes
- No sensitive info leakage

### Request Logging
- All HTTP requests logged with execution time
- Correlation IDs for request tracing

### AutoMapper Integration
- Automatic Entity → DTO conversion
- Type-safe mappings
- Nested object support

### Swagger/OpenAPI
- Interactive API documentation at `/swagger`
- Try-it-out functionality
- Auto-generated from code

## MediatR Pipeline Behaviors

Executed in order for each request:

1. **LoggingBehavior** - Request/response logging
2. **CachingBehavior** - Query caching (queries only)
3. **ValidationBehavior** - Input validation
4. **AuthorizationBehavior** - Placeholder for auth
5. **PerformanceBehavior** - Execution time monitoring
6. **TransactionBehavior** - Wraps commands in transactions
7. **ExceptionBehavior** - Exception handling

## Database Models

### Post
- `Id`, `Title`, `Content`, `Summary`
- `Slug` (value object, URL-friendly)
- `ViewCount`
- `IsDeleted` (soft delete flag)
- `CategoryId` (foreign key)
- `Tags` (many-to-many)
- `CreatedAt`, `UpdatedAt`

### Category
- `Id`, `Name`, `Description`
- `Posts` (navigation, 1-to-many)

### Tag
- `Id`, `Name`, `Description`
- `Posts` (navigation, many-to-many)

## Pagination

All list endpoints support:
- `pageNumber` (default: 1)
- `pageSize` (default: 10)

Response includes:
- `items` - Actual data
- `totalCount` - Total items in database
- `pageNumber`, `pageSize`, `totalPages`
- `hasPreviousPage`, `hasNextPage`

## Database Seeding

Auto-initialized on startup:
- **5 Categories** - Technology, Business, Lifestyle, Education, Entertainment
- **10 Tags** - C#, .NET, Clean Code, Design Patterns, CQRS, Architecture, Testing, Performance, Security, REST API
- **10 Sample Posts** - Various content, categories, and tags

## API Endpoints

See [API Reference](./API.md) for complete endpoint documentation.

### Quick Reference
- **GET** `/api/posts` - List all
- **GET** `/api/posts/{id}` - Get one
- **GET** `/api/posts/search` - Search with filters
- **POST** `/api/posts` - Create
- **PUT** `/api/posts/{id}` - Update
- **DELETE** `/api/posts/{id}` - Delete (soft)

Similar structure for `/api/categories` and `/api/tags`.

## Technology Stack

| Component | Technology |
|-----------|-----------|
| Framework | ASP.NET Core 8 |
| Database | SQLite (SQL Server optional) |
| ORM | Entity Framework Core 8 |
| CQRS | MediatR 12.x |
| Validation | FluentValidation 11.x |
| Mapping | AutoMapper 12.x |
| Documentation | Swagger/OpenAPI |
| Logging | Configured in appsettings |

## Security Considerations

**Implemented:**
- ✅ Input validation (FluentValidation)
- ✅ Exception handling (no sensitive data leakage)
- ✅ Soft delete (data preservation)

**Recommended for Production:**
- JWT authentication
- Role-based authorization
- HTTPS enforcement
- CORS configuration
- Rate limiting
- Request size limits
- OWASP compliance review
