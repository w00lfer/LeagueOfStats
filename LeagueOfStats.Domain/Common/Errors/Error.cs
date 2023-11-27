namespace LeagueOfStats.Domain.Common.Errors;

public abstract class Error
{
    protected Error(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
        
    public string ErrorMessage { get; }
}