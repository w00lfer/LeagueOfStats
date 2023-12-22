using FluentValidation.Results;
using LanguageExt;
using LeagueOfStats.Application.Common.Errors;
using LeagueOfStats.Domain.Common.Rails.Errors;

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
}