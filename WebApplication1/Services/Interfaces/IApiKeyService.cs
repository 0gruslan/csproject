using WebApplication1.Models.DTO;

namespace WebApplication1.Services.Interfaces;

public interface IApiKeyService
{
    Task<List<ApiKeyDto>> GetAllAsync();
    Task<ApiKeyDto?> GetByIdAsync(int id);
    Task<ApiKeyDto> CreateAsync(CreateApiKeyDto dto);
    Task<ApiKeyDto> UpdateAsync(int id, UpdateApiKeyDto dto);
    Task DeleteAsync(int id);
}

