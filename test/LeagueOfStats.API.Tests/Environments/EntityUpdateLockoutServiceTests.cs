using LeagueOfStats.API.Configurations.Options;
using LeagueOfStats.API.Environments;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace LeagueOfStats.API.Tests.Environments;

[TestFixture]
public class EntityUpdateLockoutServiceTests
{
    private readonly Mock<IOptions<EntityUpdateLockoutOptions>> _entityUpdateLockoutOptionsMock = new();
    
    [Test]
    public void GetSummonerUpdateLockoutInMinutes_AllValid_ReturnsLockoutInMinutesFromConfig()
    {
        const int entityLockoutOptionValue = 10;
        EntityUpdateLockoutOptions entityUpdateLockoutOptions = new EntityUpdateLockoutOptions
        {
            SummonerUpdateLockout = entityLockoutOptionValue
        };
        _entityUpdateLockoutOptionsMock
            .Setup(x => x.Value)
            .Returns(entityUpdateLockoutOptions);

        EntityUpdateLockoutService entityUpdateLockoutService = new(_entityUpdateLockoutOptionsMock.Object);

        int summonerUpdateLockoutInMinutes = entityUpdateLockoutService.GetSummonerUpdateLockoutInMinutes();
        
        Assert.That(summonerUpdateLockoutInMinutes, Is.EqualTo(entityLockoutOptionValue));
    }
}