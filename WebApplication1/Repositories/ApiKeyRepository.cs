using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Entities;
using WebApplication1.Repositories.Interfaces;

namespace WebApplication1.Repositories;

public class ApiKeyRepository : IApiKeyRepository
{
    private readonly ApplicationDbContext _context;

    public ApiKeyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiKey?> GetByIdAsync(int id)
    {
        return await _context.ApiKeys.FindAsync(id);
    }

    public async Task<ApiKey?> GetByKeyValueAsync(string keyValue)
    {
        return await _context.ApiKeys
            .FirstOrDefaultAsync(ak => ak.KeyValue == keyValue && ak.IsActive);
    }

    public async Task<List<ApiKey>> GetAllAsync()
    {
        return await _context.ApiKeys
            .OrderByDescending(ak => ak.CreatedAt)
            .ToListAsync();
    }

    public async Task<ApiKey> CreateAsync(ApiKey apiKey)
    {
        apiKey.CreatedAt = DateTime.UtcNow;
        _context.ApiKeys.Add(apiKey);
        await _context.SaveChangesAsync();
        return apiKey;
    }

    public async Task<ApiKey> UpdateAsync(ApiKey apiKey)
    {
        if (apiKey.CreatedAt.Kind != DateTimeKind.Utc)
            apiKey.CreatedAt = apiKey.CreatedAt.ToUniversalTime();
        
        _context.ApiKeys.Update(apiKey);
        await _context.SaveChangesAsync();
        return apiKey;
    }

    public async Task DeleteAsync(int id)
    {
        var apiKey = await _context.ApiKeys.FindAsync(id);
        if (apiKey != null)
        {
            _context.ApiKeys.Remove(apiKey);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.ApiKeys.AnyAsync(ak => ak.Id == id);
    }

    public async Task UpdateLastUsedAsync(int id)
    {
        var apiKey = await _context.ApiKeys.FindAsync(id);
        if (apiKey != null)
        {
            apiKey.LastUsedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}


