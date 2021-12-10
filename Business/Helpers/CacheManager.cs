using System;
using Business.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Business.Helpers
{
    public sealed class CacheManager<T> : ICacheManager<T> where T : class
    {
        private readonly IMemoryCache _cache;
        public CacheManager(IMemoryCache cache) => _cache = cache;

        public string GetCacheKey(string key) => $"{typeof(T)}_{key}";

        public void SetCache(string cacheKey, T cacheItem) => 
            _cache.Set(cacheKey, cacheItem,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

        public void RemoveCache(string cacheKey) => _cache.Remove(cacheKey);

        public T GetCachedData(string cacheKey)
        {
            _cache.TryGetValue(cacheKey, out T cacheItem);
            return cacheItem;
        }
    }
}
