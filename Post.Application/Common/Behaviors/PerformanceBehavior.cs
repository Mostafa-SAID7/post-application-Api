using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Post.Application.Common.Behaviors
{
    public class PerformanceBehavior<TRequest, TResponse>(ILogger<PerformanceBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger = logger;
        private const int SlowRequestThresholdMs = 500;

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();
            var response = await next();
            stopwatch.Stop();

            if (stopwatch.ElapsedMilliseconds > SlowRequestThresholdMs)
            {
                var requestName = typeof(TRequest).Name;
                _logger.LogWarning(
                    "Slow request detected: {RequestName} took {ElapsedMilliseconds}ms (threshold: {Threshold}ms)",
                    requestName, stopwatch.ElapsedMilliseconds, SlowRequestThresholdMs);
            }

            return response;
        }
    }
}
