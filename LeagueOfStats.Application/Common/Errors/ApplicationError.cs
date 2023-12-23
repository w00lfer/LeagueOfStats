using LeagueOfStats.Domain.Common.Rails.Errors;

namespace LeagueOfStats.Application.Common.Errors;

public class ApplicationError : Error
{
    public ApplicationError(string errorMessage) : base(errorMessage)
    {
    }
}