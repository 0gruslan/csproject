using WebApplication1.Models.Entities;

namespace WebApplication1.Repositories.Interfaces;

public interface IAccessLogRepository
{
    Task<AccessLog?> GetByIdAsync(int id);
    Task<List<AccessLog>> GetAllAsync();
    Task<AccessLog> CreateAsync(AccessLog accessLog);
}


