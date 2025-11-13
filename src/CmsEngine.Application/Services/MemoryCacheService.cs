namespace CmsEngine.Application.Services;

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly bool _sizeLimitEnabled;

    public MemoryCacheService(IMemoryCache memoryCache, MemoryCacheOptions options)
    {
        _memoryCache = memoryCache;
        _sizeLimitEnabled = options.SizeLimit.HasValue;
    }

    public T? Get<T>(string key)
    {
        return _memoryCache.TryGetValue(key, out T? value) ? value : default;
    }

    public void Set<T>(string key, T value, TimeSpan? expiration = null)
    {
        var options = new MemoryCacheEntryOptions();

        if (expiration.HasValue)
        {
            options.SetSlidingExpiration(expiration.Value);
        }

        if (_sizeLimitEnabled)
        {
            options.Size = 1;
        }

        _memoryCache.Set(key, value, options);
    }

    public bool TryGet<T>(string key, out T? value)
    {
        return _memoryCache.TryGetValue(key, out value);
    }

    public void Remove(string key)
    {
        _memoryCache.Remove(key);
    }
}
