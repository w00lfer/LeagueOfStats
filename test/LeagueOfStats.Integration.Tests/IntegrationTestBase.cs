using LeagueOfStats.Infrastructure.ApplicationDbContexts;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LeagueOfStats.Integration.Tests;

[TestFixture]
public abstract class IntegrationTestBase
{
    private IntegrationTestWebAppFactory _factory;
    private IServiceScope _scope;
    protected ApplicationDbContext ApplicationDbContext;
    
    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        _factory = new();
        await _factory.InitializeAsync();
        _scope = _factory.Services.CreateScope();
        
        ApplicationDbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _scope.Dispose();
        ApplicationDbContext.Dispose();
    }
}