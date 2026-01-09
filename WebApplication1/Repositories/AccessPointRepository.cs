using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Entities;
using WebApplication1.Repositories.Interfaces;

namespace WebApplication1.Repositories;

public class AccessPointRepository : IAccessPointRepository
{
    private readonly ApplicationDbContext _context;

    public AccessPointRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AccessPoint?> GetByIdAsync(int id)
    {
        return await _context.AccessPoints.FindAsync(id);
    }

    public async Task<List<AccessPoint>> GetAllAsync(string? search = null, int page = 1, int pageSize = 10)
    {
        var query = _context.AccessPoints.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(ap => 
                ap.Name.Contains(search) || 
                (ap.Location != null && ap.Location.Contains(search)));
        }

        return await query
            .OrderBy(ap => ap.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync(string? search = null)
    {
        var query = _context.AccessPoints.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(ap => 
                ap.Name.Contains(search) || 
                (ap.Location != null && ap.Location.Contains(search)));
        }

        return await query.CountAsync();
    }

    public async Task<AccessPoint> CreateAsync(AccessPoint accessPoint)
    {
        accessPoint.CreatedAt = DateTime.UtcNow;
        accessPoint.UpdatedAt = DateTime.UtcNow;
        _context.AccessPoints.Add(accessPoint);
        await _context.SaveChangesAsync();
        return accessPoint;
    }

    public async Task<AccessPoint> UpdateAsync(AccessPoint accessPoint)
    {
        // Убеждаемся, что все DateTime в UTC
        accessPoint.UpdatedAt = DateTime.UtcNow;
        if (accessPoint.CreatedAt.Kind != DateTimeKind.Utc)
            accessPoint.CreatedAt = accessPoint.CreatedAt.ToUniversalTime();
        
        _context.AccessPoints.Update(accessPoint);
        await _context.SaveChangesAsync();
        return accessPoint;
    }

    public async Task DeleteAsync(int id)
    {
        var accessPoint = await _context.AccessPoints.FindAsync(id);
        if (accessPoint != null)
        {
            _context.AccessPoints.Remove(accessPoint);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.AccessPoints.AnyAsync(ap => ap.Id == id);
    }
}


