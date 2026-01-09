using WebApplication1.Models.DTO;

namespace WebApplication1.Services.Interfaces;

public interface IAccessPointService
{
    Task<AccessPointDto?> GetByIdAsync(int id, int currentUserId, List<string> currentUserRoles);
    Task<PagedResponseDto<AccessPointDto>> GetAllAsync(int page, int pageSize, string? search, int currentUserId, List<string> currentUserRoles);
    Task<AccessPointDto> CreateAsync(CreateAccessPointDto dto, int currentUserId, List<string> currentUserRoles);
    Task<AccessPointDto> UpdateAsync(int id, UpdateAccessPointDto dto, int currentUserId, List<string> currentUserRoles);
    Task DeleteAsync(int id, int currentUserId, List<string> currentUserRoles);
}


