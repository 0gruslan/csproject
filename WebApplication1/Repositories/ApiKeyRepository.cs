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

    public async Task<ApiKey?> GetByKeyValueAsync(string keyValue)
    {
        return await _context.ApiKeys
            .FirstOrDefaultAsync(ak => ak.KeyValue == keyValue && ak.IsActive);
    }
}


