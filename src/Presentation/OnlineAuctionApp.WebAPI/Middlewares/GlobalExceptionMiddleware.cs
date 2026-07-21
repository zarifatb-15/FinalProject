using Microsoft.EntityFrameworkCore;
using OnlineAuctionApp.WebAPI.Helpers;
using OnlineAuctionApp.Application.Common.Exceptions;

namespace OnlineAuctionApp.WebAPI.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
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
        catch (Exception exception)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogError(exception, "The response has already started.");
                throw;
            }

            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            BadRequestException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            ForbiddenException => StatusCodes.Status403Forbidden,
            ConflictException => StatusCodes.Status409Conflict,

            KeyNotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status403Forbidden,
            DbUpdateConcurrencyException => StatusCodes.Status409Conflict,
            DbUpdateException => StatusCodes.Status409Conflict,
            InvalidOperationException => StatusCodes.Status400BadRequest,

            _ => StatusCodes.Status500InternalServerError
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception occurred.");
        }
        else
        {
            _logger.LogWarning(exception, "Handled exception occurred.");
        }

        var message = statusCode == StatusCodes.Status500InternalServerError
            ? "An unexpected error occurred."
            : exception.Message;

        var response = statusCode switch
        {
            StatusCodes.Status400BadRequest =>
                ResponseModelHelper.CreateBadRequestResponse<string>(message),

            StatusCodes.Status403Forbidden =>
                ResponseModelHelper.CreateForbiddenResponse<string>(message),

            StatusCodes.Status404NotFound =>
                ResponseModelHelper.CreateNotFoundResponse<string>(message),

            StatusCodes.Status409Conflict =>
                ResponseModelHelper.CreateConflictResponse<string>(message),

            _ =>
                ResponseModelHelper.CreateErrorResponse<string>(
                    new List<string> { message })
        };

        context.Response.Clear();
        context.Response.StatusCode = response.StatusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(response);
    }
}