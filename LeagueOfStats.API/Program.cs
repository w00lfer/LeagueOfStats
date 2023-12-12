using Azure.Identity;
using LeagueOfStats.API.Environments;
using LeagueOfStats.API.Infrastructure.RiotClient;
using LeagueOfStats.API.Options;
using LeagueOfStats.Application;
using LeagueOfStats.Application.Common;
using LeagueOfStats.Application.RiotClient;
using LeagueOfStats.Domain;
using LeagueOfStats.Infrastructure;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddEnumsWithValuesFixFilters();
});

var keyVaultUrl = new Uri(builder.Configuration.GetSection("KeyVaultURL").Value!);
var azureCredential = new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = "0ce168ae-979f-4f50-97f6-523285b0a37e"});
builder.Configuration.AddAzureKeyVault(keyVaultUrl, azureCredential);

builder.Services.Configure<EntityUpdateLockoutOptions>(
    builder.Configuration.GetSection(nameof(EntityUpdateLockoutOptions)));
builder.Services.Configure<RiotApiKeyOptions>(
    builder.Configuration.GetSection(nameof(RiotApiKeyOptions)));

builder.Services.AddSingleton<IEntityUpdateLockoutService, EntityUpdateLockoutService>();
builder.Services.AddScoped<IRiotClient, RiotClient>();

builder.Services.AddApplicationDI(builder.Configuration);
builder.Services.AddInfrastructureDI(builder.Configuration);
builder.Services.AddDomainDI(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();