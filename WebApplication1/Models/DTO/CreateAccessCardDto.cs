namespace WebApplication1.Models.DTO;

public class CreateAccessCardDto
{
    public string CardNumber { get; set; } = string.Empty;
    public string? CardType { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public List<int> AccessPointIds { get; set; } = new();
}


