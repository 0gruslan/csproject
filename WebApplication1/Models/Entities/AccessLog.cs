namespace WebApplication1.Models.Entities;

public class AccessLog
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }
    
    public int? AccessCardId { get; set; }
    public AccessCard? AccessCard { get; set; }
    
    public int AccessPointId { get; set; }
    public AccessPoint AccessPoint { get; set; } = null!;
    
    public string AccessResult { get; set; } = string.Empty; // "GRANTED" or "DENIED"
    public DateTime AccessTime { get; set; } = DateTime.UtcNow;
    public string? Reason { get; set; }
}


