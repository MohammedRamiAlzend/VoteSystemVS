using Domain.Common;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class VotingItem : AuditableEntity
{
    /// <summary>
    /// Gets or sets the ID of the voting session this item belongs to.
    /// </summary>
    public Guid VotingSessionId { get; set; }
    /// <summary>
    /// Gets or sets the description of the voting item.
    /// </summary>
    public string ItemDescription { get; set; }
    /// <summary>
    /// Gets or sets the type of voting for this item (e.g., "YesNo", "MultipleChoice").
    /// </summary>
    public string VotingType { get; set; } // e.g., "YesNo", "MultipleChoice"
    /// <summary>
    /// Gets or sets the current status of the voting item (e.g., "Pending", "Active", "Closed").
    /// </summary>
    public string ItemStatus { get; set; } // e.g., "Pending", "Active", "Closed"

    // Relationships
    /// <summary>
    /// Gets or sets the voting session this item belongs to.
    /// </summary>
    public VotingSession VotingSession { get; set; }
    /// <summary>
    /// Gets or sets the collection of options available for this voting item.
    /// </summary>
    public ICollection<VotingOption> VotingOptions { get; set; } = new List<VotingOption>();
    /// <summary>
    /// Gets or sets the collection of votes cast for this voting item.
    /// </summary>
    public ICollection<Vote> Votes { get; set; } = new List<Vote>();
    /// <summary>
    /// Gets or sets the voting result for this item.
    /// </summary>
    public VotingResult Result { get; set; } // One-to-one relationship
}