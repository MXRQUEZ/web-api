namespace DAL.Interfaces
{
    public interface ICacheManager<T> where T: class
    {
        string GetCacheKey(string key);
        void SetCache(string cacheKey, T cacheItem);
        void RemoveCache(string cacheKey);
        T GetCachedData(string cacheKey);
    }
}