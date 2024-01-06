using LeagueOfStats.Domain.Matches.Participants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Configurations.Matches.Participants;

public class PerkStyleConfiguration : IEntityTypeConfiguration<PerkStyle>
{
    public void Configure(EntityTypeBuilder<PerkStyle> builder)
    {
        builder.HasKey(ps => ps.Id);

        builder.Property(ps => ps.Description);

        builder.Property(ps => ps.Description);

        builder
            .HasMany(ps => ps.Selections)
            .WithOne(s => s.PerkStyle)
            .OnDelete(DeleteBehavior.Cascade);
    }
}