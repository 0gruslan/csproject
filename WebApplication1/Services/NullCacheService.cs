using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services;

// Заглушка для кэша, когда Redis недоступен
public class NullCacheService : ICacheService
{
    public Task<T?> GetAsync<T>(string key) where T : class
    {
        return Task.FromResult<T?>(null);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null) where T : class
    {
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        return Task.CompletedTask;
    }

    public Task RemoveByPatternAsync(string pattern)
    {
        return Task.CompletedTask;
    }
}


