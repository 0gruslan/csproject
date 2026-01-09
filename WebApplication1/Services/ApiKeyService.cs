using WebApplication1.Models.DTO;
using WebApplication1.Models.Entities;
using WebApplication1.Repositories.Interfaces;
using WebApplication1.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication1.Services;

public class ApiKeyService : IApiKeyService
{
    private readonly IApiKeyRepository _apiKeyRepository;

    public ApiKeyService(IApiKeyRepository apiKeyRepository)
    {
        _apiKeyRepository = apiKeyRepository;
    }

    public async Task<List<ApiKeyDto>> GetAllAsync()
    {
        var apiKeys = await _apiKeyRepository.GetAllAsync();
        return apiKeys.Select(MapToDto).ToList();
    }

    public async Task<ApiKeyDto?> GetByIdAsync(int id)
    {
        var apiKey = await _apiKeyRepository.GetByIdAsync(id);
        return apiKey != null ? MapToDto(apiKey) : null;
    }

    public async Task<ApiKeyDto> CreateAsync(CreateApiKeyDto dto)
    {
        var keyValue = GenerateApiKey();
        
        var apiKey = new ApiKey
        {
            KeyValue = keyValue,
            Name = dto.Name,
            IsActive = true,
            ExpiresAt = dto.ExpiresAt?.ToUniversalTime(),
            CreatedAt = DateTime.UtcNow
        };

        var created = await _apiKeyRepository.CreateAsync(apiKey);
        return MapToDto(created);
    }

    public async Task<ApiKeyDto> UpdateAsync(int id, UpdateApiKeyDto dto)
    {
        var apiKey = await _apiKeyRepository.GetByIdAsync(id);
        if (apiKey == null)
            throw new KeyNotFoundException($"API Key with id {id} not found");

        if (!string.IsNullOrEmpty(dto.Name))
            apiKey.Name = dto.Name;

        if (dto.IsActive.HasValue)
            apiKey.IsActive = dto.IsActive.Value;

        if (dto.ExpiresAt.HasValue)
            apiKey.ExpiresAt = dto.ExpiresAt.Value.ToUniversalTime();

        var updated = await _apiKeyRepository.UpdateAsync(apiKey);
        return MapToDto(updated);
    }

    public async Task DeleteAsync(int id)
    {
        await _apiKeyRepository.DeleteAsync(id);
    }

    private static ApiKeyDto MapToDto(ApiKey apiKey)
    {
        return new ApiKeyDto
        {
            Id = apiKey.Id,
            KeyValue = apiKey.KeyValue,
            Name = apiKey.Name,
            IsActive = apiKey.IsActive,
            ExpiresAt = apiKey.ExpiresAt,
            CreatedAt = apiKey.CreatedAt,
            LastUsedAt = apiKey.LastUsedAt
        };
    }

    private static string GenerateApiKey()
    {
        var bytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
        }
        return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
    }
}


