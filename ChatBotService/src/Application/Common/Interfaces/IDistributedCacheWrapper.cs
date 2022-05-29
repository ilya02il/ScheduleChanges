using Microsoft.Extensions.Caching.Distributed;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IDistributedCacheWrapper
    {
        Task SetStringAsync<Tin>(Tin obj,
            string cacheKey,
            DistributedCacheEntryOptions options = null,
            CancellationToken cancellationToken = default)
            where Tin : class;

        Task<Tout> GetStringAsync<Tout>(string cacheKey,
            CancellationToken cancellationToken = default)
            where Tout : class;
    }
}
