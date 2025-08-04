namespace Domain.Common;

public abstract class Entity
{
    public Guid Id { get; set; }

    private readonly List<DomainEvents> _domainEvents = [];
    protected Entity() { }

    protected Entity(Guid id)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
    }
    public void AddDomainEvent(DomainEvents domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    public void RemoveDomainEvent(DomainEvents domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvent()
    {
        _domainEvents.Clear();
    }

}
