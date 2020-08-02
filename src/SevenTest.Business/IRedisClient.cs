using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;

namespace SevenTest.Business
{
    public interface ICacheClient
    {
        Task<T> GetCache<T>(IDistributedCache distributedCache, string cacheKey);
        Task SaveCache<T>(IDistributedCache distributedCache, string cacheKey, T data, long validUptoSeconds);
    }
}