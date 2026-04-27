using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Cache;
using Microsoft.Extensions.Caching.Hybrid;
using ZiggyCreatures.Caching.Fusion;

namespace ELearning.Infastructure.Caching;
public sealed class CacheService(IFusionCache cache) : ICacheService
{
    private readonly IFusionCache _cache = cache;
    public async Task<T> GetOrSetAsync<T>(string cacheKey, Func<CancellationToken, ValueTask<(T, bool)>> factory, TimeSpan? expiration, CancellationToken cancellationToken = default)
    {
        var result = await _cache.GetOrSetAsync<T>(
            cacheKey,
            async (ctx, ct) =>
            {
                var (value, shouldCache) = await factory(ct);
                if (!shouldCache)
                {
                    ctx.Options.Duration = TimeSpan.FromSeconds(0);
                }
                return value;
            }, expiration is null ? null : new FusionCacheEntryOptions(expiration),
            cancellationToken);
        return result;
    }

    public async Task RemoveAsync(string cacheKey, CancellationToken cancellationToken = default)
    {
        await _cache.RemoveAsync(cacheKey, null, cancellationToken);
    }
}
