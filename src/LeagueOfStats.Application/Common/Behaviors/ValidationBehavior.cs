using System.Reflection;
using FluentValidation;
using FluentValidation.Results;
using LeagueOfStats.Application.Common.Errors;
using LeagueOfStats.Domain.Common.Rails.Errors;
using LeagueOfStats.Domain.Common.Rails.Results;
using MediatR;

namespace LeagueOfStats.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validators.Any() is false)
        {
            return await next();
        }

        ValidationResult[] validationResults = await Task.WhenAll(
            _validators.Select(async validator => await validator.ValidateAsync(request, cancellationToken)));

        ValidationError[] errors = validationResults
            .SelectMany(vr => vr.Errors)
            .Where(vf => vf is not null)
            .Select(vf => new ValidationError(vf))
            .Distinct()
            .ToArray();

        if (errors.Any())
        {
            return CreateValidationResult<TResponse>(errors);
        }

        return await next();
    }

    private static TResult CreateValidationResult<TResult>(ValidationError[] errors)
        where TResult : Result
    {
        if (typeof(TResult) == typeof(Result))
        {
            return (Result.Failure(errors) as TResult)!;
        }

        MethodInfo failureMethod = 
                typeof(Result).GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m.Name == nameof(Result.Failure))
                .Single(m => m.IsGenericMethod && m.GetParameters().Length == 1 &&
                             m.GetParameters()[0].ParameterType == typeof(Error[]));

        MethodInfo genericFailureMethod = failureMethod.MakeGenericMethod(typeof(TResult).GenericTypeArguments[0]);
        
        object validationResult = genericFailureMethod.Invoke(null, new object?[] { errors })!;

        return (TResult)validationResult;
    }
}