using LeagueOfStats.Domain.Matches.Participants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Configurations.Matches.Participants;

public class PerksConfiguration : IEntityTypeConfiguration<Perks>
{
    public void Configure(EntityTypeBuilder<Perks> builder)
    {
        builder.HasKey(ps => ps.Id);

        builder
            .HasOne(p => p.StatPerks)
            .WithOne(ps => ps.Perks)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(p => p.Styles)
            .WithOne(s => s.Perks)
            .OnDelete(DeleteBehavior.Cascade);
    }
}