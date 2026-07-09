using FluentValidation;
using MediatR;
using Post.Application.Common.Behaviors;
using Post.Application.Features.Posts.Commands.CreatePost;
using Post.Application.Features.Posts.Mapping;
using Post.Application.Features.Posts.Queries.GetPost;

namespace Post.Api.Extensions
{
    /// <summary>
    /// Extension methods for registering application layer services
    /// </summary>
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(PostProfile).Assembly);
            services.AddMemoryCache();

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(GetPostQuery).Assembly);
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ExceptionBehavior<,>));
            });

            services.AddValidatorsFromAssembly(typeof(CreatePostCommandValidator).Assembly);

            return services;
        }
    }
}
