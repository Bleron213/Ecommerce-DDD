using Ecommerce.Application.Abstractions.Infrastructure;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Admin.Application.Behaviors;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly ICurrentUserService _user;

    public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService user)
    {
        _logger = logger;
        _user = user;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _user.UserGuid.ToString() ?? string.Empty;

        _logger.LogInformation("Admin.API MediatR Request: {Name} {@UserId} {@Request}",
            requestName, userId, request);
    }
}
