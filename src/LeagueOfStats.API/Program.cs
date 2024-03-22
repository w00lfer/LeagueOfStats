using LeagueOfStats.API;
using LeagueOfStats.Application;
using LeagueOfStats.Domain;
using LeagueOfStats.Infrastructure;
using LeagueOfStats.Jobs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiDI(builder);
builder.Services.AddApplicationDI();
builder.Services.AddInfrastructureDI();
builder.Services.AddDomainDI();
builder.Services.AddJobsDI(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program { }
#pragma warning restore CA1050 // Declare types in namespaces