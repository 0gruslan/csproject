using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.DTO;
using WebApplication1.Models.Entities;
using WebApplication1.Repositories.Interfaces;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IAuthService _authService;
    private readonly ICacheService? _cacheService;
    private readonly ApplicationDbContext _context;

    public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IAuthService authService, ApplicationDbContext context, ICacheService? cacheService = null)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _authService = authService;
        _context = context;
        _cacheService = cacheService;
    }

    public async Task<UserDto?> GetByIdAsync(int id, int currentUserId, List<string> currentUserRoles)
    {
        if (!currentUserRoles.Contains("Admin") && currentUserId != id)
        {
            if (!currentUserRoles.Contains("Manager") && !currentUserRoles.Contains("User"))
                throw new UnauthorizedAccessException("Access denied");
        }

        var cacheKey = $"user:{id}";
        if (_cacheService != null)
        {
            var cached = await _cacheService.GetAsync<UserDto>(cacheKey);
            if (cached != null) return cached;
        }

        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return null;

        var dto = MapToDto(user);
        
        if (_cacheService != null)
        {
            await _cacheService.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(5));
        }

        return dto;
    }

    public async Task<PagedResponseDto<UserDto>> GetAllAsync(int page, int pageSize, int currentUserId, List<string> currentUserRoles)
    {
        if (currentUserRoles.Count == 0)
            throw new UnauthorizedAccessException("Access denied");

        var users = await _userRepository.GetAllAsync();
        var total = users.Count;

        var pagedUsers = users
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(MapToDto)
            .ToList();

        return new PagedResponseDto<UserDto>
        {
            Items = pagedUsers,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto, int currentUserId, List<string> currentUserRoles)
    {
        if (!currentUserRoles.Contains("Admin") && !currentUserRoles.Contains("Manager"))
            throw new UnauthorizedAccessException("Only Admin and Manager can create users");

        if (await _userRepository.GetByUsernameAsync(dto.Username) != null)
            throw new InvalidOperationException("Username already exists");

        if (await _userRepository.GetByEmailAsync(dto.Email) != null)
            throw new InvalidOperationException("Email already exists");

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = AuthService.HashPassword(dto.Password),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            IsActive = true
        };

        user = await _userRepository.CreateAsync(user);

        if (dto.RoleIds.Any())
        {
            user.UserRoles = dto.RoleIds.Select(roleId => new UserRole
            {
                UserId = user.Id,
                RoleId = roleId
            }).ToList();
            await _userRepository.UpdateAsync(user);
            
            user = await _userRepository.GetByIdAsync(user.Id);
            if (user == null)
                throw new KeyNotFoundException("User not found after creation");
        }

        if (_cacheService != null)
        {
            await _cacheService.RemoveByPatternAsync("user:*");
        }

        return MapToDto(user);
    }

    public async Task<UserDto> UpdateAsync(int id, UpdateUserDto dto, int currentUserId, List<string> currentUserRoles)
    {
        if (!currentUserRoles.Contains("Admin") && !currentUserRoles.Contains("Manager"))
            throw new UnauthorizedAccessException("Only Admin and Manager can update users");

        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        if (dto.FirstName != null)
            user.FirstName = dto.FirstName;
        if (dto.LastName != null)
            user.LastName = dto.LastName;
        if (dto.Email != null)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null && existingUser.Id != id)
                throw new InvalidOperationException("Email already exists");
            user.Email = dto.Email;
        }
        if (dto.IsActive.HasValue)
            user.IsActive = dto.IsActive.Value;

        if (dto.RoleIds != null)
        {
            var existingUserRoles = await _context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .ToListAsync();
            _context.UserRoles.RemoveRange(existingUserRoles);
            
            var newUserRoles = dto.RoleIds.Select(roleId => new UserRole
            {
                UserId = user.Id,
                RoleId = roleId
            }).ToList();
            _context.UserRoles.AddRange(newUserRoles);
            await _context.SaveChangesAsync();
        }

        user = await _userRepository.UpdateAsync(user);
        
        user = await _userRepository.GetByIdAsync(user.Id);
        if (user == null)
            throw new KeyNotFoundException("User not found after update");

        if (_cacheService != null)
        {
            await _cacheService.RemoveAsync($"user:{id}");
            await _cacheService.RemoveByPatternAsync("user:*");
        }

        return MapToDto(user);
    }

    public async Task DeleteAsync(int id, int currentUserId, List<string> currentUserRoles)
    {
        if (!currentUserRoles.Contains("Admin"))
            throw new UnauthorizedAccessException("Only Admin can delete users");

        if (!await _userRepository.ExistsAsync(id))
            throw new KeyNotFoundException("User not found");

        await _userRepository.DeleteAsync(id);

        if (_cacheService != null)
        {
            await _cacheService.RemoveAsync($"user:{id}");
            await _cacheService.RemoveByPatternAsync("user:*");
        }
    }

    public async Task<bool> HasPermissionAsync(int userId, string permission)
    {
        var permissions = await _authService.GetUserPermissionsAsync(userId);
        return permissions.Contains(permission);
    }

    private UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsActive = user.IsActive,
            Roles = user.UserRoles
                .Where(ur => ur.Role != null)
                .Select(ur => ur.Role!.Name)
                .ToList(),
            CreatedAt = user.CreatedAt
        };
    }
}

