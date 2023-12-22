using FluentValidation.Results;
using LanguageExt;
using LeagueOfStats.Application.Common.Errors;
using LeagueOfStats.Domain.Common.Rails.Errors;
using LeagueOfStats.Domain.Common.Rails.Results;

namespace LeagueOfStats.Application.Common.Validators;

public class Validator<T> : IValidator<T>
{
    private readonly FluentValidation.IValidator<T> _validator;

    public Validator(FluentValidation.IValidator<T> validator)
    {
        _validator = validator;
    }
    
    public async Task<Option<Error>> ValidateAsync(T objectToValidate)
    {
        ValidationResult result = await _validator.ValidateAsync(objectToValidate);

        return result.IsValid
            ? Option<Error>.None
            : Option<Error>.Some(new ValidationError(result.Errors));
    }

    public async Task<Result> ValidateAsyncTwo(T objectToValidate)
    {
        ValidationResult result = await _validator.ValidateAsync(objectToValidate);

        return result.IsValid
            ? Result.Success()
            : Result.Failure(result.Errors.Select(f => new ValidationError(f)).ToArray());
    }
}