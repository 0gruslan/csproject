using WebApplication1.Models.Entities;

namespace WebApplication1.Repositories.Interfaces;

public interface IApiKeyRepository
{
    Task<ApiKey?> GetByKeyValueAsync(string keyValue);
}


