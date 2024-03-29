using LeagueOfStats.Infrastructure.ApplicationDbContexts;
using LeagueOfStats.Infrastructure.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace LeagueOfStats.Integration.Tests.TestCommons;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            ServiceDescriptor? descriptior =
                services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (descriptior is not null)
            {
                services.Remove(descriptior);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(_dbContainer.GetConnectionString(), sqlServerOptionsAction =>
                {
                    sqlServerOptionsAction.UseNodaTime();
                });
                
            });

            services.Configure<DatabaseOptions>(opts =>
                opts.DatabaseConnectionString = _dbContainer.GetConnectionString());
        });
        
    }

    public Task InitializeAsync() => _dbContainer.StartAsync();

    public new Task DisposeAsync() => _dbContainer.StopAsync();
}