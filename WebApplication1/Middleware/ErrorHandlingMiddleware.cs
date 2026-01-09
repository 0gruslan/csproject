using System.Net;
using System.Text.Json;
using WebApplication1.Models.DTO;

namespace WebApplication1.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var error = "InternalServerError";
        var message = "An error occurred while processing your request.";

        switch (exception)
        {
            case KeyNotFoundException:
                code = HttpStatusCode.NotFound;
                error = "NotFound";
                message = exception.Message;
                break;
            case UnauthorizedAccessException:
                code = HttpStatusCode.Forbidden;
                error = "Forbidden";
                message = exception.Message;
                break;
            case InvalidOperationException:
                code = HttpStatusCode.BadRequest;
                error = "BadRequest";
                message = exception.Message;
                break;
            case ArgumentException:
                code = HttpStatusCode.BadRequest;
                error = "BadRequest";
                message = exception.Message;
                break;
        }

        var response = new ErrorResponseDto
        {
            Error = error,
            Message = message,
            TraceId = context.TraceIdentifier
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        var json = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(json);
    }
}


