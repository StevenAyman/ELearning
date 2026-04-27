using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ELearning.Application.Abstractions.Behaviors;
internal sealed class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest
    where TResponse : Result
{

    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger = logger;
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("Processing request {RequestName}", requestName);

        var result = await next(cancellationToken);

        if (result.IsSuccuss)
        {
            _logger.LogInformation("Completed request {RequestName} successfully", requestName);
        }
        else
        {
            _logger.LogInformation("Completed request {RequestName} with error {Error}", requestName, result.Error);
        }

        return result;
    }
}
