using LanguageExt;
using LeagueOfStats.API.Common.Errors;
using LeagueOfStats.Application.Common.Errors;
using LeagueOfStats.Domain.Common.Errors;
using Microsoft.AspNetCore.Mvc;

namespace LeagueOfStats.API.Extensions;

public static class EitherExtensions
{
    public static IActionResult ToIActionResult<L, R>(this Either<L,R> either, ControllerBase controllerBase) 
        where L: Error
        where R: class =>
        either
            .Match(
            r => controllerBase.Ok(r),
            l => GetActionResultBasedOnErrorType(l, controllerBase));
    
    public static Task<IActionResult> ToIActionResult<L, R>(this Task<Either<L,R>> either, ControllerBase controllerBase) 
        where L: Error
        where R: class =>
        either
            .ToAsync()
            .Match(
            r => controllerBase.Ok(r),
            l => GetActionResultBasedOnErrorType(l, controllerBase));
    
    // TODO refactor this method to create custom object result to not pass controller
    private static IActionResult GetActionResultBasedOnErrorType(Error error, ControllerBase controllerBase) =>
        error switch
        {
            ApiError apiError => controllerBase.BadRequest(apiError.ErrorMessage),
            ApplicationError applicationError => controllerBase.BadRequest(applicationError.ErrorMessage),
            DomainError domainError => controllerBase.BadRequest(domainError.ErrorMessage),
            EntityNotFoundError entityNotFoundError => controllerBase.NotFound(entityNotFoundError.ErrorMessage),
            ValidationError validationError => controllerBase.BadRequest(validationError.ErrorMessage),
            _ => throw new NotSupportedException("This error is not yet supported.")
        };
}