using LeagueOfStats.Infrastructure.ApplicationDbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LeagueOfStats.Integration.Tests.Common;

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
        ApplicationDbContext.Database.Migrate();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        _scope.Dispose();
        ApplicationDbContext.Dispose();
        await _factory.DisposeAsync();
    }
}