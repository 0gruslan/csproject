namespace WebApplication1.Models.DTO;

public class UpdateUserDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public bool? IsActive { get; set; }
    public List<int>? RoleIds { get; set; }
}


