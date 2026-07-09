using MediatR;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Post.Application.Common.Behaviors
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AuthorizeAttribute : Attribute
    {
        public string? Roles { get; set; }
        public string? Policies { get; set; }
    }

    public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<AuthorizationBehavior<TRequest, TResponse>> _logger;

        public AuthorizationBehavior(ILogger<AuthorizationBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var authorizeAttribute = typeof(TRequest)
                .GetCustomAttribute<AuthorizeAttribute>();

            if (authorizeAttribute == null)
            {
                // No authorization required
                return await next();
            }

            _logger.LogInformation(
                "Authorization check required for {RequestName}. Roles: {Roles}, Policies: {Policies}",
                typeof(TRequest).Name, authorizeAttribute.Roles ?? "None", authorizeAttribute.Policies ?? "None");

            // TODO: Implement actual authorization logic here
            // For now, just log and allow - integrate with identity/claims system
            _logger.LogInformation("Authorization check passed for {RequestName}", typeof(TRequest).Name);

            return await next();
        }
    }
}
