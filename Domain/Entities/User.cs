using Domain.Common;

namespace Domain.Entities;

public class User : Entity
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }

    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public int CreatedByAdminId { get; set; }
    public Admin CreatedByAdmin { get; set; }

}
