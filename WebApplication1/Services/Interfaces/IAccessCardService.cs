using WebApplication1.Models.DTO;

namespace WebApplication1.Services.Interfaces;

public interface IAccessCardService
{
    Task<AccessCardDto?> GetByIdAsync(int id, int currentUserId, List<string> currentUserRoles);
    Task<List<AccessCardDto>> GetAllAsync(int currentUserId, List<string> currentUserRoles);
    Task<AccessCardDto> CreateAsync(CreateAccessCardDto dto, int currentUserId, List<string> currentUserRoles);
    Task<AccessCardDto> UpdateAsync(int id, UpdateAccessCardDto dto, int currentUserId, List<string> currentUserRoles);
    Task DeleteAsync(int id, int currentUserId, List<string> currentUserRoles);
}


