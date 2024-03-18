using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Matches.Participants;
using LeagueOfStats.Domain.Matches.Participants.Dtos;
using LeagueOfStats.Domain.Matches.Teams.Enums;
using LeagueOfStats.Domain.Summoners;
using Moq;
using NUnit.Framework;
using Match = LeagueOfStats.Domain.Matches.Match;

namespace LeagueOfStats.Domain.Tests.Matches.Participants;

[TestFixture]
public class ParticipantTests
{
    [Test]
    public void Constructor_AllValid_CreatesParticipantAndPerksWithProvidedData()
    {
        Match match = Mock.Of<Match>();

        const int defense = 1;
        const int flex = 2;
        const int offense = 3;
        AddPerkStatsDto addPerkStatsDto = new(
            defense,
            flex,
            offense);
        
        AddPerksDto addPerksDto = new(
            addPerkStatsDto,
            Enumerable.Empty<AddPerkStyleDto>());
        
        Guid championId = Guid.NewGuid();
        Guid summonerId = Guid.NewGuid();

        Champion champion = Mock.Of<Champion>(c => c.Id == championId);
        Summoner summoner = Mock.Of<Summoner>(s => s.Id == summonerId);
        const int assists = 1;
        const int champLevel = 2;
        const int damageDealtToBuildings = 3;
        const int damageDealtToObjectives = 4;
        const int damageDealtToTurrets = 5;
        const int damageSelfMitigated = 6;
        const int deaths = 7;
        const int detectorWardsPlaced = 8;
        const int doubleKills = 9;
        const bool firstBloodKill = true;
        const bool firstTowerKill = false;
        const bool gameEndedInEarlySurrender = true;
        const bool gameEndedInSurrender = true;
        const int goldEarned = 10;
        const int goldSpent = 11;
        const int item0 = 12;
        const int item1 = 13;
        const int item2 = 14;
        const int item3 = 15;
        const int item4 = 16;
        const int item5 = 17;
        const int item6 = 18;
        const int itemsPurchased = 19;
        const int killingSprees = 20;
        const int kills = 21;
        const int largestCriticalStrike = 22;
        const int largestKillingSpree = 23;
        const int largestMultiKill = 24;
        const int longestTimeSpentLiving = 25;
        const int magicDamageDealt = 26;
        const int magicDamageDealtToChampions = 26;
        const int magicDamageTaken = 70;
        const int neutralMinionsKilled = 71;
        const int nexusKills = 28;
        const int objectivesStolen = 29;
        const int pentaKills = 30;
        const int physicalDamageDealt = 31;
        const int physicalDamageDealtToChampions = 32;
        const int physicalDamageTaken = 33;
        const int placement = 34;
        const int playerAugment1 = 35;
        const int playerAugment2 = 36;
        const int playerAugment3 = 37;
        const int playerAugment4 = 38;
        const int playerSubteamId = 72;
        const int quadraKills = 39;
        const int spell1Casts = 40;
        const int spell2Casts = 41;
        const int spell3Casts = 42;
        const int spell4Casts = 43;
        const int subteamPlacement = 44;
        const int summoner1Casts = 45;
        const int summoner1Id = 46;
        const int summoner2Casts = 47;
        const int summoner2Id = 48;
        const bool teamEarlySurrendered = true;
        const Side side = Side.Blue;
        const string teamPosition = "teamPosition";
        const int timeCCingOthers = 49;
        const int timePlayed = 50;
        const int totalDamageDealt = 51;
        const int totalDamageDealtToChampions = 52;
        const int totalDamageShieldedOnTeammates = 53;
        const int totalDamageTaken = 54;
        const int totalHeal = 55;
        const int totalHealsOnTeammates = 56;
        const int totalMinionsKilled = 57;
        const int totalTimeCCDealt = 58;
        const int totalTimeSpentDead = 59;
        const int totalUnitsHealed = 60;
        const int tripleKills = 61;
        const int trueDamageDealt = 62;
        const int trueDamageDealtToChampions = 63;
        const int trueDamageTaken = 64;
        const int turretKills = 65;
        const int turretsLost = 73;
        const int turretTakedowns = 74;
        const int visionScore = 66;
        const int visionWardsBoughtInGame = 67;
        const int wardsKilled = 68;
        const int wardsPlaced = 69;
        const bool win = true;

        AddParticipantDto addParticipantDto = new(
            champion,
            summoner,
            addPerksDto,
            assists,
            champLevel,
            damageDealtToBuildings,
            damageDealtToObjectives,
            damageDealtToTurrets,
            damageSelfMitigated,
            deaths,
            detectorWardsPlaced,
            doubleKills,
            firstBloodKill,
            firstTowerKill,
            gameEndedInEarlySurrender,
            gameEndedInSurrender,
            goldEarned,
            goldSpent,
            item0,
            item1,
            item2,
            item3,
            item4,
            item5,
            item6,
            itemsPurchased,
            killingSprees,
            kills,
            largestCriticalStrike,
            largestKillingSpree,
            largestMultiKill,
            longestTimeSpentLiving,
            magicDamageDealt,
            magicDamageDealtToChampions,
            magicDamageTaken,
            neutralMinionsKilled,
            nexusKills,
            objectivesStolen,
            pentaKills,
            physicalDamageDealt,
            physicalDamageDealtToChampions,
            physicalDamageTaken,
            placement,
            playerAugment1,
            playerAugment2,
            playerAugment3,
            playerAugment4,
            playerSubteamId,
            quadraKills,
            spell1Casts,
            spell2Casts,
            spell3Casts,
            spell4Casts,
            subteamPlacement,
            summoner1Casts,
            summoner1Id,
            summoner2Casts,
            summoner2Id,
            teamEarlySurrendered,
            side,
            teamPosition,
            timeCCingOthers,
            timePlayed,
            totalDamageDealt,
            totalDamageDealtToChampions,
            totalDamageShieldedOnTeammates,
            totalDamageTaken,
            totalHeal,
            totalHealsOnTeammates,
            totalMinionsKilled,
            totalTimeCCDealt,
            totalTimeSpentDead,
            totalUnitsHealed,
            tripleKills,
            trueDamageDealt,
            trueDamageDealtToChampions,
            trueDamageTaken,
            turretKills,
            turretsLost,
            turretTakedowns,
            visionScore,
            visionWardsBoughtInGame,
            wardsKilled,
            wardsPlaced,
            win);

        Participant participant = new(addParticipantDto, match);
        
        Assert.That(participant.Match, Is.EqualTo(match));
            Assert.That(participant.Assists, Is.EqualTo(assists));
            Assert.That(participant.ChampLevel, Is.EqualTo(champLevel));
            Assert.That(participant.DamageDealtToBuildings, Is.EqualTo(damageDealtToBuildings));
            Assert.That(participant.DamageDealtToObjectives, Is.EqualTo(damageDealtToObjectives));
            Assert.That(participant.DamageDealtToTurrets, Is.EqualTo(damageDealtToTurrets));
            Assert.That(participant.DamageSelfMitigated, Is.EqualTo(damageSelfMitigated));
            Assert.That(participant.Deaths, Is.EqualTo(deaths));
            Assert.That(participant.DetectorWardsPlaced, Is.EqualTo(detectorWardsPlaced));
            Assert.That(participant.DoubleKills, Is.EqualTo(doubleKills));
            Assert.That(participant.FirstBloodKill, Is.EqualTo(firstBloodKill));
            Assert.That(participant.FirstTowerKill, Is.EqualTo(firstTowerKill));
            Assert.That(participant.GameEndedInEarlySurrender, Is.EqualTo(gameEndedInEarlySurrender));
            Assert.That(participant.GameEndedInSurrender, Is.EqualTo(gameEndedInSurrender));
            Assert.That(participant.GoldEarned, Is.EqualTo(goldEarned));
            Assert.That(participant.GoldSpent, Is.EqualTo(goldSpent));
            Assert.That(participant.Item0, Is.EqualTo(item0));
            Assert.That(participant.Item1, Is.EqualTo(item1));
            Assert.That(participant.Item2, Is.EqualTo(item2));
            Assert.That(participant.Item3, Is.EqualTo(item3));
            Assert.That(participant.Item4, Is.EqualTo(item4));
            Assert.That(participant.Item5, Is.EqualTo(item5));
            Assert.That(participant.Item6, Is.EqualTo(item6));
            Assert.That(participant.ItemsPurchased, Is.EqualTo(itemsPurchased));
            Assert.That(participant.KillingSprees, Is.EqualTo(killingSprees));
            Assert.That(participant.Kills, Is.EqualTo(kills));
            Assert.That(participant.LargestCriticalStrike, Is.EqualTo(largestCriticalStrike));
            Assert.That(participant.LargestKillingSpree, Is.EqualTo(largestKillingSpree));
            Assert.That(participant.LargestMultiKill, Is.EqualTo(largestMultiKill));
            Assert.That(participant.LongestTimeSpentLiving, Is.EqualTo(longestTimeSpentLiving));
            Assert.That(participant.MagicDamageDealt, Is.EqualTo(magicDamageDealt));
            Assert.That(participant.MagicDamageDealtToChampions, Is.EqualTo(magicDamageDealtToChampions));
            Assert.That(participant.MagicDamageTaken, Is.EqualTo(magicDamageTaken));
            Assert.That(participant.NeutralMinionsKilled, Is.EqualTo(neutralMinionsKilled));
            Assert.That(participant.NexusKills, Is.EqualTo(nexusKills));
            Assert.That(participant.ObjectivesStolen, Is.EqualTo(objectivesStolen));
            Assert.That(participant.PentaKills, Is.EqualTo(pentaKills));
            Assert.That(participant.PhysicalDamageDealt, Is.EqualTo(physicalDamageDealt));
            Assert.That(participant.PhysicalDamageDealtToChampions, Is.EqualTo(physicalDamageDealtToChampions));
            Assert.That(participant.PhysicalDamageTaken, Is.EqualTo(physicalDamageTaken));
            Assert.That(participant.Placement, Is.EqualTo(placement));
            Assert.That(participant.PlayerAugment1, Is.EqualTo(playerAugment1));
            Assert.That(participant.PlayerAugment2, Is.EqualTo(playerAugment2));
            Assert.That(participant.PlayerAugment3, Is.EqualTo(playerAugment3));
            Assert.That(participant.PlayerAugment4, Is.EqualTo(playerAugment4));
            Assert.That(participant.PlayerSubteamId, Is.EqualTo(playerSubteamId));
            Assert.That(participant.QuadraKills, Is.EqualTo(quadraKills));
            Assert.That(participant.Spell1Casts, Is.EqualTo(spell1Casts));
            Assert.That(participant.Spell2Casts, Is.EqualTo(spell2Casts));
            Assert.That(participant.Spell3Casts, Is.EqualTo(spell3Casts));
            Assert.That(participant.Spell4Casts, Is.EqualTo(spell4Casts));
            Assert.That(participant.SubteamPlacement, Is.EqualTo(subteamPlacement));
            Assert.That(participant.Summoner1Casts, Is.EqualTo(summoner1Casts));
            Assert.That(participant.Summoner1Id, Is.EqualTo(summoner1Id));
            Assert.That(participant.Summoner2Casts, Is.EqualTo(summoner2Casts));
            Assert.That(participant.Summoner2Id, Is.EqualTo(summoner2Id));
            Assert.That(participant.TeamEarlySurrendered, Is.EqualTo(teamEarlySurrendered));
            Assert.That(participant.Side, Is.EqualTo(side));
            Assert.That(participant.TeamPosition, Is.EqualTo(teamPosition));
            Assert.That(participant.TimeCCingOthers, Is.EqualTo(timeCCingOthers));
            Assert.That(participant.TimePlayed, Is.EqualTo(timePlayed));
            Assert.That(participant.TotalDamageDealt, Is.EqualTo(totalDamageDealt));
            Assert.That(participant.TotalDamageDealtToChampions, Is.EqualTo(totalDamageDealtToChampions));
            Assert.That(participant.TotalDamageShieldedOnTeammates, Is.EqualTo(totalDamageShieldedOnTeammates));
            Assert.That(participant.TotalDamageTaken, Is.EqualTo(totalDamageTaken));
            Assert.That(participant.TotalHeal, Is.EqualTo(totalHeal));
            Assert.That(participant.TotalHealsOnTeammates, Is.EqualTo(totalHealsOnTeammates));
            Assert.That(participant.TotalMinionsKilled, Is.EqualTo(totalMinionsKilled));
            Assert.That(participant.TotalTimeCCDealt, Is.EqualTo(totalTimeCCDealt));
            Assert.That(participant.TotalTimeSpentDead, Is.EqualTo(totalTimeSpentDead));
            Assert.That(participant.TotalUnitsHealed, Is.EqualTo(totalUnitsHealed));
            Assert.That(participant.TripleKills, Is.EqualTo(tripleKills));
            Assert.That(participant.TrueDamageDealt, Is.EqualTo(trueDamageDealt));
            Assert.That(participant.TrueDamageDealtToChampions, Is.EqualTo(trueDamageDealtToChampions));
            Assert.That(participant.TrueDamageTaken, Is.EqualTo(trueDamageTaken));
            Assert.That(participant.TurretKills, Is.EqualTo(turretKills));
            Assert.That(participant.TurretsLost, Is.EqualTo(turretsLost));
            Assert.That(participant.TurretTakedowns, Is.EqualTo(turretTakedowns));
            Assert.That(participant.VisionScore, Is.EqualTo(visionScore));
            Assert.That(participant.VisionWardsBoughtInGame, Is.EqualTo(visionWardsBoughtInGame));
            Assert.That(participant.WardsKilled, Is.EqualTo(wardsKilled));
            Assert.That(participant.WardsPlaced, Is.EqualTo(wardsPlaced));
            Assert.That(participant.Win, Is.EqualTo(win));
            Assert.That(participant.Perks.Styles.Count(), Is.EqualTo(0));
            Assert.That(participant.Perks.StatPerks.Defense, Is.EqualTo(defense));
            Assert.That(participant.Perks.StatPerks.Flex, Is.EqualTo(flex));
            Assert.That(participant.Perks.StatPerks.Offense, Is.EqualTo(offense));
            Assert.That(participant.Perks.StatPerks.Perks, Is.EqualTo(participant.Perks));
    }
}