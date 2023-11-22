using LeagueOfStats.Domain.Common.Errors;

namespace LeagueOfStats.API.Common.Errors
{
    public class ApiError : Error
    {
        public ApiError(string errorMessage) : base(errorMessage)
        {
        }
    }
}