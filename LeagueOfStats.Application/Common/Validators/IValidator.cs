using LanguageExt;
using LeagueOfStats.Domain.Common.Errors;

namespace LeagueOfStats.Application.Common.Validators;

public interface IValidator<T>
{
    Task<Option<Error>> ValidateAsync(T objectToValidate);
}