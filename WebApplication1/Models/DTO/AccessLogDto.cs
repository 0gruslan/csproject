namespace WebApplication1.Models.DTO;

public class AccessLogDto
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string? Username { get; set; }
    public int? AccessCardId { get; set; }
    public string? CardNumber { get; set; }
    public int AccessPointId { get; set; }
    public string AccessPointName { get; set; } = string.Empty;
    public string AccessResult { get; set; } = string.Empty;
    public DateTime AccessTime { get; set; }
    public string? Reason { get; set; }
}


