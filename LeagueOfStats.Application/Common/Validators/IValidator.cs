using LeagueOfStats.Domain.Common.Rails.Results;

namespace LeagueOfStats.Application.Common.Validators;

public interface IValidator<T>
{
    Task<Result> ValidateAsyncTwo(T objectToValidate);
}