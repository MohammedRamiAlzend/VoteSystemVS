using Domain.Common;

namespace Domain.Entities;

public class Role : AuditableEntity
{
    /// <summary>
    /// Gets or sets the name of the role.
    /// </summary>
    public string RoleName { get; set; }
    /// <summary>
    /// Gets or sets the description of the role.
    /// </summary>
    public string Description { get; set; }

    // صلاحيات الدور
    /// <summary>
    /// Gets or sets a value indicating whether users with this role can create voting sessions.
    /// </summary>
    public bool CanCreateVoting { get; set; } = false;
    /// <summary>
    /// Gets or sets a value indicating whether users with this role can manage other users.
    /// </summary>
    public bool CanManageUsers { get; set; } = false;
    /// <summary>
    /// Gets or sets a value indicating whether users with this role can view voting results.
    /// </summary>
    public bool CanViewResults { get; set; } = false;
    /// <summary>
    /// Gets or sets a value indicating whether users with this role can modify system settings.
    /// </summary>
    public bool CanModifySettings { get; set; } = false;

    // العلاقات
    /// <summary>
    /// Gets or sets the collection of user roles associated with this role.
    /// </summary>
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    // لإنشاء علاقة many-to-many ضمنية مع User
    /// <summary>
    /// Gets or sets the collection of users associated with this role (implicit many-to-many).
    /// </summary>
    public ICollection<User> Users { get; set; } = new List<User>();
}
