namespace LeagueOfStats.Domain.Common.Entities;

public abstract class AggregateRoot : Entity
{
    protected AggregateRoot(Guid id) 
        : base(id)
    {
    }
}