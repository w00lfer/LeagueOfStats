using LeagueOfStats.Domain.Summoners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Configurations;

public class SummonerConfiguration : EntityConfigurationBase<Summoner>, IEntityTypeConfiguration<Summoner>
{
    public void Configure(EntityTypeBuilder<Summoner> builder)
    {
        ConfigureDefaultProperties(builder);

        builder.Property(s => s.SummonerId);

        builder.Property(s => s.AccountId);

        builder.Property(s => s.Name);

        builder.Property(s => s.ProfileIconId);

        builder.Property(s => s.Puuid);

        builder.Property(s => s.SummonerLevel);

        builder.ComplexProperty<SummonerName>(s => s.SummonerName, snBuilder =>
        {
            snBuilder.Property(sn => sn.GameName);

            snBuilder.Property(sn => sn.TagLine);
        });

        builder.Property(s => s.Region);

        builder.Property(s => s.LastUpdated);

        builder
            .HasMany(s => s.SummonerChampionMasteries)
            .WithOne(scm => scm.Summoner)
            .OnDelete(DeleteBehavior.Cascade);
    }
}