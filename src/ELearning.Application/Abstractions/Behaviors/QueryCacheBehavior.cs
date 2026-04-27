using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Cache;
using ELearning.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ELearning.Application.Abstractions.Behaviors;
internal sealed class QueryCacheBehavior<TRequest, TResponse>(
    ICacheService cacheService, 
    ILogger<QueryCacheBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery<TResponse>
    where TResponse : Result
{
    private readonly ICacheService _cacheService = cacheService;
    private readonly ILogger<QueryCacheBehavior<TRequest, TResponse>> _logger = logger;
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var isCacheMiss = false;
        var requestName = typeof(TRequest).Name;

        var result = await _cacheService.GetOrSetAsync(
            request.CacheKey,
            async token =>
            {
                isCacheMiss = true;
                var shouldCache = true;
                _logger.LogInformation("Cache miss for request {RequestName}", requestName);
                var result = await next(token);
                if (!result.IsSuccuss)
                {
                    shouldCache = false;
                }

                return (result, shouldCache);
            }, request.Expiration, cancellationToken);

        if (!isCacheMiss)
        {
            _logger.LogInformation("Cache hit for request {RequestName}", requestName);
        }

        return result;
    }
}
