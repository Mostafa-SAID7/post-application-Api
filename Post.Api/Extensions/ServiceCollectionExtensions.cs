namespace Post.Api.Extensions
{
    /// <summary>
    /// Main extension orchestrator for all DI services
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAllServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddApplicationServices()
                .AddInfrastructureServices(configuration);
        }
    }
}
