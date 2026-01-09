using WebApplication1.Models.DTO;
using WebApplication1.Models.Entities;
using WebApplication1.Repositories.Interfaces;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services;

public class AccessLogService : IAccessLogService
{
    private readonly IAccessLogRepository _accessLogRepository;

    public AccessLogService(IAccessLogRepository accessLogRepository)
    {
        _accessLogRepository = accessLogRepository;
    }

    public async Task<AccessLogDto?> GetByIdAsync(int id, int currentUserId, List<string> currentUserRoles)
    {
        var accessLog = await _accessLogRepository.GetByIdAsync(id);
        if (accessLog == null)
            return null;

        return MapToDto(accessLog);
    }

    public async Task<List<AccessLogDto>> GetAllAsync(int currentUserId, List<string> currentUserRoles)
    {
        var accessLogs = await _accessLogRepository.GetAllAsync();
        return accessLogs.Select(MapToDto).ToList();
    }

    public async Task<AccessLogDto> CreateAsync(int accessPointId, int? userId, int? accessCardId, string result, string? reason)
    {
        var accessLog = new AccessLog
        {
            AccessPointId = accessPointId,
            UserId = userId,
            AccessCardId = accessCardId,
            AccessResult = result,
            Reason = reason
        };

        accessLog = await _accessLogRepository.CreateAsync(accessLog);
        return MapToDto(await _accessLogRepository.GetByIdAsync(accessLog.Id)!);
    }

    private AccessLogDto MapToDto(AccessLog accessLog)
    {
        return new AccessLogDto
        {
            Id = accessLog.Id,
            UserId = accessLog.UserId,
            Username = accessLog.User?.Username,
            AccessCardId = accessLog.AccessCardId,
            CardNumber = accessLog.AccessCard?.CardNumber,
            AccessPointId = accessLog.AccessPointId,
            AccessPointName = accessLog.AccessPoint.Name,
            AccessResult = accessLog.AccessResult,
            AccessTime = accessLog.AccessTime,
            Reason = accessLog.Reason
        };
    }
}


