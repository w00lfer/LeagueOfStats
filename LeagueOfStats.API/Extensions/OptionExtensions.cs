using LanguageExt;
using LeagueOfStats.API.Common.Errors;
using LeagueOfStats.Application.Common.Errors;
using LeagueOfStats.Domain.Common.Errors;
using Microsoft.AspNetCore.Mvc;

namespace LeagueOfStats.API.Extensions;

public static class OptionExtensions
{
    public static IActionResult ToIActionResult(this Option<Error> option, ControllerBase controllerBase) =>
        option.Match<IActionResult>(
            error => GetActionResultBasedOnErrorType(error, controllerBase),
            () => controllerBase.Ok());
    
    public static Task<IActionResult> ToIActionResult(this Task<Option<Error>> option, ControllerBase controllerBase) =>
        option
            .ToAsync()
            .Match<IActionResult>(
            error => GetActionResultBasedOnErrorType(error, controllerBase),
            () => controllerBase.Ok());


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