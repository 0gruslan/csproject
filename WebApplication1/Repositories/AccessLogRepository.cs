using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Entities;
using WebApplication1.Repositories.Interfaces;

namespace WebApplication1.Repositories;

public class AccessLogRepository : IAccessLogRepository
{
    private readonly ApplicationDbContext _context;

    public AccessLogRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AccessLog?> GetByIdAsync(int id)
    {
        return await _context.AccessLogs
            .Include(al => al.User)
            .Include(al => al.AccessCard)
            .Include(al => al.AccessPoint)
            .FirstOrDefaultAsync(al => al.Id == id);
    }

    public async Task<List<AccessLog>> GetAllAsync()
    {
        return await _context.AccessLogs
            .Include(al => al.User)
            .Include(al => al.AccessCard)
            .Include(al => al.AccessPoint)
            .OrderByDescending(al => al.AccessTime)
            .ToListAsync();
    }

    public async Task<AccessLog> CreateAsync(AccessLog accessLog)
    {
        accessLog.AccessTime = DateTime.UtcNow;
        _context.AccessLogs.Add(accessLog);
        await _context.SaveChangesAsync();
        return accessLog;
    }
}


