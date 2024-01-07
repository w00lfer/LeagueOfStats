using LeagueOfStats.Domain.Matches.Teams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Configurations.Matches.Teams;

public class TeamConfiguration : EntityConfigurationBase<Team>, IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        ConfigureDefaultProperties(builder);

        builder.Property(t => t.Side);

        builder.Property(t => t.Win);

        builder
            .HasOne(t => t.Objectives)
            .WithOne(o => o.Team)
            .HasForeignKey<Objectives>(o => o.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(t => t.Bans)
            .WithOne(b => b.Team)
            .OnDelete(DeleteBehavior.Cascade);
    }
}