using Domain.Common;

namespace Domain.Entities;

public class VotingSession : AuditableEntity
{
    /// <summary>
    /// Gets or sets the title of the voting session.
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// Gets or sets the description of the voting session.
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Gets or sets the start date and time of the voting session.
    /// </summary>
    public DateTime StartDateTime { get; set; }
    /// <summary>
    /// Gets or sets the end date and time of the voting session.
    /// </summary>
    public DateTime EndDateTime { get; set; }
    /// <summary>
    /// Gets or sets the ID of the association this voting session belongs to.
    /// </summary>
    public Guid AssociationId { get; set; }

    // الحالة يمكن أن تكون: Scheduled, Active, Completed, Cancelled
    /// <summary>
    /// Gets or sets the current status of the voting session (e.g., Scheduled, Active, Completed, Cancelled).
    /// </summary>
    public string Status { get; set; } = "Scheduled";

    // معلومات إنشاء الجلسة
    /// <summary>
    /// Gets or sets the ID of the user who created this voting session.
    /// </summary>
    public Guid CreatedByUserId { get; set; }
    // public DateTime CreatedAt { get; set; } = DateTime.Now; // To be removed
    /// <summary>
    /// Gets or sets the ID of the user who last modified this voting session.
    /// </summary>
    public Guid? ModifiedByUserId { get; set; }
    // public DateTime? ModifiedAt { get; set; } // To be removed

    // إعدادات التصويت
    /// <summary>
    /// Gets or sets a value indicating whether the voting is secret.
    /// </summary>
    public bool IsSecret { get; set; } = true;
    /// <summary>
    /// Gets or sets the required quorum for the voting session.
    /// </summary>
    public int QuorumRequired { get; set; } = 0;
    /// <summary>
    /// Gets or sets the minimum number of votes required for the voting session.
    /// </summary>
    public int MinVotesRequired { get; set; } = 0;

    // العلاقات
    /// <summary>
    /// Gets or sets the user who created this voting session.
    /// </summary>
    public User CreatedByUser { get; set; }
    /// <summary>
    /// Gets or sets the user who last modified this voting session.
    /// </summary>
    public User ModifiedByUser { get; set; }
    /// <summary>
    /// Gets or sets the association this voting session belongs to.
    /// </summary>
    public Association Association { get; set; }
    /// <summary>
    /// Gets or sets the collection of voting items within this session.
    /// </summary>
    public ICollection<VotingItem> VotingItems { get; set; } = new List<VotingItem>();
    /// <summary>
    /// Gets or sets the collection of individual votes cast in this session.
    /// </summary>
    public ICollection<Vote> IndividualVotes { get; set; } = new List<Vote>();
    /// <summary>
    /// Gets or sets the collection of verification codes associated with this voting session.
    /// </summary>
    public ICollection<VerificationCode> VerificationCodes { get; set; } = new List<VerificationCode>();

}
