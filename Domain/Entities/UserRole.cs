using Domain.Common;

namespace Domain.Entities;

public class UserRole:AuditableEntity
{
    // لا حاجة لـ UserRoleId لأن Id موجود في الكيان الأساسي

    // المفاتيح الخارجية
    /// <summary>
    /// Gets or sets the ID of the user.
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// Gets or sets the ID of the role.
    /// </summary>
    public Guid RoleId { get; set; }
    /// <summary>
    /// Gets or sets the ID of the user who assigned this role.
    /// </summary>
    public Guid AssignedByUserId { get; set; }

    // تواريخ
    // public DateTime AssignmentDate { get; set; } = DateTime.Now; // To be removed

    // العلاقات
    /// <summary>
    /// Gets or sets the user associated with this user role.
    /// </summary>
    public User User { get; set; }
    /// <summary>
    /// Gets or sets the role associated with this user role.
    /// </summary>
    public Role Role { get; set; }
    /// <summary>
    /// Gets or sets the user who assigned this role.
    /// </summary>
    public User AssignedByUser { get; set; }
}
