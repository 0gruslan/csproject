namespace WebApplication1.Models.DTO;

public class ApiKeyDto
{
    public int Id { get; set; }
    public string KeyValue { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastUsedAt { get; set; }
}

