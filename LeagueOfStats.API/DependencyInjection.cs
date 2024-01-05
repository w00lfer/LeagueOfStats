using System.Text.Json.Serialization;
using Azure.Identity;
using FluentValidation.AspNetCore;
using LeagueOfStats.API.Environments;
using LeagueOfStats.API.Infrastructure.RiotClient;
using LeagueOfStats.API.Options;
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

        AddSwagger(builder);

        AddKeyVault(builder);

        services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();

        services.Configure<EntityUpdateLockoutOptions>(
            builder.Configuration.GetSection(nameof(EntityUpdateLockoutOptions)));
        services.Configure<RiotApiKeyOptions>(
            builder.Configuration.GetSection(nameof(RiotApiKeyOptions)));

        services.AddSingleton<IEntityUpdateLockoutService, EntityUpdateLockoutService>();
        services.AddScoped<IRiotClient, RiotClient>();
    }

    private static void AddSwagger(WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
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
                ManagedIdentityClientId = builder.Configuration.GetSection("ManagedIdentityClientId")
                    .Value!,
            });
        var keyVaultUrl = new Uri((builder.Configuration.GetSection("KeyVaultURL")
            .Value!));

        builder.Configuration.AddAzureKeyVault(keyVaultUrl,
            azureCredential);
    }
}