using LeagueOfStats.Domain.Common.Rails.Results;

namespace LeagueOfStats.Domain.Common.Rails.Errors;

public abstract class Error
{
    protected Error(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
        
    public string ErrorMessage { get; }

    public static implicit operator Result(Error error) => Result.Failure(error);
}