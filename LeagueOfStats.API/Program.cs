using LeagueOfStats.API.Infrastructure.RiotClient;
using LeagueOfStats.Application.RiotClient;
using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Persistence.Champions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRiotClient, RiotClient>();

builder.Services.AddSingleton<IChampionRepository, ChampionRepository>();

// TODO MOVE IT AND REGISTER PROPERLY IN APP LAYER
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IRiotClient>());

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