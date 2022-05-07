using Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
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
            DistributedCacheEntryOptions options = null)

            where Tin : class
        {
            var serializedObj = JsonConvert.SerializeObject(obj);

            if (options is null)
            {
                await _distributedCache.SetStringAsync(cacheKey, serializedObj);
                return;
            }

            await _distributedCache.SetStringAsync(cacheKey, serializedObj, options);
        }

        public async Task<Tout> GetStringAsync<Tout>(string cacheKey)
            where Tout : class
        {
            var serializedObj = await _distributedCache.GetStringAsync(cacheKey);

            if (serializedObj is null)
                return null;//here must be logging

            return JsonConvert.DeserializeObject<Tout>(serializedObj);
        }
    }
}
