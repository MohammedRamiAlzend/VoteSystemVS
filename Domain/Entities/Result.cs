using Domain.Common;
using System;

namespace Domain.Entities;

public class Result : AuditableEntity
{
    public Guid VotingItemId { get; set; }
    public string VoteCountsJson { get; set; } // Stores vote counts for each option as JSON
    public string PercentageJson { get; set; } // Stores percentages for each option as JSON
    public string ResultStatus { get; set; } // e.g., "Preliminary", "Final"

    // Relationships
    public VotingItem VotingItem { get; set; }
}