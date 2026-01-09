using WebApplication1.Models.DTO;

namespace WebApplication1.Services.Interfaces;

public interface IAccessLogService
{
    Task<AccessLogDto?> GetByIdAsync(int id, int currentUserId, List<string> currentUserRoles);
    Task<List<AccessLogDto>> GetAllAsync(int currentUserId, List<string> currentUserRoles);
    Task<AccessLogDto> CreateAsync(int accessPointId, int? userId, int? accessCardId, string result, string? reason);
}


