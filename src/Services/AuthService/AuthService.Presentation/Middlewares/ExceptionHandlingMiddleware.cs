using System.Text.Json;
using AuthService.Buisness.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Presentation.Middlewares;

public class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (AlreadyExistsException ex)
        {
            await context.Response.WriteAsync(GenerateErrorDetails(context, ex, StatusCodes.Status409Conflict));
        }
        catch (NotFoundException ex)
        {
            await context.Response.WriteAsync(GenerateErrorDetails(context, ex, StatusCodes.Status404NotFound));
        }
        catch (OperationFailedException ex)
        {
            await context.Response.WriteAsync(GenerateErrorDetails(context, ex, StatusCodes.Status400BadRequest));
        }
        catch (UnauthorizedException ex)
        {
            await context.Response.WriteAsync(GenerateErrorDetails(context, ex, StatusCodes.Status401Unauthorized));
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred: {ex.Message}");

            await context.Response.WriteAsync(GenerateErrorDetails(context, ex, StatusCodes.Status500InternalServerError));
        }
    }

    private string GenerateErrorDetails(HttpContext context, Exception ex, int statusCode)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json+error";

        var errorDetails = new ProblemDetails
        {
            Status = statusCode,
            Detail = ex.Message,
            Instance = context.Request.Path
        };

        return JsonSerializer.Serialize(errorDetails);
    }
}