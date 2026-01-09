using WebApplication1.Models.Entities;

namespace WebApplication1.Repositories.Interfaces;

public interface IAccessCardRepository
{
    Task<AccessCard?> GetByIdAsync(int id);
    Task<AccessCard?> GetByCardNumberAsync(string cardNumber);
    Task<List<AccessCard>> GetAllAsync();
    Task<AccessCard> CreateAsync(AccessCard accessCard);
    Task<AccessCard> UpdateAsync(AccessCard accessCard);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task AssignAccessPointsAsync(int cardId, List<int> accessPointIds);
}


