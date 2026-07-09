using MediatR;
using Microsoft.Extensions.Logging;
using Post.Application.Common.Interfaces;

namespace Post.Application.Common.Behaviors
{
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

            // Only apply transactions to commands (not queries)
            // Queries don't modify data, so they don't need transactions
            if (!IsCommand(requestName))
            {
                return await next();
            }

            _logger.LogInformation("Starting transaction for command: {CommandName}", requestName);

            try
            {
                var response = await next();
                _logger.LogInformation("Transaction committed for command: {CommandName}", requestName);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transaction rolled back for command: {CommandName}", requestName);
                throw;
            }
        }

        private static bool IsCommand(string requestName)
        {
            return requestName.EndsWith("Request") && 
                   (requestName.Contains("Create") || requestName.Contains("Update") || requestName.Contains("Delete"));
        }
    }
}
