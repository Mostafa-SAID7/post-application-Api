using MediatR;
using Microsoft.Extensions.Logging;

namespace Post.Application.Common.Behaviors
{
    /// <summary>
    /// Logs command execution scope. Queries pass through with zero overhead.
    /// Actual DB transactions are managed per-handler via IUnitOfWork.
    /// Bug fix: was checking EndsWith("Request") — all commands end with "Command".
    /// </summary>
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

        public TransactionBehavior(ILogger<TransactionBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            if (!IsCommand(requestName))
                return await next();

            _logger.LogInformation("Executing command: {CommandName}", requestName);

            try
            {
                var response = await next();
                _logger.LogInformation("Command succeeded: {CommandName}", requestName);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Command failed: {CommandName}", requestName);
                throw;
            }
        }

        private static bool IsCommand(string requestName) =>
            requestName.EndsWith("Command", StringComparison.OrdinalIgnoreCase);
    }
}
