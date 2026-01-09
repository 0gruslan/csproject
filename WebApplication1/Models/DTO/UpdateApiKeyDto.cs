namespace WebApplication1.Models.DTO;

public class UpdateApiKeyDto
{
    public string? Name { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? ExpiresAt { get; set; }
}

