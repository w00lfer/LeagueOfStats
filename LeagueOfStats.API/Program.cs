using LeagueOfStats.API;
using LeagueOfStats.Application;
using LeagueOfStats.Domain;
using LeagueOfStats.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiDI(builder);
builder.Services.AddApplicationDI(builder.Configuration);
builder.Services.AddInfrastructureDI();
builder.Services.AddDomainDI(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();