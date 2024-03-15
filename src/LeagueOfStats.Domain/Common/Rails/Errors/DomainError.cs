namespace LeagueOfStats.Domain.Common.Rails.Errors;

public class DomainError : Error
{
    public DomainError(string errorMessage) : base(errorMessage)
    {
    }
}