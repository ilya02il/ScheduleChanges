using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IDistributedCacheWrapper
    {
        Task SetStringAsync<Tin>(Tin obj,
            string cacheKey,
            DistributedCacheEntryOptions options = null)
            where Tin : class;

        Task<Tout> GetStringAsync<Tout>(string cacheKey)
            where Tout : class;
    }
}
