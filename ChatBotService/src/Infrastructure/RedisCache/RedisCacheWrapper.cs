using Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.RedisCache
{
    public class RedisCacheWrapper : IDistributedCacheWrapper
    {
        private readonly IDistributedCache _distributedCache;

        public RedisCacheWrapper(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task SetStringAsync<Tin>(Tin obj,
            string cacheKey,
            DistributedCacheEntryOptions options = null,
            CancellationToken cancellationToken = default)

            where Tin : class
        {
            var serializedObj = JsonConvert.SerializeObject(obj);

            if (options is null)
            {
                await _distributedCache.SetStringAsync(cacheKey, serializedObj, cancellationToken);
                return;
            }

            await _distributedCache.SetStringAsync(cacheKey, serializedObj, options, cancellationToken);
        }

        public async Task<Tout> GetStringAsync<Tout>(string cacheKey,
            CancellationToken cancellationToken = default)
            where Tout : class
        {
            var serializedObj = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);

            if (serializedObj is null)
                return null;//here must be logging

            return JsonConvert.DeserializeObject<Tout>(serializedObj);
        }
    }
}
