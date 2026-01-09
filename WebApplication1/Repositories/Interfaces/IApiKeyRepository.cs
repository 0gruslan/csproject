using WebApplication1.Models.Entities;

namespace WebApplication1.Repositories.Interfaces;

public interface IApiKeyRepository
{
    Task<ApiKey?> GetByIdAsync(int id);
    Task<ApiKey?> GetByKeyValueAsync(string keyValue);
    Task<List<ApiKey>> GetAllAsync();
    Task<ApiKey> CreateAsync(ApiKey apiKey);
    Task<ApiKey> UpdateAsync(ApiKey apiKey);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task UpdateLastUsedAsync(int id);
}


