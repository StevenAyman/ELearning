using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Application.Abstractions.Cache;
public interface ICacheService
{
    Task<T> GetOrSetAsync<T>(string cacheKey, Func<CancellationToken, ValueTask<(T, bool)>> factory, TimeSpan? expiration, CancellationToken cancellationToken = default);
    Task RemoveAsync(string cacheKey, CancellationToken cancellationToken = default);

}
