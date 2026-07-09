namespace Post.Api.Middleware
{
    /// <summary>
    /// Middleware for logging HTTP requests and responses
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.TraceIdentifier;
            var method = context.Request.Method;
            var path = context.Request.Path;

            _logger.LogInformation(
                "Incoming request - CorrelationId: {CorrelationId}, Method: {Method}, Path: {Path}",
                correlationId, method, path);

            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                try
                {
                    await _next(context);

                    _logger.LogInformation(
                        "Request completed - CorrelationId: {CorrelationId}, StatusCode: {StatusCode}",
                        correlationId, context.Response.StatusCode);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Request failed - CorrelationId: {CorrelationId}, Exception: {Message}",
                        correlationId, ex.Message);
                    throw;
                }
                finally
                {
                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
        }
    }
}
