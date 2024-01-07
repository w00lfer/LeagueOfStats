using LeagueOfStats.Domain.Matches.Participants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Configurations.Matches.Participants;

public class PerksConfiguration : EntityConfiguration<Perks>, IEntityTypeConfiguration<Perks>
{
    public void Configure(EntityTypeBuilder<Perks> builder)
    {
        ConfigureDefaultProperties(builder);

        builder
            .HasOne(p => p.StatPerks)
            .WithOne(ps => ps.Perks)
            .HasForeignKey<PerkStats>(ps => ps.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(p => p.Styles)
            .WithOne(s => s.Perks)
            .OnDelete(DeleteBehavior.Cascade);
    }
}