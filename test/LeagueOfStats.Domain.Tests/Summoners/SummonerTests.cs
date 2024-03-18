using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Enums;
using LeagueOfStats.Domain.Summoners;
using LeagueOfStats.Domain.Summoners.Dtos;
using Moq;
using NodaTime;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Summoners;

[TestFixture]
public class SummonerTests
{
    [Test]
    public void Constructor_AllValid_CreatesSummonerWithSummonerNameAndSummonerChampionMasteriesWithProvidedData()
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
        
        Assert.That(summoner.SummonerId, Is.EqualTo(summonerId));
        Assert.That(summoner.AccountId, Is.EqualTo(accountId));
        Assert.That(summoner.SummonerName, Is.EqualTo(SummonerName.Create(gameName, tagLine)));
        Assert.That(summoner.Name, Is.EqualTo(name));
        Assert.That(summoner.ProfileIconId, Is.EqualTo(profileIconId));
        Assert.That(summoner.Puuid, Is.EqualTo(puuid));
        Assert.That(summoner.SummonerLevel, Is.EqualTo(summonerLevel));
        Assert.That(summoner.Region, Is.EqualTo(region));
        Assert.That(summoner.LastUpdated, Is.EqualTo(lastUpdated));
        
        Assert.That(summoner.SummonerChampionMasteries.Count, Is.EqualTo(1));
        SummonerChampionMastery summonerChampionMastery = summoner.SummonerChampionMasteries.Single();
        
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
    public void Update_AlLValid_UpdateBasicDataAndSummonerChampionMasteries()
    {
        Guid championId1 = Guid.NewGuid();
        Champion champion1 = Mock.Of<Champion>(c => c.Id == championId1);
        UpdateChampionMasteryDto updateChampionMasteryDto = new(
            champion1,
            5,
            50000,
            10000,
            0,
            false,
            10,
            1);
        List<UpdateChampionMasteryDto> updateChampionMasteryDtos = new()
        {
            updateChampionMasteryDto
        };

        Summoner summoner = new Summoner(
            "summonerId",
            "accountId",
            "name",
            25,
            "puuid",
            500,
            "gameName",
            "tagLine",
            Region.EUNE,
            updateChampionMasteryDtos,
            Instant.MinValue);
        
        const int newChampionLevel1 = 5;
        const int newChampionPoints1 = 100000;
        const long newChampionPointsSinceLastLevel1 = 10000;
        const long newChampionPointsUntilNextLevel1 = 0;
        const bool newChestGranted1 = true;
        const long newLastPlayTime1 = 10;
        const int newTokensEarned1 = 2;
        UpdateChampionMasteryDto updateChampionMasteryDtoForExistingChampionMastery = new(
            champion1,
            newChampionLevel1,
            newChampionPoints1,
            newChampionPointsSinceLastLevel1,
            newChampionPointsUntilNextLevel1,
            newChestGranted1,
            newLastPlayTime1,
            newTokensEarned1);
        
        Guid championId2 = Guid.NewGuid();
        Champion champion2 = Mock.Of<Champion>(c => c.Id == championId2);
        const int newChampionLevel2 = 5;
        const int newChampionPoints2 = 100000;
        const long newChampionPointsSinceLastLevel2 = 10000;
        const long newChampionPointsUntilNextLevel2 = 0;
        const bool newChestGranted2 = true;
        const long newLastPlayTime2 = 10;
        const int newTokensEarned2 = 2;
        UpdateChampionMasteryDto updateChampionMasteryDtoForNewChampionMastery = new(
            champion2,
            newChampionLevel2,
            newChampionPoints2,
            newChampionPointsSinceLastLevel2,
            newChampionPointsUntilNextLevel2,
            newChestGranted2,
            newLastPlayTime2,
            newTokensEarned2);
        List<UpdateChampionMasteryDto> updateChampionMasteryDtosForUpdate = new()
        {
            updateChampionMasteryDtoForNewChampionMastery,
            updateChampionMasteryDtoForExistingChampionMastery
        };
        
        const int newProfileIconId = 100;
        const long newSummonerLevel = 600;
        Instant newLastUpdated = Instant.MaxValue;
        
        summoner.Update(
            newProfileIconId,
            newSummonerLevel,
            updateChampionMasteryDtosForUpdate,
            newLastUpdated);
        
        Assert.That(summoner.ProfileIconId, Is.EqualTo(newProfileIconId));
        Assert.That(summoner.SummonerLevel, Is.EqualTo(newSummonerLevel));
        Assert.That(summoner.LastUpdated, Is.EqualTo(newLastUpdated));
        
        Assert.That(summoner.SummonerChampionMasteries.Count, Is.EqualTo(2));
        SummonerChampionMastery summonerChampionMastery = summoner.SummonerChampionMasteries.ElementAt(0);
        
        Assert.That(summonerChampionMastery.Summoner, Is.EqualTo(summoner));
        Assert.That(summonerChampionMastery.ChampionId, Is.EqualTo(championId1));
        Assert.That(summonerChampionMastery.ChampionLevel, Is.EqualTo(newChampionLevel1));
        Assert.That(summonerChampionMastery.ChampionPoints, Is.EqualTo(newChampionPoints1));
        Assert.That(summonerChampionMastery.ChampionPointsSinceLastLevel, Is.EqualTo(newChampionPointsSinceLastLevel1));
        Assert.That(summonerChampionMastery.ChampionPointsUntilNextLevel, Is.EqualTo(newChampionPointsUntilNextLevel1));
        Assert.That(summonerChampionMastery.ChestGranted, Is.EqualTo(newChestGranted1));
        Assert.That(summonerChampionMastery.LastPlayTime, Is.EqualTo(newLastPlayTime1));
        Assert.That(summonerChampionMastery.TokensEarned, Is.EqualTo(newTokensEarned1));
        
        summonerChampionMastery = summoner.SummonerChampionMasteries.ElementAt(1);
        
        Assert.That(summonerChampionMastery.Summoner, Is.EqualTo(summoner));
        Assert.That(summonerChampionMastery.ChampionId, Is.EqualTo(championId2));
        Assert.That(summonerChampionMastery.ChampionLevel, Is.EqualTo(newChampionLevel2));
        Assert.That(summonerChampionMastery.ChampionPoints, Is.EqualTo(newChampionPoints2));
        Assert.That(summonerChampionMastery.ChampionPointsSinceLastLevel, Is.EqualTo(newChampionPointsSinceLastLevel2));
        Assert.That(summonerChampionMastery.ChampionPointsUntilNextLevel, Is.EqualTo(newChampionPointsUntilNextLevel2));
        Assert.That(summonerChampionMastery.ChestGranted, Is.EqualTo(newChestGranted2));
        Assert.That(summonerChampionMastery.LastPlayTime, Is.EqualTo(newLastPlayTime2));
        Assert.That(summonerChampionMastery.TokensEarned, Is.EqualTo(newTokensEarned2));
    }
}