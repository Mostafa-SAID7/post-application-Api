using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Post.Application.Common.Behaviors
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CacheableAttribute : Attribute
    {
        public int DurationSeconds { get; set; } = 300; // 5 minutes default
    }

    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;

        public CachingBehavior(IMemoryCache cache, ILogger<CachingBehavior<TRequest, TResponse>> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var cacheAttribute = typeof(TRequest)
                .GetCustomAttribute<CacheableAttribute>();

            if (cacheAttribute == null)
            {
                // Not cacheable, proceed directly
                return await next();
            }

            var cacheKey = GenerateCacheKey(request);

            if (_cache.TryGetValue(cacheKey, out TResponse? cachedResponse))
            {
                _logger.LogInformation(
                    "Cache hit for {RequestName} with key {CacheKey}",
                    typeof(TRequest).Name, cacheKey);

                return cachedResponse!;
            }

            _logger.LogInformation(
                "Cache miss for {RequestName} with key {CacheKey}",
                typeof(TRequest).Name, cacheKey);

            var response = await next();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(cacheAttribute.DurationSeconds));

            _cache.Set(cacheKey, response, cacheEntryOptions);
            _logger.LogInformation(
                "Cached response for {RequestName} for {Seconds} seconds",
                typeof(TRequest).Name, cacheAttribute.DurationSeconds);

            return response;
        }

        private static string GenerateCacheKey(TRequest request)
        {
            var requestName = typeof(TRequest).Name;
            var properties = typeof(TRequest).GetProperties();

            if (properties.Length == 0)
            {
                return $"{requestName}";
            }

            var keyParts = new List<string> { requestName };
            foreach (var prop in properties)
            {
                var value = prop.GetValue(request);
                keyParts.Add($"{prop.Name}={value}");
            }

            return string.Join("_", keyParts);
        }
    }
}
