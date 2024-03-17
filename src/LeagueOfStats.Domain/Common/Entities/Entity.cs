namespace LeagueOfStats.Domain.Common.Entities;

public abstract class Entity : IEquatable<Entity>
{
    public virtual Guid Id { get; protected set; }

    protected Entity(Guid id)
    {
        Id = id;
    }

    public override bool Equals(object? obj) => 
        obj is Entity entity && Id.Equals(entity.Id);

    public override int GetHashCode() => 
        Id.GetHashCode();

    public bool Equals(Entity? other) =>
        Equals((object?)other);
    
    public static bool operator ==(Entity left, Entity right) => 
        Equals(left, right);

    public static bool operator !=(Entity left, Entity right) => 
        !Equals(left, right);
}