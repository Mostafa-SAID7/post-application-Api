using Post.Api.Extensions;
using Post.Api.Filters;
using Post.Api.Middleware;
using Post.Infrastructure.Persistence;
using Post.Infrastructure.Persistence.Seeds;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAllServices(builder.Configuration);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Explicitly set WebRootPath so it is always correct regardless of working directory
builder.Environment.WebRootPath =
    Path.Combine(builder.Environment.ContentRootPath, "wwwroot");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.ApplyMigrationsAndSeedAsync();
}

// Static files (explicit FileProvider so path is always resolved correctly)
var wwwroot = Path.Combine(app.Environment.ContentRootPath, "wwwroot");
var fileProvider = new PhysicalFileProvider(wwwroot);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = fileProvider,
    RequestPath  = ""
});

app.UseMiddleware<RequestLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

// Serve index.html for the root path explicitly
app.MapGet("/", async (HttpContext ctx) =>
{
    ctx.Response.ContentType = "text/html; charset=utf-8";
    await ctx.Response.SendFileAsync(Path.Combine(wwwroot, "index.html"));
});

app.MapControllers();

// Fallback: unknown /api paths → JSON 404; everything else → 404.html
app.MapFallback(async (HttpContext ctx) =>
{
    if (ctx.Request.Path.StartsWithSegments("/api"))
    {
        ctx.Response.StatusCode  = 404;
        ctx.Response.ContentType = "application/json";
        await ctx.Response.WriteAsync("{\"error\":\"Not Found\"}");
    }
    else
    {
        ctx.Response.StatusCode  = 404;
        ctx.Response.ContentType = "text/html";
        var file = Path.Combine(app.Environment.WebRootPath ?? wwwroot, "404.html");
        if (File.Exists(file))
            await ctx.Response.SendFileAsync(file);
    }
});

app.Run();
