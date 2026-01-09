namespace WebApplication1.Models.DTO;

public class CreateApiKeyDto
{
    public string Name { get; set; } = string.Empty;
    public DateTime? ExpiresAt { get; set; }
}

