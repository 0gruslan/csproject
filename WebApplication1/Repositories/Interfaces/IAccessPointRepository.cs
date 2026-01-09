using WebApplication1.Models.Entities;

namespace WebApplication1.Repositories.Interfaces;

public interface IAccessPointRepository
{
    Task<AccessPoint?> GetByIdAsync(int id);
    Task<List<AccessPoint>> GetAllAsync(string? search = null, int page = 1, int pageSize = 10);
    Task<int> GetTotalCountAsync(string? search = null);
    Task<AccessPoint> CreateAsync(AccessPoint accessPoint);
    Task<AccessPoint> UpdateAsync(AccessPoint accessPoint);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}


