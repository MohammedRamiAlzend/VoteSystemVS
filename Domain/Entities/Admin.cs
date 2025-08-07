using Domain.Common;

namespace Domain.Entities;

public class Admin : Entity
{
    public string UserName { get; set; }
    public string HashedPassword { get; set; }
    public ICollection<User> CreatedUsersByAdmin { get; set; } = [];
    public ICollection<AttendanceUser> CreatedAttendanceUsersByAdmin { get; set; } = [];
}
