using LeagueOfStats.Domain.Common.Rails.Errors;

namespace LeagueOfStats.Jobs.Common.Errors;

public class JobError : Error
{
    public JobError(string errorMessage) : base(errorMessage)
    {
    }
}