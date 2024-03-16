using LeagueOfStats.Domain.Matches.Teams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Configurations.Matches.Teams;

public class ObjectiveConfiguration : EntityConfigurationBase<Objective>, IEntityTypeConfiguration<Objective>
{
    public void Configure(EntityTypeBuilder<Objective> builder)
    {
        ConfigureDefaultProperties(builder);

        builder.Property(o => o.First);

        builder.Property(o => o.Kills);
    }
}