using System.Data.Common;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using System.Diagnostics;
using Ecommerce.Application.Abstractions.Infrastructure;
using Ecommerce.API.Common.Exceptions;

namespace Admin.API.Handlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
        };

        var _currentUserService = httpContext.RequestServices.GetRequiredService<ICurrentUserService>();

        if (exception is AppException)
        {
            var appException = exception as AppException;
            problemDetails.Title = appException!.Error.ErrorKey;
            problemDetails.Detail = appException.Error.Message;
            problemDetails.Status = (int)appException.Error.StatusCode;

            // Log as a warning without the exception object
            _logger.LogWarning(
                "Application Error in Custom Middleware: {ErrorKey}, {Message}, {StatusCode}, {ExceptionType}",
                appException.Error.ErrorKey,
                appException.Error.Message,
                appException.Error.StatusCode,
                appException.GetType().Name

                );

            httpContext.Response.StatusCode = (int)appException.Error.StatusCode;
        }        
        else if (exception is ValidationException)
        {
            var validationException = exception as ValidationException;
            problemDetails.Status = (int)HttpStatusCode.BadRequest;
            problemDetails.Title = "validation_failed";
            problemDetails.Detail = validationException!.Message;

            if (validationException.Errors.Any())
            {
                List<KeyValuePair<string, string>> errors = new();
                foreach (var error in validationException.Errors)
                {
                    errors.Add(new KeyValuePair<string, string>(error.PropertyName, error.ErrorMessage));
                }
                problemDetails.Extensions.Add("validation_errors", errors);
            }

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            // Log as warning without the exception object
            _logger.LogWarning(validationException, "Validation exception ocurred");
        }

        else if (exception is BadHttpRequestException)
        {
            problemDetails.Title = "bad_request";
            problemDetails.Detail = exception.Message;
            problemDetails.Status = (int)HttpStatusCode.BadRequest;
            _logger.LogError(exception, "A bad request was received");
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        }
        else if (exception is UnauthorizedAccessException)
        {
            problemDetails.Title = "unauthorized_access";
            problemDetails.Detail = exception.Message;
            problemDetails.Status = (int)HttpStatusCode.Unauthorized;
            _logger.LogError(exception, "Unauthorized exception occurred");
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

        }
        else if (exception is DbUpdateException)
        {
            if (exception.InnerException is DbException dbException)
            {
                problemDetails.Title = "unique_constraint_violation";
                problemDetails.Detail = "A conflict occured.";
                problemDetails.Status = (int)HttpStatusCode.Conflict;
                _logger.LogError(exception, "Db exception occurred");

                httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
            }
            else
            {
                problemDetails.Title = "dbupdate_failed";
                problemDetails.Detail = "Db update failed.";
                problemDetails.Status = (int)HttpStatusCode.ExpectationFailed;

                httpContext.Response.StatusCode = StatusCodes.Status417ExpectationFailed;

                _logger.LogError(exception, "DbUpdate exception occurred");
            }
        }
        else
        {
            problemDetails.Title = "something_went_wrong";
            problemDetails.Detail = "Something went wrong";
            problemDetails.Status = (int)HttpStatusCode.InternalServerError;

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            _logger.LogError(exception, "An unhandled exception occurred");
        }

        // Add a trace ID
        string? requestId = httpContext.Request.Headers["RequestId"].FirstOrDefault();
        if (!string.IsNullOrEmpty(requestId))
        {
            problemDetails.Extensions.TryAdd("RequestId", requestId);
        }
        problemDetails.Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}";

        var activity_id = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
        if (!string.IsNullOrEmpty(activity_id))
        {
            problemDetails.Extensions.Add("activity_id", activity_id);
        }

        // Add exception details only in development or local environment
        if (httpContext.RequestServices.GetRequiredService<IHostEnvironment>().IsDevelopment() ||
            string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "Local", StringComparison.OrdinalIgnoreCase))
        {
            problemDetails.Extensions.Add("exception_message", exception.Message);
            problemDetails.Extensions.Add("inner_exception", exception.InnerException?.Message);
            problemDetails.Extensions.Add("stack_trace", exception.StackTrace);

        }

        await httpContext.Response.WriteAsJsonAsync(problemDetails);

        return true;
    }
}
