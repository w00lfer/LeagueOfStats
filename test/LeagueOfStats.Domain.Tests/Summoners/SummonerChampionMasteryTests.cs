using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Summoners;
using Moq;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Summoners;

[TestFixture]
public class SummonerChampionMasteryTests
{
    [Test]
    public void Constructor_AllValid_CreatesSummonerChampionMasteriesWithProvidedData()
    {
        Summoner summoner = Mock.Of<Summoner>();
        Guid championId = Guid.NewGuid();
        Champion champion = Mock.Of<Champion>(c => c.Id == championId);
        const int championLevel = 5;
        const int championPoints = 100000;
        const long championPointsSinceLastLevel = 10000;
        const long championPointsUntilNextLevel = 0;
        const bool chestGranted = true;
        const long lastPlayTime = 10;
        const int tokensEarned = 2;

        SummonerChampionMastery summonerChampionMastery = new SummonerChampionMastery(
            summoner,
            champion,
            championLevel,
            championPoints,
            championPointsSinceLastLevel,
            championPointsUntilNextLevel,
            chestGranted,
            lastPlayTime,
            tokensEarned);
        
        Assert.That(summonerChampionMastery.Summoner, Is.EqualTo(summoner));
        Assert.That(summonerChampionMastery.ChampionId, Is.EqualTo(championId));
        Assert.That(summonerChampionMastery.ChampionLevel, Is.EqualTo(championLevel));
        Assert.That(summonerChampionMastery.ChampionPoints, Is.EqualTo(championPoints));
        Assert.That(summonerChampionMastery.ChampionPointsSinceLastLevel, Is.EqualTo(championPointsSinceLastLevel));
        Assert.That(summonerChampionMastery.ChampionPointsUntilNextLevel, Is.EqualTo(championPointsUntilNextLevel));
        Assert.That(summonerChampionMastery.ChestGranted, Is.EqualTo(chestGranted));
        Assert.That(summonerChampionMastery.LastPlayTime, Is.EqualTo(lastPlayTime));
        Assert.That(summonerChampionMastery.TokensEarned, Is.EqualTo(tokensEarned));
    }
    
    [Test]
    public void Update_AllValid_UpdateSummonerChampionMasteriesWithProvidedData()
    {
        Summoner summoner = Mock.Of<Summoner>();
        Guid championId = Guid.NewGuid();
        Champion champion = Mock.Of<Champion>(c => c.Id == championId);

        SummonerChampionMastery summonerChampionMastery = new SummonerChampionMastery(
            summoner,
            champion,
            2,
            100,
            50,
            200,
            false,
            5,
            0);
        
        const int championLevel = 5;
        const int championPoints = 100000;
        const long championPointsSinceLastLevel = 10000;
        const long championPointsUntilNextLevel = 0;
        const bool chestGranted = true;
        const long lastPlayTime = 10;
        const int tokensEarned = 2;
        
        summonerChampionMastery.Update(
            championLevel,
            championPoints,
            championPointsSinceLastLevel,
            championPointsUntilNextLevel,
            chestGranted,
            lastPlayTime,
            tokensEarned);
        
        Assert.That(summonerChampionMastery.Summoner, Is.EqualTo(summoner));
        Assert.That(summonerChampionMastery.ChampionId, Is.EqualTo(championId));
        Assert.That(summonerChampionMastery.ChampionLevel, Is.EqualTo(championLevel));
        Assert.That(summonerChampionMastery.ChampionPoints, Is.EqualTo(championPoints));
        Assert.That(summonerChampionMastery.ChampionPointsSinceLastLevel, Is.EqualTo(championPointsSinceLastLevel));
        Assert.That(summonerChampionMastery.ChampionPointsUntilNextLevel, Is.EqualTo(championPointsUntilNextLevel));
        Assert.That(summonerChampionMastery.ChestGranted, Is.EqualTo(chestGranted));
        Assert.That(summonerChampionMastery.LastPlayTime, Is.EqualTo(lastPlayTime));
        Assert.That(summonerChampionMastery.TokensEarned, Is.EqualTo(tokensEarned));
    }
}