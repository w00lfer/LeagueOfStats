using System.Reflection;
using FluentValidation;
using LeagueOfStats.Application.Common.Validators;
using LeagueOfStats.Application.Summoners;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LeagueOfStats.Application;

public static class DependencyInjection
{
    public static void AddApplicationDI(this IServiceCollection services, IConfiguration configuration)
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