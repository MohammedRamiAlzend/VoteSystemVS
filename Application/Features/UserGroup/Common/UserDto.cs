namespace Application.Features.UserGroup.Common;

public class UserDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public int CreatedByAdminId { get; set; }
}