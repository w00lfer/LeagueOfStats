using LeagueOfStats.Domain.Matches.Participants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Configurations.Matches.Participants;

public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        builder.HasKey(p => p.Id);

        builder
            .HasOne(p => p.Perks)
            .WithOne(p => p.Participant)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(p => p.ChampionId);

        builder.Property(p => p.SummonerId);
        
        builder.Property(p => p.Assists);
        
        builder.Property(p => p.ChampLevel);
        
        builder.Property(p => p.DamageDealtToBuildings);
        
        builder.Property(p => p.DamageDealtToObjectives);
        
        builder.Property(p => p.DamageDealtToTurrets);
        
        builder.Property(p => p.DamageSelfMitigated);
        
        builder.Property(p => p.Deaths);
        
        builder.Property(p => p.DetectorWardsPlaced);
        
        builder.Property(p => p.DoubleKills);
        
        builder.Property(p => p.FirstBloodKill);
        
        builder.Property(p => p.FirstTowerKill);
        
        builder.Property(p => p.GameEndedInEarlySurrender);
        
        builder.Property(p => p.GameEndedInSurrender);
        
        builder.Property(p => p.GoldEarned);
        
        builder.Property(p => p.GoldSpent);
        
        builder.Property(p => p.Item0);
        
        builder.Property(p => p.Item1);
        
        builder.Property(p => p.Item2);
        
        builder.Property(p => p.Item3);
        
        builder.Property(p => p.Item4);
        
        builder.Property(p => p.Item5);
        
        builder.Property(p => p.Item6);
        
        builder.Property(p => p.ItemsPurchased);
        
        builder.Property(p => p.KillingSprees);
        
        builder.Property(p => p.Kills);
        
        builder.Property(p => p.LargestCriticalStrike);
        
        builder.Property(p => p.LargestKillingSpree);
        
        builder.Property(p => p.LargestMultiKill);
        
        builder.Property(p => p.LongestTimeSpentLiving);
        
        builder.Property(p => p.MagicDamageDealt);
        
        builder.Property(p => p.MagicDamageDealtToChampions);
        
        builder.Property(p => p.MagicDamageTaken);
        
        builder.Property(p => p.NeutralMinionsKilled);
        
        builder.Property(p => p.NexusKills);
        
        builder.Property(p => p.ObjectivesStolen);
        
        builder.Property(p => p.PentaKills);
        
        builder.Property(p => p.PhysicalDamageDealt);
        
        builder.Property(p => p.PhysicalDamageDealtToChampions);
        
        builder.Property(p => p.PhysicalDamageTaken);
        
        builder.Property(p => p.Placement);
        
        builder.Property(p => p.PlayerAugment1);
        
        builder.Property(p => p.PlayerAugment2);
        
        builder.Property(p => p.PlayerAugment3);
        
        builder.Property(p => p.PlayerAugment4);
        
        builder.Property(p => p.PlayerSubteamId);
        
        builder.Property(p => p.QuadraKills);
        
        builder.Property(p => p.Spell1Casts);
        
        builder.Property(p => p.Spell2Casts);
        
        builder.Property(p => p.Spell3Casts);
        
        builder.Property(p => p.Spell4Casts);
        
        builder.Property(p => p.SubteamPlacement);
        
        builder.Property(p => p.Summoner1Casts);
        
        builder.Property(p => p.Summoner1Id);
        
        builder.Property(p => p.Summoner2Casts);
        
        builder.Property(p => p.Summoner2Id);
        
        builder.Property(p => p.TeamEarlySurrendered);
        
        builder.Property(p => p.Side);
        
        builder.Property(p => p.TeamPosition);
        
        builder.Property(p => p.TimeCCingOthers);
        
        builder.Property(p => p.TimePlayed);
        
        builder.Property(p => p.TotalDamageDealt);
        
        builder.Property(p => p.TotalDamageDealtToChampions);
        
        builder.Property(p => p.TotalDamageShieldedOnTeammates);
        
        builder.Property(p => p.TotalDamageTaken);
        
        builder.Property(p => p.TotalHeal);
        
        builder.Property(p => p.TotalHealsOnTeammates);
        
        builder.Property(p => p.TotalMinionsKilled);
        
        builder.Property(p => p.TotalTimeCCDealt);
        
        builder.Property(p => p.TotalTimeSpentDead);
        
        builder.Property(p => p.TotalUnitsHealed);
        
        builder.Property(p => p.TripleKills);
        
        builder.Property(p => p.TrueDamageDealt);
        
        builder.Property(p => p.TrueDamageDealtToChampions);
        
        builder.Property(p => p.TrueDamageTaken);
        
        builder.Property(p => p.TurretKills);
        
        builder.Property(p => p.TurretsLost);
        
        builder.Property(p => p.TurretTakedowns);
        
        builder.Property(p => p.VisionScore);
        
        builder.Property(p => p.VisionWardsBoughtInGame);
        
        builder.Property(p => p.WardsKilled);
        
        builder.Property(p => p.WardsPlaced);
        
        builder.Property(p => p.Win);
    }
}