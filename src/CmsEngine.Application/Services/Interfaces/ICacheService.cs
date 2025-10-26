namespace CmsEngine.Application.Services.Interfaces;

public interface ICacheService
{
    T? Get<T>(string key);

    void Set<T>(string key, T value, TimeSpan? expiration = null);

    bool TryGet<T>(string key, out T? value);

    void Remove(string key);
}
