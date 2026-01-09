namespace WebApplication1.Models.DTO;

public class AccessCardDto
{
    public int Id { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public string? CardType { get; set; }
    public bool IsActive { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public List<int> AccessPointIds { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}


