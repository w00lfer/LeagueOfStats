using LeagueOfStats.API.Environments;
using LeagueOfStats.API.Infrastructure.RiotClient;
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

builder.Services.Configure<EntityUpdateLockoutOptions>(
    builder.Configuration.GetSection(nameof(EntityUpdateLockoutOptions)));

builder.Services.AddSingleton<IEntityUpdateLockoutService, EntityUpdateLockoutService>();
builder.Services.AddScoped<IRiotClient, RiotClient>();

builder.Services.AddApplicationDI(builder.Configuration);
builder.Services.AddInfrastructureDI(builder.Configuration);
builder.Services.AddDomainDI(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();