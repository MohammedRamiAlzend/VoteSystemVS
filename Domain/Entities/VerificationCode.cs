using Domain.Common;

namespace Domain.Entities;
public class VerificationCode : AuditableEntity
{
    /// <summary>
    /// Gets or sets the ID of the user this verification code is for.
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// Gets or sets the ID of the voting session this verification code is for.
    /// </summary>
    public Guid VotingId { get; set; }
    /// <summary>
    /// Gets or sets the actual verification code string.
    /// </summary>
    public string Code { get; set; }
    /// <summary>
    /// Gets or sets the expiry date and time of the verification code.
    /// </summary>
    public DateTime ExpiryDate { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the verification code has been used.
    /// </summary>
    public bool Used { get; set; } = false;
    // public DateTime? UsedAt { get; set; } // To be removed, can be derived from LastModifiedUtc if Used is true

    // العلاقات
    /// <summary>
    /// Gets or sets the user associated with this verification code.
    /// </summary>
    public User User { get; set; }
    /// <summary>
    /// Gets or sets the voting session associated with this verification code.
    /// </summary>
    public VotingSession VotingSession { get; set; }
}
