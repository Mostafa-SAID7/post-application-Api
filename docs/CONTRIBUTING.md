# Contributing Guide

## Code Style & Standards

### Naming Conventions
- **Classes/Interfaces**: PascalCase
- **Methods/Properties**: PascalCase
- **Private fields**: camelCase with underscore prefix (`_field`)
- **Parameters**: camelCase
- **Constants**: UPPER_SNAKE_CASE

### File Organization

Each feature in `Post.Application/Features/` follows consistent structure:
```
Features/Posts/
├── Commands/
│   ├── CreatePost/
│   │   ├── CreatePostCommand.cs
│   │   ├── CreatePostCommandHandler.cs
│   │   └── CreatePostCommandValidator.cs
│   ├── UpdatePost/
│   └── DeletePost/
├── Queries/
│   ├── GetPost/
│   ├── GetPosts/
│   └── SearchPosts/
├── Responses/
│   ├── PostDetailResponse.cs
│   └── PostListResponse.cs
└── Mapping/
    └── PostProfile.cs
```

### Folder Naming
- **Plural** for collections: `Commands`, `Queries`, `Repositories`
- **Singular** for specific handlers/classes: `CreatePost/`, `GetPost/`

## CQRS Guidelines

### Queries
- **Purpose**: Read operations only, no side effects
- **Caching**: Eligible for caching (implement in `CachingBehavior`)
- **Validation**: Basic parameter validation only
- **Handler**: Single query handler per query class

```csharp
public class GetPostQuery : IRequest<PostDetailResponse>
{
    public Guid Id { get; set; }
}

public class GetPostQueryHandler : IRequestHandler<GetPostQuery, PostDetailResponse>
{
    public async Task<PostDetailResponse> Handle(GetPostQuery request, CancellationToken cancellationToken)
    {
        var post = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return _mapper.Map<PostDetailResponse>(post);
    }
}
```

### Commands
- **Purpose**: Write operations with business logic
- **Validation**: Comprehensive validation with validator class
- **Transaction**: Wrapped automatically by `TransactionBehavior`
- **Handler**: Single command handler per command class

```csharp
public class CreatePostCommand : IRequest<PostDetailResponse>
{
    public string Title { get; set; }
    public string Content { get; set; }
    public Guid CategoryId { get; set; }
}

public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Content).NotEmpty().MinimumLength(10);
    }
}

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, PostDetailResponse>
{
    public async Task<PostDetailResponse> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var post = new Post { Title = request.Title, /* ... */ };
        await _repository.AddAsync(post, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<PostDetailResponse>(post);
    }
}
```

## Repository Pattern

### Generic Repository Usage
```csharp
// Generic operations
await repository.AddAsync(entity, cancellationToken);
await repository.UpdateAsync(entity, cancellationToken);
await repository.DeleteAsync(entity, cancellationToken);
var entity = await repository.GetByIdAsync(id, cancellationToken);
var items = await repository.GetAsync(spec, cancellationToken);
```

### Specification Pattern
```csharp
public class PostSearchSpecification : Specification<Post>
{
    public PostSearchSpecification(string searchTerm, Guid? categoryId)
    {
        Query.Where(x => !x.IsDeleted);
        
        if (!string.IsNullOrWhiteSpace(searchTerm))
            Query.Where(x => x.Title.Contains(searchTerm) || x.Content.Contains(searchTerm));
            
        if (categoryId.HasValue)
            Query.Where(x => x.CategoryId == categoryId.Value);
    }
}

var spec = new PostSearchSpecification(searchTerm, categoryId);
var posts = await repository.GetAsync(spec, cancellationToken);
```

## Entity Framework Core

### Entity Configuration
Define entity mappings in `Infrastructure/Persistence/Configurations/`:

```csharp
public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(256);
            
        builder.Property(x => x.Slug)
            .HasConversion(
                x => x.Value,
                x => Slug.Create(x));
                
        builder.HasOne(x => x.Category)
            .WithMany(x => x.Posts)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
```

### Migrations
```bash
# Create migration
dotnet ef migrations add FeatureName --project Post.Infrastructure

# Apply migration
dotnet ef database update --project Post.Infrastructure

# Rollback
dotnet ef migrations remove --project Post.Infrastructure
```

## Validation

### FluentValidation Rules
```csharp
public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(256).WithMessage("Title cannot exceed 256 characters");
            
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required")
            .MinimumLength(10).WithMessage("Content must be at least 10 characters");
            
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category is required");
    }
}
```

