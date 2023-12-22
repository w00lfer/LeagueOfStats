using LanguageExt;
using LeagueOfStats.Domain.Common.Rails.Errors;
using LeagueOfStats.Domain.Common.Rails.Results;

namespace LeagueOfStats.Application.Common.Validators;

public interface IValidator<T>
{
    Task<Option<Error>> ValidateAsync(T objectToValidate);
    
    Task<Result> ValidateAsyncTwo(T objectToValidate);
}