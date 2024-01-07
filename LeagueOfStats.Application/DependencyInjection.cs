using System.Reflection;
using FluentValidation;
using LeagueOfStats.Application.Common.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace LeagueOfStats.Application;

public static class DependencyInjection
{
    public static void AddApplicationDI(this IServiceCollection services)
    {
        AddMediatR(services);
        AddValidation(services);
    }

    private static void AddMediatR(IServiceCollection services)
    {
        services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
    }

    private static void AddValidation(IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        ValidatorOptions.Global.LanguageManager.Enabled = false;

        services.AddScoped(typeof(Common.Validators.IValidator<>), typeof(Validator<>));
    }
}