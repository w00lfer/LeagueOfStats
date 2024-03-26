using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Enums;
using LeagueOfStats.Domain.Summoners;
using LeagueOfStats.Domain.Summoners.Dtos;
using LeagueOfStats.Infrastructure.ApplicationDbContexts;
using LeagueOfStats.Infrastructure.Summoners;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NodaTime;
using NUnit.Framework;

namespace LeagueOfStats.Integration.Tests.Summoners;

[TestFixture]
public class SummonerRepositoryTests
{
        private const string Database = "master";
        private const string Username = "sa";
        private const string Password = "$trongPassword";
        private const ushort MsSqlPort = 1433;

        private WebApplicationFactory<Program> _factory;
        private IContainer _container;
        private ApplicationDbContext _applicationDbContext;
    
        private SummonerRepository _summonerRepository;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            // Set up Testcontainers SQL Server container
            _container = new ContainerBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithPortBinding(MsSqlPort, true)
                .WithEnvironment("ACCEPT_EULA", "Y")
                .WithEnvironment("SQLCMDUSER", Username)
                .WithEnvironment("SQLCMDPASSWORD", Password)
                .WithEnvironment("MSSQL_SA_PASSWORD", Password)
                .WithResourceMapping(Array.Empty<byte>(), "/var/lib/mysql-files/gh-issue-1142")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(MsSqlPort))
                .Build();

            //Start Container
            await _container.StartAsync();

            var host = _container.Hostname;
            var port = _container.GetMappedPublicPort(MsSqlPort);

            // Replace connection string in DbContext
            var connectionString = $"Server={host},{port};Database={Database};User Id={Username};Password={Password};TrustServerCertificate=True";
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddDbContext<ApplicationDbContext>(options =>
                            options.UseSqlServer(connectionString));
                    });
                });

            // Initialize database
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();

            _applicationDbContext = dbContext;
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await _container.StopAsync();
            await _container.DisposeAsync();
            _factory.Dispose();
        }

    [SetUp]
    public void SetUp()
    {
        _summonerRepository = new SummonerRepository(_applicationDbContext);
    }
    
    [Test]
    public async Task s()
    {
        const string summonerId = "summonerId";
        const string accountId = "accountId";
        const string name = "name";
        const int profileIconId = 1000;
        const string puuid = "puuid";
        const long summonerLevel = 500;
        const string gameName = "gameName";
        const string tagLine = "tagLine";
        const Region region = Region.EUNE;
        Instant lastUpdated = Instant.MaxValue;

        Guid championId = Guid.NewGuid();
        Champion champion = Mock.Of<Champion>(c => c.Id == championId);
        const int championLevel = 5;
        const int championPoints = 100000;
        const long championPointsSinceLastLevel = 10000;
        const long championPointsUntilNextLevel = 0;
        const bool chestGranted = true;
        const long lastPlayTime = 10;
        const int tokensEarned = 2;
        UpdateChampionMasteryDto updateChampionMasteryDto = new(
            champion,
            championLevel,
            championPoints,
            championPointsSinceLastLevel,
            championPointsUntilNextLevel,
            chestGranted, lastPlayTime,
            tokensEarned);
        List<UpdateChampionMasteryDto> updateChampionMasteryDtos = new()
        {
            updateChampionMasteryDto
        };
        
        Summoner summoner = new Summoner(
            summonerId,
            accountId,
            name,
            profileIconId,
            puuid,
            summonerLevel,
            gameName,
            tagLine,
            region,
            updateChampionMasteryDtos,
            lastUpdated);

        await _summonerRepository.AddAsync(summoner);

        IEnumerable<Summoner> summonersInDb = await _applicationDbContext.Summoners.ToListAsync();
        
        Assert.That(summonersInDb.Count(), Is.EqualTo(1));
        Assert.That(summonersInDb.Single(), Is.EqualTo(summoner));
    }
}