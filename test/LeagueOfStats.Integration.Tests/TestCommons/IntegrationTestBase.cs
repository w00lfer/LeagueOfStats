using AutoBogus;
using Bogus;
using LeagueOfStats.Infrastructure.ApplicationDbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LeagueOfStats.Integration.Tests.TestCommons;

[TestFixture]
public abstract class IntegrationTestBase
{
    private readonly IntegrationTestWebAppFactory _factory = new();
    private IServiceScope _scope;
    
    protected ApplicationDbContext ApplicationDbContext;
    protected readonly Faker Faker = new();
    
    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        await _factory.InitializeAsync();
        _scope = _factory.Services.CreateScope();
        
        ApplicationDbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        ApplicationDbContext.Database.Migrate();
    }

    [SetUp]
    public async Task ClearDatabaseBeforeTest()
    {
        await ClearTables();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        _scope.Dispose();
        await ApplicationDbContext.DisposeAsync();
        await _factory.DisposeAsync();
    }

    private async Task ClearTables()
    {
        await ApplicationDbContext.Database.ExecuteSqlRawAsync(
            "EXEC sp_MSforeachtable N'ALTER TABLE ? NOCHECK CONSTRAINT ALL;'EXEC sp_MSforeachtable N'DELETE FROM ?;'EXEC sp_MSforeachtable N'ALTER TABLE ? CHECK CONSTRAINT ALL;'");
    }
}