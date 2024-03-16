namespace LeagueOfStats.Domain.Common.Rails.Errors;

public class EntityNotFoundError: Error
{
    public EntityNotFoundError(string errorMessage) : base(errorMessage)
    {
    }
}