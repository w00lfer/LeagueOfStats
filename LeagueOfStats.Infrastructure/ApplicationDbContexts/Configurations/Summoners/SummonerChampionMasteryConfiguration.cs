using LeagueOfStats.Domain.Summoners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Configurations;

public class SummonerChampionMasteryConfiguration : EntityConfiguration<SummonerChampionMastery>, IEntityTypeConfiguration<SummonerChampionMastery>
{
    public void Configure(EntityTypeBuilder<SummonerChampionMastery> builder)
    {
        ConfigureDefaultProperties(builder);

        builder.Property(scm => scm.RiotChampionId);

        builder.Property(scm => scm.ChampionLevel);

        builder.Property(scm => scm.ChampionPoints);

        builder.Property(scm => scm.ChampionPointsSinceLastLevel);

        builder.Property(scm => scm.ChampionPointsUntilNextLevel);

        builder.Property(scm => scm.ChestGranted);

        builder.Property(scm => scm.LastPlayTime);

        builder.Property(scm => scm.TokensEarned);
    }
}