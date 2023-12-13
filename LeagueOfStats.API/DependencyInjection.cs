using Azure.Identity;
using LeagueOfStats.API.Environments;
using LeagueOfStats.API.Infrastructure.RiotClient;
using LeagueOfStats.API.Options;
using LeagueOfStats.Application.Common;
using LeagueOfStats.Application.RiotClient;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

namespace LeagueOfStats.API;

public static class DependencyInjection
{
    public static void AddApiDI(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddControllers();
        
        AddSwagger(builder);
        
        AddKeyVault(services, builder);

        services.Configure<EntityUpdateLockoutOptions>(builder.Configuration.GetSection(nameof(EntityUpdateLockoutOptions)));
        services.Configure<RiotApiKeyOptions>(builder.Configuration.GetSection(nameof(RiotApiKeyOptions)));

        services.AddSingleton<IEntityUpdateLockoutService, EntityUpdateLockoutService>();
        services.AddScoped<IRiotClient, RiotClient>();
    }

    private static void AddSwagger(WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddEnumsWithValuesFixFilters();
        });
    }

    private static void AddKeyVault(IServiceCollection services, WebApplicationBuilder builder)
    {
        var azureCredential = new DefaultAzureCredential(
            new DefaultAzureCredentialOptions
            {
                ManagedIdentityClientId = builder.Configuration.GetSection("ManagedIdentityClientId").Value!,
            });
        var keyVaultUrl = new Uri((builder.Configuration.GetSection("KeyVaultURL").Value!));
        
        builder.Configuration.AddAzureKeyVault(keyVaultUrl, azureCredential);
    }
}