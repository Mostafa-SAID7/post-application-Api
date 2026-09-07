using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Post.Application.Common.Exceptions;

namespace Post.Api.Filters
{
    /// <summary>
    /// Global exception filter for handling application exceptions
    /// </summary>
    public class ExceptionFilter(ILogger<ExceptionFilter> logger) : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger = logger;

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var correlationId = context.HttpContext.TraceIdentifier;

            _logger.LogError(exception, "An unhandled exception occurred. CorrelationId: {CorrelationId}", correlationId);

            var response = new ProblemDetails
            {
                Type = "https://api.example.com/errors/internal",
                Title = "An unexpected error occurred",
                Status = StatusCodes.Status500InternalServerError,
                Detail = exception.Message,
                Instance = context.HttpContext.Request.Path
            };

            // Handle specific exception types
            if (exception is EntityNotFoundException)
            {
                response.Status = StatusCodes.Status404NotFound;
                response.Type = "https://api.example.com/errors/not-found";
                response.Title = "Resource not found";
            }
            else if (exception is ValidationException validationException)
            {
                response.Status = StatusCodes.Status400BadRequest;
                response.Type = "https://api.example.com/errors/validation";
                response.Title = "Validation failed";
                response.Detail = validationException.Message;
            }

            context.Result = new ObjectResult(response)
            {
                StatusCode = response.Status
            };

            context.ExceptionHandled = true;
        }
    }
}
