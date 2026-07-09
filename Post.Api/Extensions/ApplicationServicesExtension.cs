using FluentValidation;
using MediatR;
using Post.Application.Common.Behaviors;
using Post.Application.Features.Posts.Commands.CreatePost;
using Post.Application.Features.Posts.Mapping;
using Post.Application.Features.Posts.Queries.GetPost;

namespace Post.Api.Extensions
{
    /// <summary>
    /// Registers Application-layer services: AutoMapper, MediatR pipeline, FluentValidation.
    ///
    /// Pipeline behavior order (outer → inner):
    ///   LoggingBehavior     — traces every request with timing
    ///   CachingBehavior     — returns cached response when [Cacheable] is present
    ///   ValidationBehavior  — runs FluentValidation before the handler
    ///   PerformanceBehavior — warns when a request exceeds 500 ms
    ///   TransactionBehavior — logs command execution scope (queries bypass it)
    ///
    /// Removed behaviors:
    ///   AuthorizationBehavior — was a TODO stub with no real logic; zero security value
    ///   ExceptionBehavior     — duplicated ExceptionFilter; logging is already in LoggingBehavior
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
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
            });

            services.AddValidatorsFromAssembly(typeof(CreatePostCommandValidator).Assembly);

            return services;
        }
    }
}
