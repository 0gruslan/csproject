using WebApplication1.Models.DTO;

namespace WebApplication1.Services.Interfaces;

public interface IAuthService
{
    Task<TokenResponseDto?> LoginAsync(LoginDto loginDto);
    Task<List<string>> GetUserPermissionsAsync(int userId);
    Task<List<string>> GetUserRolesAsync(int userId);
}


