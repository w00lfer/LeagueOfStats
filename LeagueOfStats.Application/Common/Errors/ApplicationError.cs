using LeagueOfStats.Domain.Common.Errors;

namespace LeagueOfStats.Application.Common.Errors;

public class ApplicationError : Error
{
    public ApplicationError(string errorMessage) : base(errorMessage)
    {
    }
}