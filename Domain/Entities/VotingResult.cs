using Domain.Common;
using System;

namespace Domain.Entities;

public class VotingResult : AuditableEntity
{
    /// <summary>
    /// Gets or sets the ID of the voting item this result is for.
    /// </summary>
    public Guid VotingItemId { get; set; }
    /// <summary>
    /// Gets or sets the JSON string containing vote counts for each option.
    /// </summary>
    public string VoteCountsJson { get; set; } // Stores vote counts for each option as JSON
    /// <summary>
    /// Gets or sets the JSON string containing percentages for each option.
    /// </summary>
    public string PercentageJson { get; set; } // Stores percentages for each option as JSON
    /// <summary>
    /// Gets or sets the status of the voting result (e.g., "Preliminary", "Final").
    /// </summary>
    public string ResultStatus { get; set; } // e.g., "Preliminary", "Final"

    // Relationships
    /// <summary>
    /// Gets or sets the voting item associated with this result.
    /// </summary>
    public VotingItem VotingItem { get; set; }
}