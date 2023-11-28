using LeagueOfStats.API.Infrastructure.RiotClient;
using LeagueOfStats.Application.RiotClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRiotClient, RiotClient>();

LeagueOfStats.Application.DependencyInjection.AddApplicationDI(builder.Services, builder.Configuration);
LeagueOfStats.Infrastructure.DependencyInjection.AddInfrastructureDI(builder.Services, builder.Configuration);
LeagueOfStats.Domain.DependencyInjection.AddDomainDI(builder.Services, builder.Configuration);

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