namespace WebApplication1.Models.Entities;

public class AccessCard
{
    public int Id { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public string? CardType { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<UserAccessCard> UserAccessCards { get; set; } = new List<UserAccessCard>();
    public ICollection<AccessCardAccessPoint> AccessCardAccessPoints { get; set; } = new List<AccessCardAccessPoint>();
    public ICollection<AccessLog> AccessLogs { get; set; } = new List<AccessLog>();
}


