using Domain.Common;

namespace Domain.Entities;

public class AuditLog : Entity
{
    /// <summary>
    /// Gets or sets the ID of the user who performed the action.
    /// </summary>
    public Guid? UserId { get; set; }
    /// <summary>
    /// Gets or sets the type of action performed.
    /// </summary>
    public string Action { get; set; }
    /// <summary>
    /// Gets or sets a description of the audited activity.
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Gets or sets the IP address from which the action was performed.
    /// </summary>
    public string IpAddress { get; set; }
    /// <summary>
    /// Gets or sets information about the device used for the action.
    /// </summary>
    public string DeviceInfo { get; set; }
    /// <summary>
    /// Gets or sets the timestamp when the action occurred.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.Now;
    /// <summary>
    /// Gets or sets the name of the related entity (e.g., "User", "VotingSession").
    /// </summary>
    public string RelatedEntity { get; set; }
    /// <summary>
    /// Gets or sets the ID of the related entity.
    /// </summary>
    public Guid? RelatedEntityId { get; set; }

    // العلاقات
    /// <summary>
    /// Gets or sets the user associated with this audit log entry.
    /// </summary>
    public User User { get; set; }
}
