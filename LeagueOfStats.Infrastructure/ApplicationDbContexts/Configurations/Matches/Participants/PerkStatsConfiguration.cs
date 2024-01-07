using LeagueOfStats.Domain.Matches.Participants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Configurations.Matches.Participants;

public class PerkStatsConfiguration : EntityConfigurationBase<PerkStats>, IEntityTypeConfiguration<PerkStats>
{
    public void Configure(EntityTypeBuilder<PerkStats> builder)
    {
        ConfigureDefaultProperties(builder);

        builder.Property(ps => ps.Defense);

        builder.Property(ps => ps.Flex);

        builder.Property(ps => ps.Offense);
    }
}