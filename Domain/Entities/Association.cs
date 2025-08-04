using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Association : AuditableEntity
{
    /// <summary>
    /// Gets or sets the name of the association.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Gets or sets the description of the association.
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Gets or sets the type of the association (e.g., "Non-profit", "Commercial").
    /// </summary>
    public string AssociationType { get; set; } // e.g., "Non-profit", "Commercial", etc.

    // Relationships
    /// <summary>
    /// Gets or sets the collection of members belonging to this association.
    /// </summary>
    public ICollection<User> Members { get; set; } = new List<User>();
    /// <summary>
    /// Gets or sets the collection of general assemblies held by this association.
    /// </summary>
    public ICollection<VotingSession> GeneralAssemblies { get; set; } = new List<VotingSession>();
}