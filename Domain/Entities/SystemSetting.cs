using Domain.Common;

namespace Domain.Entities;

public class SystemSetting : AuditableEntity
{
    /// <summary>
    /// Gets or sets the name of the system setting.
    /// </summary>
    public string SettingName { get; set; }
    /// <summary>
    /// Gets or sets the value of the system setting.
    /// </summary>
    public string SettingValue { get; set; }
    /// <summary>
    /// Gets or sets the description of the system setting.
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Gets or sets the data type of the system setting value.
    /// </summary>
    public string DataType { get; set; }
    /// <summary>
    /// Gets or sets the ID of the user who last modified this setting.
    /// </summary>
    public Guid? LastModifiedByUserId { get; set; }
    // public DateTime LastModifiedAt { get; set; } = DateTime.Now; // To be removed

    // العلاقات
    /// <summary>
    /// Gets or sets the user who last modified this system setting.
    /// </summary>
    public User LastModifiedByUser { get; set; }
}
