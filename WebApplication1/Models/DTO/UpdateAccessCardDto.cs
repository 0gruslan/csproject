namespace WebApplication1.Models.DTO;

public class UpdateAccessCardDto
{
    public string? CardType { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public List<int>? AccessPointIds { get; set; }
}


