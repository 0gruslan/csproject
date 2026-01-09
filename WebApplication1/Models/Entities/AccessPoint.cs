namespace WebApplication1.Models.Entities;

public class AccessPoint
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Location { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<AccessCardAccessPoint> AccessCardAccessPoints { get; set; } = new List<AccessCardAccessPoint>();
    public ICollection<AccessLog> AccessLogs { get; set; } = new List<AccessLog>();
}


