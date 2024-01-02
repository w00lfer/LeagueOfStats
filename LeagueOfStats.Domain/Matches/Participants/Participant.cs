using LeagueOfStats.Domain.Common.Entities;
using LeagueOfStats.Domain.Matches.Participants.Dtos;
using LeagueOfStats.Domain.Matches.Teams.Enums;

namespace LeagueOfStats.Domain.Matches.Participants;

public class Participant : Entity
{
    public Participant(
        AddParticipantDto addParticipantDto) 
        : base(Guid.NewGuid())
    {
        ChampionId = addParticipantDto.Champion.Id;
        SummonerId = addParticipantDto.Summoner.Id;
        Assists = addParticipantDto.Assists;
        ChampLevel = addParticipantDto.ChampLevel;
        DamageDealtToBuildings = addParticipantDto.DamageDealtToBuildings;
        DamageDealtToObjectives = addParticipantDto.DamageDealtToObjectives;
        DamageDealtToTurrets = addParticipantDto.DamageDealtToTurrets;
        DamageSelfMitigated = addParticipantDto.DamageSelfMitigated;
        Deaths = addParticipantDto.Deaths;
        DetectorWardsPlaced = addParticipantDto.DetectorWardsPlaced;
        DoubleKills = addParticipantDto.DoubleKills;
        FirstBloodKill = addParticipantDto.FirstBloodKill;
        FirstTowerKill = addParticipantDto.FirstTowerKill;
        GameEndedInEarlySurrender = addParticipantDto.GameEndedInEarlySurrender;
        GameEndedInSurrender = addParticipantDto.GameEndedInSurrender;
        GoldEarned = addParticipantDto.GoldEarned;
        GoldSpent = addParticipantDto.GoldSpent;
        Item0 = addParticipantDto.Item0;
        Item1 = addParticipantDto.Item1;
        Item2 = addParticipantDto.Item2;
        Item3 = addParticipantDto.Item3;
        Item4 = addParticipantDto.Item4;
        Item5 = addParticipantDto.Item5;
        Item6 = addParticipantDto.Item6;
        ItemsPurchased = addParticipantDto.ItemsPurchased;
        KillingSprees = addParticipantDto.KillingSprees;
        Kills = addParticipantDto.Kills;
        LargestCriticalStrike = addParticipantDto.LargestCriticalStrike;
        LargestKillingSpree = addParticipantDto.LargestKillingSpree;
        LargestMultiKill = addParticipantDto.LargestMultiKill;
        LongestTimeSpentLiving = addParticipantDto.LongestTimeSpentLiving;
        MagicDamageDealt = addParticipantDto.MagicDamageDealt;
        MagicDamageDealtToChampions = addParticipantDto.MagicDamageDealtToChampions;
        MagicDamageTaken =addParticipantDto.MagicDamageTaken;
        NeutralMinionsKilled = addParticipantDto.NeutralMinionsKilled;
        NexusKills = addParticipantDto.NexusKills;
        ObjectivesStolen = addParticipantDto.ObjectivesStolen;
        PentaKills = addParticipantDto.PentaKills;
        Perks = new Perks(addParticipantDto.AddPerksDto);
        PhysicalDamageDealt = addParticipantDto.PhysicalDamageDealt;
        PhysicalDamageDealtToChampions = addParticipantDto.PhysicalDamageDealtToChampions;
        PhysicalDamageTaken = addParticipantDto.PhysicalDamageTaken;
        Placement = addParticipantDto.Placement;
        PlayerAugment1 = addParticipantDto.PlayerAugment1;
        PlayerAugment2 = addParticipantDto.PlayerAugment2;
        PlayerAugment3 = addParticipantDto.PlayerAugment3;
        PlayerAugment4 = addParticipantDto.PlayerAugment4;
        PlayerSubteamId = addParticipantDto.PlayerSubteamId;
        QuadraKills = addParticipantDto.QuadraKills;
        Spell1Casts = addParticipantDto.Spell1Casts;
        Spell2Casts = addParticipantDto.Spell2Casts;
        Spell3Casts = addParticipantDto.Spell3Casts;
        Spell4Casts = addParticipantDto.Spell4Casts;
        SubteamPlacement = addParticipantDto.SubteamPlacement;
        Summoner1Casts = addParticipantDto.Summoner1Casts;
        Summoner1Id = addParticipantDto.Summoner1Id;
        Summoner2Casts = addParticipantDto.Summoner2Casts;
        Summoner2Id = addParticipantDto.Summoner2Id;
        TeamEarlySurrendered = addParticipantDto.TeamEarlySurrendered;
        Side = addParticipantDto.Side;
        TeamPosition = addParticipantDto.TeamPosition;
        TimeCCingOthers = addParticipantDto.TimeCCingOthers;
        TimePlayed = addParticipantDto.TimePlayed;
        TotalDamageDealt = addParticipantDto.TotalDamageDealt;
        TotalDamageDealtToChampions = addParticipantDto.TotalDamageDealtToChampions;
        TotalDamageShieldedOnTeammates = addParticipantDto.TotalDamageShieldedOnTeammates;
        TotalDamageTaken = addParticipantDto.TotalDamageTaken;
        TotalHeal = addParticipantDto.TotalHeal;
        TotalHealsOnTeammates = addParticipantDto.TotalHealsOnTeammates;
        TotalMinionsKilled = addParticipantDto.TotalMinionsKilled;
        TotalTimeCCDealt = addParticipantDto.TotalTimeCcDealt;
        TotalTimeSpentDead = addParticipantDto.TotalTimeSpentDead;
        TotalUnitsHealed = addParticipantDto.TotalUnitsHealed;
        TripleKills = addParticipantDto.TripleKills;
        TrueDamageDealt = addParticipantDto.TrueDamageDealt;
        TrueDamageDealtToChampions = addParticipantDto.TrueDamageDealtToChampions;
        TrueDamageTaken = addParticipantDto.TrueDamageTaken;
        TurretKills = addParticipantDto.TurretKills;
        TurretsLost = addParticipantDto.TurretsLost;
        TurretTakedowns = addParticipantDto.TurretTakedowns;
        VisionScore = addParticipantDto.VisionScore;
        VisionWardsBoughtInGame = addParticipantDto.VisionWardsBoughtInGame;
        WardsKilled = addParticipantDto.WardsKilled;
        WardsPlaced = addParticipantDto.WardsPlaced;
        Win = addParticipantDto.Win;
    }