### Custom Validation
```csharp
public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    private readonly IPostRepository _repository;
    
    public CreatePostCommandValidator(IPostRepository repository)
    {
        _repository = repository;
        
        RuleFor(x => x.CategoryId)
            .MustAsync(CategoryExists, "Category not found")
            .WithMessage("Selected category does not exist");
    }
    
    private async Task<bool> CategoryExists(Guid categoryId, CancellationToken ct)
    {
        return await _repository.CategoryExistsAsync(categoryId, ct);
    }
}
```

## AutoMapper Configuration

### Profile Definition
```csharp
public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<Post, PostDetailResponse>()
            .ForMember(x => x.CategoryName, opt => opt.MapFrom(x => x.Category.Name))
            .ForMember(x => x.Tags, opt => opt.MapFrom(x => x.Tags.Select(t => t.Name)));
            
        CreateMap<CreatePostCommand, Post>();
    }
}
```

## Exception Handling

### Custom Exceptions
```csharp
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}
```

### Usage in Handlers
```csharp
public async Task<PostDetailResponse> Handle(GetPostQuery request, CancellationToken cancellationToken)
{
    var post = await _repository.GetByIdAsync(request.Id, cancellationToken);
    
    if (post == null)
        throw new NotFoundException($"Post with ID {request.Id} not found");
        
    return _mapper.Map<PostDetailResponse>(post);
}
```

## Testing

### Unit Test Structure
```
PostApplication.Tests/
├── Application/
│   └── Features/Posts/
│       ├── Commands/
│       │   └── CreatePostCommandHandlerTests.cs
│       └── Queries/
│           └── GetPostQueryHandlerTests.cs
└── Infrastructure/
    └── Repositories/
        └── PostRepositoryTests.cs
```

### Test Naming Convention
```csharp
[TestClass]
public class CreatePostCommandHandlerTests
{
    [TestMethod]
    public async Task Handle_ValidCommand_CreatesPost()
    {
        // Arrange
        var command = new CreatePostCommand { Title = "Test", /* ... */ };
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Test", result.Title);
    }
}
```

## Commit Message Convention

Follow conventional commits:

```
<type>(<scope>): <subject>

<body>

<footer>
```

**Types:**
- `feat`: New feature
- `fix`: Bug fix
- `refactor`: Code refactoring
- `docs`: Documentation changes
- `test`: Test additions/changes
- `chore`: Build, dependencies, configuration

**Examples:**
```
feat(posts): add advanced search with multiple filters

fix(validation): improve error messages in CreatePostCommandValidator

docs(api): update endpoint documentation

refactor(repositories): consolidate duplicate query logic
```

## Pull Request Process

1. **Create Branch**: `git checkout -b feature/feature-name`
2. **Make Changes**: Follow code style and standards
3. **Test**: Ensure all tests pass
4. **Commit**: Use conventional commit messages
5. **Push**: `git push origin feature/feature-name`
6. **Create PR**: Include description of changes
7. **Code Review**: Address feedback
8. **Merge**: Squash and merge to main

## Performance Considerations

### Query Optimization
- Use `Specification` pattern for efficient queries
- Use `Select()` for projections (return only needed fields)
- Include related entities only when needed (avoid N+1 queries)
- Use `.AsNoTracking()` for read-only queries

### Caching
- Cache frequently accessed queries (5-minute default TTL)
- Invalidate cache on create/update/delete
- Monitor cache hit rates

### Async/Await
- Use async methods for I/O operations
- Use `CancellationToken` throughout the call chain
- Avoid `.Result` or `.Wait()`

## Documentation

### Code Comments
- Document complex business logic
- Explain "why", not "what" (code should be self-explanatory)
- Keep comments up-to-date with code changes

### XML Documentation
```csharp
/// <summary>
/// Retrieves all posts with optional filtering and sorting.
/// </summary>
/// <param name="specification">Query specification with filters and sorting</param>
/// <param name="cancellationToken">Cancellation token</param>
/// <returns>List of posts matching the specification</returns>
public async Task<List<Post>> GetAsync(
    Specification<Post> specification,
    CancellationToken cancellationToken)
{
    // Implementation
}
```

## Resources

- [Clean Architecture](./ARCHITECTURE.md)
- [API Reference](./API.md)
- [Features Overview](./FEATURES.md)
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core/)
- [MediatR Documentation](https://github.com/jbogard/MediatR)
- [FluentValidation Documentation](https://docs.fluentvalidation.net/)
