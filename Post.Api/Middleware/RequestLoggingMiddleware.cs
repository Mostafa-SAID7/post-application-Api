namespace Post.Api.Middleware
{
    /// <summary>
    /// Logs incoming requests and their completed status codes.
    /// The MemoryStream body-swap pattern was removed — it intercepted
    /// SendFileAsync responses (static files, minimal API routes) and produced
    /// Content-Length: 0 because SendFileAsync writes directly to the Kestrel socket,
    /// bypassing any substituted stream. Status code is available on HttpContext
    /// without capturing the body.
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

            _logger.LogInformation(
                "Incoming request - CorrelationId: {CorrelationId}, Method: {Method}, Path: {Path}",
                correlationId, context.Request.Method, context.Request.Path);

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
        }
    }
}