    public Guid ChampionId { get; }

    public Guid SummonerId { get; }
    
    public int Assists { get; }

    public int ChampLevel { get; }

    public int? DamageDealtToBuildings { get; set; }

    public int DamageDealtToObjectives { get; set; }

    public int DamageDealtToTurrets { get; set; }

    public int DamageSelfMitigated { get; set; }

    public int Deaths { get; set; }

    public int DetectorWardsPlaced { get; set; }

    public int DoubleKills { get; set; }

    public bool FirstBloodKill { get; set; }

    public bool FirstTowerKill { get; set; }

    public bool GameEndedInEarlySurrender { get; set; }

    public bool GameEndedInSurrender { get; set; }

    public int GoldEarned { get; set; }

    public int GoldSpent { get; set; }

    public int Item0 { get; set; }

    public int Item1 { get; set; }

    public int Item2 { get; set; }

    public int Item3 { get; set; }

    public int Item4 { get; set; }

    public int Item5 { get; set; }

    public int Item6 { get; set; }

    public int ItemsPurchased { get; set; }

    public int KillingSprees { get; set; }

    public int Kills { get; set; }

    public int LargestCriticalStrike { get; set; }

    public int LargestKillingSpree { get; set; }

    public int LargestMultiKill { get; set; }

    public int LongestTimeSpentLiving { get; set; }

    public int MagicDamageDealt { get; set; }

    public int MagicDamageDealtToChampions { get; set; }

    public int MagicDamageTaken { get; set; }

    public int NeutralMinionsKilled { get; set; }

    public int NexusKills { get; set; }

    public int ObjectivesStolen { get; set; }

    public int PentaKills { get; set; }

    public Perks Perks { get; }

    public int PhysicalDamageDealt { get; set; }
    
    public int PhysicalDamageDealtToChampions { get; set; }
    
    public int PhysicalDamageTaken { get; set; }
    
    public int? Placement { get; set; }
    
    public int? PlayerAugment1 { get; set; }
    
    public int? PlayerAugment2 { get; set; }
    
    public int? PlayerAugment3 { get; set; }
    
    public int? PlayerAugment4 { get; set; }
    
    public int? PlayerSubteamId { get; set; }
    
    public int QuadraKills { get; set; }
    
    public int Spell1Casts { get; set; }
    
    public int Spell2Casts { get; set; }
    
    public int Spell3Casts { get; set; }
    
    public int Spell4Casts { get; set; }
    
    public int? SubteamPlacement { get; set; }
    
    public int Summoner1Casts { get; set; }
    
    public int Summoner1Id { get; set; }
    
    public int Summoner2Casts { get; set; }
    
    public int Summoner2Id { get; set; }
    
    public bool TeamEarlySurrendered { get; set; }
    
    public Side Side { get; set; }
    
    public string TeamPosition { get; set; }
    
    public int TimeCCingOthers { get; set; }
    
    public int TimePlayed { get; set; }
    
    public int TotalDamageDealt { get; set; }
    
    public int TotalDamageDealtToChampions { get; set; }
    
    public int TotalDamageShieldedOnTeammates { get; set; }
    
    public int TotalDamageTaken { get; set; }

    public int TotalHeal { get; set; }
    
    public int TotalHealsOnTeammates { get; set; }
    
    public int TotalMinionsKilled { get; set; }
    
    public int TotalTimeCCDealt { get; set; }
    
    public int TotalTimeSpentDead { get; set; }
    
    public int TotalUnitsHealed { get; set; }
    
    public int TripleKills { get; set; }
    
    public int TrueDamageDealt { get; set; }
    
    public int TrueDamageDealtToChampions { get; set; }
    
    public int TrueDamageTaken { get; set; }
    
    public int TurretKills { get; set; }
    
    public int? TurretsLost { get; set; }
    
    public int? TurretTakedowns { get; set; }
    
    public int VisionScore { get; set; }
    
    public int VisionWardsBoughtInGame { get; set; }
    
    public int WardsKilled { get; set; }
    
    public int WardsPlaced { get; set; }
    
    public bool Win { get; set; }
}