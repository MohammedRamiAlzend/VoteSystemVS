namespace Domain.Common;

public abstract class Entity
{

    public int Id { get; set; }

    protected Entity() { }

    protected Entity(int id)
    {
        Id = id;
    }


}
