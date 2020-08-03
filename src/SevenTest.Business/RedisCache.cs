using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SevenTest.Business
{
    public class AsyncJsonCacheHelper
    {
        public static async Task<T> GetCache<T>(IDistributedCache distributedCache, string cacheKey)
        {
            var cachedData = await distributedCache.GetStringAsync(cacheKey);
            if (cachedData != null)
            {
                var cachedJson = JsonConvert.DeserializeObject<T>(cachedData);
                return cachedJson;
            }
            else
            {
                return default(T);
            }
        }

        public static async Task SaveCache<T>(IDistributedCache distributedCache, string cacheKey, long expireInSeconds, T data)
        {
            var jsonData = JsonConvert.SerializeObject(data);           
            if(expireInSeconds != 0)
            await distributedCache.SetStringAsync(cacheKey, jsonData,
                new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expireInSeconds) });
        }
    }
}
