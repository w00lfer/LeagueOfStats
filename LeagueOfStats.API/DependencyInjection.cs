using System.Text.Json.Serialization;
using Azure.Identity;
using FluentValidation.AspNetCore;
using LeagueOfStats.API.Configurations;
using LeagueOfStats.API.Configurations.Options;
using LeagueOfStats.API.Environments;
using LeagueOfStats.API.Infrastructure.RiotClient;
using LeagueOfStats.Application.Common;
using LeagueOfStats.Application.RiotClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

namespace LeagueOfStats.API;

public static class DependencyInjection
{
    public static void AddApiDI(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

        AddSwagger(builder.Services);

        AddKeyVault(builder);

        services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();

        AddOptions(builder);

        services.AddSingleton<IEntityUpdateLockoutService, EntityUpdateLockoutService>();
        services.AddScoped<IRiotClient, RiotClient>();
    }

    private static void AddSwagger(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.AddEnumsWithValuesFixFilters();
            c.MapType<Instant>(() => new OpenApiSchema
            {
                Type = "string"
            });
        });
    }

    private static void AddKeyVault(WebApplicationBuilder builder)
    {
        var azureCredential = new DefaultAzureCredential(
            new DefaultAzureCredentialOptions
            {
                ManagedIdentityClientId = builder.Configuration.GetSection(AppSettingsNameConstants.ManagedIdentityClientId)
                    .Value!,
            });
        var keyVaultUrl = new Uri(builder.Configuration.GetSection(AppSettingsNameConstants.KeyVaultURL)
            .Value!);

        builder.Configuration.AddAzureKeyVault(keyVaultUrl,
            azureCredential);
    }

    private static void AddOptions(WebApplicationBuilder builder)
    {
        builder.Services.Configure<EntityUpdateLockoutOptions>(
            builder.Configuration.GetSection(nameof(EntityUpdateLockoutOptions)));
        builder.Services.Configure<RiotApiKeyOptions>(
            builder.Configuration.GetSection(nameof(RiotApiKeyOptions)));
        builder.Services.Configure<DatabaseOptions>(
            builder.Configuration.GetSection(nameof(DatabaseOptions)));
    }
}