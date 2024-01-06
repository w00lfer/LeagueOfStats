using LeagueOfStats.Domain.Matches.Participants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Configurations.Matches.Participants;

public class PerkStatsConfiguration : IEntityTypeConfiguration<PerkStats>
{
    public void Configure(EntityTypeBuilder<PerkStats> builder)
    {
        builder.HasKey(ps => ps.Id);

        builder.Property(ps => ps.Defense);

        builder.Property(ps => ps.Flex);

        builder.Property(ps => ps.Offense);
    }
}