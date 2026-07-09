using Post.Api.Extensions;
using Post.Api.Filters;
using Post.Api.Middleware;
using Post.Infrastructure.Persistence;
using Post.Infrastructure.Persistence.Seeds;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add all services (Application + Infrastructure)
builder.Services.AddAllServices(builder.Configuration);

// Add API layer services
builder.Services.AddControllers(options =>
{
    // Register global exception filter
    options.Filters.Add<ExceptionFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply migrations and seed database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.ApplyMigrationsAndSeedAsync();
}

// Configure middleware pipeline
app.UseMiddleware<RequestLoggingMiddleware>();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
