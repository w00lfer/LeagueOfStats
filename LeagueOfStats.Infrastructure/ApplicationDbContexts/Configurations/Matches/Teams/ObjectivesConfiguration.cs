using LeagueOfStats.Domain.Matches.Teams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Configurations.Matches.Teams;

public class ObjectivesConfiguration : EntityConfiguration<Objectives>, IEntityTypeConfiguration<Objectives>
{
    public void Configure(EntityTypeBuilder<Objectives> builder)
    {
        ConfigureDefaultProperties(builder);

        builder
            .HasOne(o => o.BaronObjective)
            .WithOne()
            .HasForeignKey<Objective>(o => o.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(o => o.ChampionObjective)
            .WithOne()
            .HasForeignKey<Objective>(o => o.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(o => o.DragonObjective)
            .WithOne()
            .HasForeignKey<Objective>(o => o.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(o => o.HordeObjective)
            .WithOne()
            .HasForeignKey<Objective>(o => o.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(o => o.InhibitorObjective)
            .WithOne()
            .HasForeignKey<Objective>(o => o.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(o => o.RiftHeraldObjective)
            .WithOne()
            .HasForeignKey<Objective>(o => o.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(o => o.TowerObjective)
            .WithOne()
            .HasForeignKey<Objective>(o => o.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}