using LanguageExt;
using LeagueOfStats.API.Common.Errors;
using LeagueOfStats.Application.Common.Errors;
using LeagueOfStats.Domain.Common.Errors;
using Microsoft.AspNetCore.Mvc;

namespace LeagueOfStats.API.Extensions;

public static class EitherExtensions
{
    public static IActionResult ToIActionResult<L, R>(this Either<L,R>either, ControllerBase controllerBase) 
        where L: Error
        where R: class =>
        either.Match(
            r => controllerBase.Ok(r) as IActionResult,
            l => GetActionResultBasedOnErrorType(l, controllerBase)) as IActionResult;

    // TODO refactor this method to create custom object resukt to not pass controller
    private static IActionResult GetActionResultBasedOnErrorType(Error error, ControllerBase controllerBase) =>
        error switch
        {
            ApplicationError applicationError => controllerBase.BadRequest(applicationError.ErrorMessage),
            DomainError domainError => controllerBase.BadRequest(domainError.ErrorMessage),
            ApiError apiError => controllerBase.BadRequest(apiError.ErrorMessage),
            _ => throw new NotSupportedException("This error is not yet supported3")
        };
}