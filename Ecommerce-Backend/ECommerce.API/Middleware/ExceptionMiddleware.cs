using System.Net;
using System.Text.Json;

namespace ECommerce.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Full exception (with stack trace) always goes to the server log —
                // only a trimmed, environment-aware version goes to the client.
                _logger.LogError(ex, "An unhandled exception occurred. TraceId: {TraceId}", context.TraceIdentifier);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Map known exception types to the correct HTTP status instead of
            // returning 500 for everything. Add more mappings here as your
            // Domain/Application layers introduce dedicated exception types
            // (e.g. NotFoundException, ValidationException, ConflictException).
            var statusCode = exception switch
            {
                UnauthorizedAccessException => HttpStatusCode.Forbidden,
                KeyNotFoundException => HttpStatusCode.NotFound,
                ArgumentException => HttpStatusCode.BadRequest,
                InvalidOperationException => HttpStatusCode.BadRequest, // most service-layer "business rule" throws use this
                _ => HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = (int)statusCode;

            // Only leak the real exception message in Development. In every other
            // environment, return a generic message and rely on the server log +
            // traceId for diagnosis. Previously this leaked exception.Message
            // (which can include SQL fragments / internal details) in ALL environments.
            var detail = _env.IsDevelopment()
                ? exception.Message
                : statusCode == HttpStatusCode.InternalServerError
                    ? "An unexpected error occurred. Please try again later."
                    : exception.Message; // 4xx messages here are already meant to be user-facing (e.g. "Return request must be approved before refund.")

            var response = new
            {
                status = (int)statusCode,
                title = "An error occurred while processing your request.",
                detail,
                traceId = context.TraceIdentifier
            };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}