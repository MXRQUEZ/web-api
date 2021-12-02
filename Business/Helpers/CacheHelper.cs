using System;
using Business.DTO;
using DAL.Models.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Business.Helpers
{
    public sealed class CacheHelper<T>
    {
        private readonly IMemoryCache _cache;
        public CacheHelper(IMemoryCache cache)
        {
            _cache = cache;
        }

        public string GetCacheKey(string key) => $"{typeof(T)}_{key}";

        public void SetCache(string cacheKey, T cacheItem)
        {
            _cache.Set(cacheKey, cacheItem,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
        }

        public T GetCachedData(string cacheKey)
        {
            _cache.TryGetValue(cacheKey, out T cacheItem);
            return cacheItem;
        }

        public void RemoveCache(string cacheKey)
        {
            _cache.Remove(cacheKey);
        }
    }
}
