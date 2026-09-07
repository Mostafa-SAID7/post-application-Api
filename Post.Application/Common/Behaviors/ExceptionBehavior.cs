using MediatR;
using Microsoft.Extensions.Logging;
using Post.Application.Common.Exceptions;

namespace Post.Application.Common.Behaviors
{
    public class ExceptionBehavior<TRequest, TResponse>(ILogger<ExceptionBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<ExceptionBehavior<TRequest, TResponse>> _logger = logger;

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation error: {@ValidationErrors}", ex.Errors);
                throw;
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning("Entity not found: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception in request handler");
                throw;
            }
        }
    }
}

