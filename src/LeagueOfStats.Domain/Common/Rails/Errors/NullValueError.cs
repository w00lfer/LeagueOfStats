namespace LeagueOfStats.Domain.Common.Rails.Errors;

public class NullValueError: Error
{
    public NullValueError() : base("Null value provided")
    {
    }
}