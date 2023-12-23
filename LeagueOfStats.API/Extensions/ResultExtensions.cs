using LeagueOfStats.API.Common.Errors;
using LeagueOfStats.Application.Common.Errors;
using LeagueOfStats.Domain.Common.Rails.Errors;
using LeagueOfStats.Domain.Common.Rails.Results;
using Microsoft.AspNetCore.Mvc;

namespace LeagueOfStats.API.Extensions;

public static class ResultExtensions
{
    public static async Task<IActionResult> ToIActionResult(this Task<Result> taskResult, ControllerBase controllerBase)
    {
        var result = await taskResult;
        
        return result.IsSuccess
            ? controllerBase.Ok()
            : GetActionResultBasedOnErrorType(result.Errors, controllerBase);
    }

    public static async Task<IActionResult> ToIActionResult<T>(this Task<Result<T>> taskResult, ControllerBase controllerBase)
    {
        var result = await taskResult;
        
        return result.IsSuccess
            ? controllerBase.Ok(result.Value)
            : GetActionResultBasedOnErrorType(result.Errors, controllerBase);
    }
    
    // TODO refactor this method to create custom object result to not pass controller
    private static IActionResult GetActionResultBasedOnErrorType(Error[] errors, ControllerBase controllerBase)
    {
        var errorsAreOfSameType = errors.Select(e => e.GetType()).Distinct().Count() > 1;
        if (errorsAreOfSameType)
        {
            throw new NotSupportedException("Converting errors of more than one type is not yet supported.");
        }
        
        return errors.First() switch
        {
            ApiError apiError => controllerBase.BadRequest(apiError.ErrorMessage),
            ApplicationError applicationError => controllerBase.BadRequest(applicationError.ErrorMessage),
            DomainError domainError => controllerBase.BadRequest(domainError.ErrorMessage),
            EntityNotFoundError entityNotFoundError => controllerBase.NotFound(entityNotFoundError.ErrorMessage),
            ValidationError validationError => controllerBase.BadRequest(validationError.ErrorMessage),
            _ => throw new NotSupportedException("This error is not yet supported.")
        };
    }
}