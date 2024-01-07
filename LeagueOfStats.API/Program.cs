using LeagueOfStats.API;
using LeagueOfStats.Application;
using LeagueOfStats.Domain;
using LeagueOfStats.Infrastructure;
using LeagueOfStats.Infrastructure.ApplicationDbContexts;
using LeagueOfStats.Infrastructure.ApplicationDbContexts.Seeds;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiDI(builder);
builder.Services.AddApplicationDI();
builder.Services.AddInfrastructureDI();
builder.Services.AddDomainDI();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var applicationDbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
    await ApplicationDbContextSeed.SeedDataAsync(applicationDbContext);
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();