using WebApplication1.Models.Entities;

namespace WebApplication1.Repositories.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(int id);
    Task<Role?> GetByNameAsync(string name);
    Task<List<Role>> GetAllAsync();
    Task<List<Permission>> GetPermissionsByRoleIdAsync(int roleId);
}


