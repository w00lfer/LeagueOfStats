namespace LeagueOfStats.Domain.Common.Errors
{
    public abstract class Error
    {
        public Error(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
        
        public string ErrorMessage { get; }
    }
}