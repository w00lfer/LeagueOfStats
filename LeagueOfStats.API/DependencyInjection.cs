using System.Text.Json.Serialization;
using Azure.Identity;
using FluentValidation.AspNetCore;
using LeagueOfStats.API.Configurations;
using LeagueOfStats.API.Configurations.Options;
using LeagueOfStats.API.Environments;
using LeagueOfStats.API.Infrastructure.ApiClients.CommunityDragonClient;
using LeagueOfStats.API.Infrastructure.ApiClients.DataDragonClient;
using LeagueOfStats.API.Infrastructure.ApiClients.RiotClient;
using LeagueOfStats.API.Infrastructure.ApiClients.RiotGamesShopClient;
using LeagueOfStats.Application.ApiClients.RiotClient;
using LeagueOfStats.Application.Common;
using LeagueOfStats.Application.Jobs;
using LeagueOfStats.Infrastructure.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using Quartz;
using Quartz.AspNetCore;
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

        services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();

        AddOptions(builder);

        services.AddSingleton<IEntityUpdateLockoutService, EntityUpdateLockoutService>();

        //services.AddScoped<ISyncChampionAndSkinService, SyncChampionAndSkinService>();
        
        AddExternalApiClients(services);
        
        AddQuartz(builder);
    }

    private static void AddExternalApiClients(IServiceCollection services)
    {
        services.AddScoped<IRiotClient, RiotClient>();
        services.ConfigureRiotGamesShopClient();
        services.ConfigureDataDragonClient();
        services.ConfigureCommunityDragonClient();
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
                ManagedIdentityClientId = builder
                    .Configuration
                    .GetSection(AppSettingsNameConstants.ManagedIdentityClientId).Value!,
            });
        var keyVaultUrl = new Uri(builder
            .Configuration
            .GetSection(AppSettingsNameConstants.KeyVaultURL).Value!);

        builder.Configuration.AddAzureKeyVault(keyVaultUrl, azureCredential);
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

    private static void AddQuartz(WebApplicationBuilder builder)
    {
        builder.Services.AddQuartz(q =>
        {
            q.UsePersistentStore(c =>
            {
                c.UseSqlServer(sqlServerOptions =>
                {
                    var dbConnectionString = builder.Configuration
                        .GetSection(nameof(DatabaseOptions))
                        .GetSection(nameof(DatabaseOptions.DatabaseConnectionString)).Value;
                    
                    var dbAdminPassword = builder.Configuration
                        .GetSection(nameof(DatabaseOptions))
                        .GetSection(nameof(DatabaseOptions.DatabaseAdminPassword)).Value;
                    
                    var connectionString = string.Format(dbConnectionString, dbAdminPassword);
                    
                    sqlServerOptions.ConnectionString = connectionString;
                });

                c.UseProperties = true;
                
                c.UseNewtonsoftJsonSerializer();
            });
            
            string syncChampionAndSkiNdataAfterPatchJobKey = nameof(SyncChampionAndSkinDataAfterPatchJob);
            q.AddJob<SyncChampionAndSkinDataAfterPatchJob>(opts => opts
                .StoreDurably()
                .WithIdentity(syncChampionAndSkiNdataAfterPatchJobKey));

            q.AddTrigger(opts => opts
                .ForJob(syncChampionAndSkiNdataAfterPatchJobKey)
                .WithIdentity($"{syncChampionAndSkiNdataAfterPatchJobKey}-trigger")
                .StartAt(new DateTimeOffset(2024, 3, 6, 6, 0, 0, TimeSpan.Zero))
                .WithSchedule(CronScheduleBuilder
                    .WeeklyOnDayAndHourAndMinute(DayOfWeek.Tuesday, 6, 0)));
            
        });
        
        builder.Services.AddQuartzServer(options =>
        {
            options.WaitForJobsToComplete = true;
        });
    }
}