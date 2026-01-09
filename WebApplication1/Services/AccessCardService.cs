using WebApplication1.Models.DTO;
using WebApplication1.Models.Entities;
using WebApplication1.Repositories.Interfaces;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services;

public class AccessCardService : IAccessCardService
{
    private readonly IAccessCardRepository _accessCardRepository;
    private readonly IAccessPointRepository _accessPointRepository;

    public AccessCardService(IAccessCardRepository accessCardRepository, IAccessPointRepository accessPointRepository)
    {
        _accessCardRepository = accessCardRepository;
        _accessPointRepository = accessPointRepository;
    }

    public async Task<AccessCardDto?> GetByIdAsync(int id, int currentUserId, List<string> currentUserRoles)
    {
        var accessCard = await _accessCardRepository.GetByIdAsync(id);
        if (accessCard == null)
            return null;

        return MapToDto(accessCard);
    }

    public async Task<List<AccessCardDto>> GetAllAsync(int currentUserId, List<string> currentUserRoles)
    {
        var accessCards = await _accessCardRepository.GetAllAsync();
        return accessCards.Select(MapToDto).ToList();
    }

    public async Task<AccessCardDto> CreateAsync(CreateAccessCardDto dto, int currentUserId, List<string> currentUserRoles)
    {
        if (!currentUserRoles.Contains("Admin") && !currentUserRoles.Contains("Manager"))
            throw new UnauthorizedAccessException("Only Admin and Manager can create access cards");

        if (await _accessCardRepository.GetByCardNumberAsync(dto.CardNumber) != null)
            throw new InvalidOperationException("Card number already exists");

        foreach (var accessPointId in dto.AccessPointIds)
        {
            if (!await _accessPointRepository.ExistsAsync(accessPointId))
                throw new KeyNotFoundException($"Access point with id {accessPointId} not found");
        }

        var accessCard = new AccessCard
        {
            CardNumber = dto.CardNumber,
            CardType = dto.CardType,
            IsActive = true,
            ExpiresAt = dto.ExpiresAt
        };

        accessCard = await _accessCardRepository.CreateAsync(accessCard);

        if (dto.AccessPointIds.Any())
        {
            await _accessCardRepository.AssignAccessPointsAsync(accessCard.Id, dto.AccessPointIds);
        }

        return MapToDto(await _accessCardRepository.GetByIdAsync(accessCard.Id)!);
    }

    public async Task<AccessCardDto> UpdateAsync(int id, UpdateAccessCardDto dto, int currentUserId, List<string> currentUserRoles)
    {
        if (!currentUserRoles.Contains("Admin") && !currentUserRoles.Contains("Manager"))
            throw new UnauthorizedAccessException("Only Admin and Manager can update access cards");

        var accessCard = await _accessCardRepository.GetByIdAsync(id);
        if (accessCard == null)
            throw new KeyNotFoundException("Access card not found");

        if (dto.CardType != null)
            accessCard.CardType = dto.CardType;
        if (dto.IsActive.HasValue)
            accessCard.IsActive = dto.IsActive.Value;
        if (dto.ExpiresAt.HasValue)
            accessCard.ExpiresAt = dto.ExpiresAt;

        accessCard = await _accessCardRepository.UpdateAsync(accessCard);

        if (dto.AccessPointIds != null)
        {
            foreach (var accessPointId in dto.AccessPointIds)
            {
                if (!await _accessPointRepository.ExistsAsync(accessPointId))
                    throw new KeyNotFoundException($"Access point with id {accessPointId} not found");
            }

            await _accessCardRepository.AssignAccessPointsAsync(accessCard.Id, dto.AccessPointIds);
        }

        return MapToDto(await _accessCardRepository.GetByIdAsync(accessCard.Id)!);
    }

    public async Task DeleteAsync(int id, int currentUserId, List<string> currentUserRoles)
    {
        if (!currentUserRoles.Contains("Admin"))
            throw new UnauthorizedAccessException("Only Admin can delete access cards");

        if (!await _accessCardRepository.ExistsAsync(id))
            throw new KeyNotFoundException("Access card not found");

        await _accessCardRepository.DeleteAsync(id);
    }

    private AccessCardDto MapToDto(AccessCard accessCard)
    {
        return new AccessCardDto
        {
            Id = accessCard.Id,
            CardNumber = accessCard.CardNumber,
            CardType = accessCard.CardType,
            IsActive = accessCard.IsActive,
            ExpiresAt = accessCard.ExpiresAt,
            AccessPointIds = accessCard.AccessCardAccessPoints.Select(acap => acap.AccessPointId).ToList(),
            CreatedAt = accessCard.CreatedAt
        };
    }
}


