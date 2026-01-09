namespace WebApplication1.Models.Entities;

public class AccessCardAccessPoint
{
    public int AccessCardId { get; set; }
    public AccessCard AccessCard { get; set; } = null!;
    
    public int AccessPointId { get; set; }
    public AccessPoint AccessPoint { get; set; } = null!;
}


