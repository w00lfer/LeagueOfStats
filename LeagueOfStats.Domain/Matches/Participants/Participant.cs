using LeagueOfStats.Domain.Common.Entities;
using LeagueOfStats.Domain.Matches.Participants.Dtos;
using LeagueOfStats.Domain.Matches.Teams.Enums;

namespace LeagueOfStats.Domain.Matches.Participants;

public class Participant : Entity
{
    public Participant(
        AddParticipantDto addParticipantDto,
        Match match)
        : base(Guid.NewGuid())
    {
        Match = match;
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
        Perks = new Perks(addParticipantDto.AddPerksDto, this);
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

    private Participant()
        : base(Guid.Empty)
    {
    }

    public Match Match { get; }
    
    public Guid ChampionId { get; }

    public Guid SummonerId { get; }
    
    public int Assists { get; }

    public int ChampLevel { get; }

    public int? DamageDealtToBuildings { get;  }

    public int DamageDealtToObjectives { get;  }

    public int DamageDealtToTurrets { get;  }

    public int DamageSelfMitigated { get;  }

    public int Deaths { get;  }

    public int DetectorWardsPlaced { get;  }

    public int DoubleKills { get;  }

    public bool FirstBloodKill { get;  }

    public bool FirstTowerKill { get;  }

    public bool GameEndedInEarlySurrender { get;  }

    public bool GameEndedInSurrender { get;  }

    public int GoldEarned { get;  }

    public int GoldSpent { get;  }

    public int Item0 { get;  }

    public int Item1 { get;  }

    public int Item2 { get;  }

    public int Item3 { get;  }

    public int Item4 { get;  }

    public int Item5 { get;  }

    public int Item6 { get;  }

    public int ItemsPurchased { get;  }

    public int KillingSprees { get;  }

    public int Kills { get;  }

    public int LargestCriticalStrike { get;  }

    public int LargestKillingSpree { get;  }

    public int LargestMultiKill { get;  }

    public int LongestTimeSpentLiving { get;  }

    public int MagicDamageDealt { get;  }

    public int MagicDamageDealtToChampions { get;  }

    public int MagicDamageTaken { get;  }

    public int NeutralMinionsKilled { get;  }

    public int NexusKills { get;  }

    public int ObjectivesStolen { get;  }

    public int PentaKills { get;  }

    public Perks Perks { get; }

    public int PhysicalDamageDealt { get;  }
    
    public int PhysicalDamageDealtToChampions { get;  }
    
    public int PhysicalDamageTaken { get;  }
    
    public int? Placement { get;  }
    
    public int? PlayerAugment1 { get;  }
    
    public int? PlayerAugment2 { get;  }
    
    public int? PlayerAugment3 { get;  }
    
    public int? PlayerAugment4 { get;  }
    
    public int? PlayerSubteamId { get;  }
    
    public int QuadraKills { get;  }
    
    public int Spell1Casts { get;  }
    
    public int Spell2Casts { get;  }
    
    public int Spell3Casts { get;  }
    
    public int Spell4Casts { get;  }
    
    public int? SubteamPlacement { get;  }
    
    public int Summoner1Casts { get;  }
    
    public int Summoner1Id { get;  }
    
    public int Summoner2Casts { get;  }
    
    public int Summoner2Id { get;  }
    
    public bool TeamEarlySurrendered { get;  }
    
    public Side Side { get;  }
    
    public string TeamPosition { get;  }
    
    public int TimeCCingOthers { get;  }
    
    public int TimePlayed { get;  }
    
    public int TotalDamageDealt { get;  }
    
    public int TotalDamageDealtToChampions { get;  }
    
    public int TotalDamageShieldedOnTeammates { get;  }
    
    public int TotalDamageTaken { get;  }

    public int TotalHeal { get;  }
    
    public int TotalHealsOnTeammates { get;  }
    
    public int TotalMinionsKilled { get;  }
    
    public int TotalTimeCCDealt { get;  }
    
    public int TotalTimeSpentDead { get;  }
    
    public int TotalUnitsHealed { get;  }
    
    public int TripleKills { get;  }
    
    public int TrueDamageDealt { get;  }
    
    public int TrueDamageDealtToChampions { get;  }
    
    public int TrueDamageTaken { get;  }
    
    public int TurretKills { get;  }
    
    public int? TurretsLost { get;  }
    
    public int? TurretTakedowns { get;  }
    
    public int VisionScore { get;  }
    
    public int VisionWardsBoughtInGame { get;  }
    
    public int WardsKilled { get;  }
    
    public int WardsPlaced { get;  }
    
    public bool Win { get;  }
}