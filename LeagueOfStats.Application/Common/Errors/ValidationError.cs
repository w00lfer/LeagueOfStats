using FluentValidation.Results;
using LeagueOfStats.Domain.Common.Rails.Errors;

namespace LeagueOfStats.Application.Common.Errors;

public class ValidationError : Error
{
    // #TODO refactor error class to somehow accept more validation messages
    public ValidationError(IEnumerable<ValidationFailure> failures) : base(string.Join(Environment.NewLine, failures.Select(f => f.ErrorMessage)))
    {
    }
    
    public ValidationError(ValidationFailure failure) : base(failure.ErrorMessage)
    {
    }
}