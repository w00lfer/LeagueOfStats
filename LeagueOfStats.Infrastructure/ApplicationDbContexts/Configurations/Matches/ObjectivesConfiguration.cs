using LeagueOfStats.Domain.Matches.Teams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Configurations.Matches;

public class ObjectivesConfiguration : IEntityTypeConfiguration<Objectives>
{
    public void Configure(EntityTypeBuilder<Objectives> builder)
    {
        builder.HasKey(o => o.Id);

        builder
            .HasOne(o => o.BaronObjective)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(o => o.ChampionObjective)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(o => o.DragonObjective)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(o => o.HordeObjective)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(o => o.InhibitorObjective)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(o => o.RiftHeraldObjective)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(o => o.TowerObjective)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}