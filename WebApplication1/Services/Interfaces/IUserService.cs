using WebApplication1.Models.DTO;

namespace WebApplication1.Services.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetByIdAsync(int id, int currentUserId, List<string> currentUserRoles);
    Task<PagedResponseDto<UserDto>> GetAllAsync(int page, int pageSize, int currentUserId, List<string> currentUserRoles);
    Task<UserDto> CreateAsync(CreateUserDto dto, int currentUserId, List<string> currentUserRoles);
    Task<UserDto> UpdateAsync(int id, UpdateUserDto dto, int currentUserId, List<string> currentUserRoles);
    Task DeleteAsync(int id, int currentUserId, List<string> currentUserRoles);
    Task<bool> HasPermissionAsync(int userId, string permission);
}


