using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using WebApplication1.Data;
using WebApplication1.Models.Entities;
using WebApplication1.Repositories.Interfaces;

namespace WebApplication1.Repositories;

public class AccessCardRepository : IAccessCardRepository
{
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;

    public AccessCardRepository(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
    }

    public async Task<AccessCard?> GetByIdAsync(int id)
    {
        return await _context.AccessCards
            .Include(ac => ac.AccessCardAccessPoints)
            .FirstOrDefaultAsync(ac => ac.Id == id);
    }

    public async Task<AccessCard?> GetByCardNumberAsync(string cardNumber)
    {
        return await _context.AccessCards
            .Include(ac => ac.AccessCardAccessPoints)
            .FirstOrDefaultAsync(ac => ac.CardNumber == cardNumber);
    }

    public async Task<List<AccessCard>> GetAllAsync()
    {
        return await _context.AccessCards
            .Include(ac => ac.AccessCardAccessPoints)
            .ToListAsync();
    }

    public async Task<AccessCard> CreateAsync(AccessCard accessCard)
    {
        accessCard.CreatedAt = DateTime.UtcNow;
        accessCard.UpdatedAt = DateTime.UtcNow;
        if (accessCard.ExpiresAt.HasValue && accessCard.ExpiresAt.Value.Kind != DateTimeKind.Utc)
            accessCard.ExpiresAt = accessCard.ExpiresAt.Value.ToUniversalTime();
        
        _context.AccessCards.Add(accessCard);
        await _context.SaveChangesAsync();
        return accessCard;
    }

    public async Task<AccessCard> UpdateAsync(AccessCard accessCard)
    {
        accessCard.UpdatedAt = DateTime.UtcNow;
        if (accessCard.CreatedAt.Kind != DateTimeKind.Utc)
            accessCard.CreatedAt = accessCard.CreatedAt.ToUniversalTime();
        if (accessCard.ExpiresAt.HasValue && accessCard.ExpiresAt.Value.Kind != DateTimeKind.Utc)
            accessCard.ExpiresAt = accessCard.ExpiresAt.Value.ToUniversalTime();
        
        _context.AccessCards.Update(accessCard);
        await _context.SaveChangesAsync();
        return accessCard;
    }

    public async Task DeleteAsync(int id)
    {
        var accessCard = await _context.AccessCards.FindAsync(id);
        if (accessCard != null)
        {
            _context.AccessCards.Remove(accessCard);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.AccessCards.AnyAsync(ac => ac.Id == id);
    }

    public async Task AssignAccessPointsAsync(int cardId, List<int> accessPointIds)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        
        using var transaction = await connection.BeginTransactionAsync();
        
        try
        {
            await connection.ExecuteAsync(
                "DELETE FROM access_card_access_points WHERE access_card_id = @CardId",
                new { CardId = cardId },
                transaction);

            if (accessPointIds.Any())
            {
                var insertSql = @"
                    INSERT INTO access_card_access_points (access_card_id, access_point_id)
                    VALUES (@CardId, @AccessPointId)";

                foreach (var accessPointId in accessPointIds)
                {
                    await connection.ExecuteAsync(
                        insertSql,
                        new { CardId = cardId, AccessPointId = accessPointId },
                        transaction);
                }
            }

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}


