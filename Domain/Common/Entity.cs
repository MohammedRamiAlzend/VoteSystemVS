using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Common;

/// <summary>
/// Base class for all domain entities, providing identity and domain event management.
/// </summary>
public abstract class Entity
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public int Id { get; set; }

    [NotMapped]
    private readonly List<DomainEvents> _domainEvents = new();

    /// <summary>
    /// Gets the domain events associated with this entity.
    /// </summary>
    [NotMapped]
    public IReadOnlyCollection<DomainEvents> DomainEvents => _domainEvents.AsReadOnly();

    protected Entity() { }

    protected Entity(int id)
    {
        Id = id;
    }

    /// <summary>
    /// Adds a domain event to the entity.
    /// </summary>
    public void AddDomainEvent(DomainEvents domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Removes a domain event from the entity.
    /// </summary>
    public void RemoveDomainEvent(DomainEvents domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    /// <summary>
    /// Clears all domain events from the entity.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
