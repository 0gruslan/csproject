using WebApplication1.Models.DTO;
using WebApplication1.Models.Entities;
using WebApplication1.Repositories.Interfaces;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services;

public class AccessPointService : IAccessPointService
{
    private readonly IAccessPointRepository _accessPointRepository;
    private readonly ICacheService? _cacheService;

    public AccessPointService(IAccessPointRepository accessPointRepository, ICacheService? cacheService = null)
    {
        _accessPointRepository = accessPointRepository;
        _cacheService = cacheService;
    }

    public async Task<AccessPointDto?> GetByIdAsync(int id, int currentUserId, List<string> currentUserRoles)
    {
        // Простая проверка - любой авторизованный может читать
        if (currentUserRoles.Count == 0)
            throw new UnauthorizedAccessException("Access denied");

        var cacheKey = $"accesspoint:{id}";
        if (_cacheService != null)
        {
            var cached = await _cacheService.GetAsync<AccessPointDto>(cacheKey);
            if (cached != null)
                return cached;
        }

        var accessPoint = await _accessPointRepository.GetByIdAsync(id);
        if (accessPoint == null)
            return null;

        var dto = MapToDto(accessPoint);
        
        if (_cacheService != null)
        {
            await _cacheService.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(5));
        }

        return dto;
    }

    public async Task<PagedResponseDto<AccessPointDto>> GetAllAsync(int page, int pageSize, string? search, int currentUserId, List<string> currentUserRoles)
    {
        // Простая проверка - любой авторизованный может читать
        if (currentUserRoles.Count == 0)
            throw new UnauthorizedAccessException("Access denied");

        var accessPoints = await _accessPointRepository.GetAllAsync(search, page, pageSize);
        var total = await _accessPointRepository.GetTotalCountAsync(search);

        var dtos = accessPoints.Select(MapToDto).ToList();

        return new PagedResponseDto<AccessPointDto>
        {
            Items = dtos,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<AccessPointDto> CreateAsync(CreateAccessPointDto dto, int currentUserId, List<string> currentUserRoles)
    {
        // Простая проверка - только Admin и Manager могут создавать
        if (!currentUserRoles.Contains("Admin") && !currentUserRoles.Contains("Manager"))
            throw new UnauthorizedAccessException("Only Admin and Manager can create access points");

        var accessPoint = new AccessPoint
        {
            Name = dto.Name,
            Location = dto.Location,
            Description = dto.Description,
            IsActive = true
        };

        accessPoint = await _accessPointRepository.CreateAsync(accessPoint);

        if (_cacheService != null)
        {
            await _cacheService.RemoveByPatternAsync("accesspoint:*");
        }

        return MapToDto(accessPoint);
    }

    public async Task<AccessPointDto> UpdateAsync(int id, UpdateAccessPointDto dto, int currentUserId, List<string> currentUserRoles)
    {
        // Простая проверка - только Admin и Manager могут обновлять
        if (!currentUserRoles.Contains("Admin") && !currentUserRoles.Contains("Manager"))
            throw new UnauthorizedAccessException("Only Admin and Manager can update access points");

        var accessPoint = await _accessPointRepository.GetByIdAsync(id);
        if (accessPoint == null)
            throw new KeyNotFoundException("Access point not found");

        if (dto.Name != null)
            accessPoint.Name = dto.Name;
        if (dto.Location != null)
            accessPoint.Location = dto.Location;
        if (dto.Description != null)
            accessPoint.Description = dto.Description;
        if (dto.IsActive.HasValue)
            accessPoint.IsActive = dto.IsActive.Value;

        accessPoint = await _accessPointRepository.UpdateAsync(accessPoint);

        if (_cacheService != null)
        {
            await _cacheService.RemoveAsync($"accesspoint:{id}");
            await _cacheService.RemoveByPatternAsync("accesspoint:*");
        }

        return MapToDto(accessPoint);
    }

    public async Task DeleteAsync(int id, int currentUserId, List<string> currentUserRoles)
    {
        // Простая проверка - только Admin может удалять
        if (!currentUserRoles.Contains("Admin"))
            throw new UnauthorizedAccessException("Only Admin can delete access points");

        if (!await _accessPointRepository.ExistsAsync(id))
            throw new KeyNotFoundException("Access point not found");

        await _accessPointRepository.DeleteAsync(id);

        if (_cacheService != null)
        {
            await _cacheService.RemoveAsync($"accesspoint:{id}");
            await _cacheService.RemoveByPatternAsync("accesspoint:*");
        }
    }


    private AccessPointDto MapToDto(AccessPoint accessPoint)
    {
        return new AccessPointDto
        {
            Id = accessPoint.Id,
            Name = accessPoint.Name,
            Location = accessPoint.Location,
            Description = accessPoint.Description,
            IsActive = accessPoint.IsActive,
            CreatedAt = accessPoint.CreatedAt
        };
    }
}

