namespace Caching.Redis
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string cacheKey, object? response, TimeSpan timeToLive);

        Task<string?> GetCachedReponseAsync(string cacheKey);
    }
}
