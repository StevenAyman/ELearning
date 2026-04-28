using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Cache;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Shared;
using MediatR;

namespace ELearning.Application.Abstractions.Behaviors;
internal sealed class CacheInvalidationBehavior<TRequest, TResponse>(ICacheService cacheService)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IInvalidatesCache
    where TResponse : Result
{
    private readonly ICacheService _cacheService = cacheService;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var result = await next(cancellationToken);
        if (result.IsSuccuss)
        {
            foreach(var key in request.CacheKeys)
            {
                await _cacheService.RemoveAsync(key, cancellationToken);
            }
        }

        return result;
    }
}
