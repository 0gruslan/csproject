namespace WebApplication1.Models.Entities;

public class UserAccessCard
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public int AccessCardId { get; set; }
    public AccessCard AccessCard { get; set; } = null!;
    
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
}


