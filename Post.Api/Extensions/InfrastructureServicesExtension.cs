using Microsoft.EntityFrameworkCore;
using Post.Application.Common.Interfaces;
using Post.Infrastructure.Persistence;
using Post.Infrastructure.Repositories;

namespace Post.Api.Extensions
{
    /// <summary>
    /// Extension methods for registering infrastructure layer services
    /// </summary>
    public static class InfrastructureServicesExtension
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddDatabaseServices(configuration)
                .AddRepositoryServices()
                .AddUnitOfWorkServices();
        }

        private static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Use SQLite if connection string looks like a file path
            if (connectionString?.Contains(".db") == true || connectionString?.Contains(".sqlite") == true)
            {
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlite(connectionString,
                        b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
            }
            else
            {
                // Use SQL Server as default
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(connectionString,
                        b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
            }

            return services;
        }

        private static IServiceCollection AddRepositoryServices(this IServiceCollection services)
        {
            // Generic repository
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Specific repositories
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITagRepository, TagRepository>();

            return services;
        }

        private static IServiceCollection AddUnitOfWorkServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
