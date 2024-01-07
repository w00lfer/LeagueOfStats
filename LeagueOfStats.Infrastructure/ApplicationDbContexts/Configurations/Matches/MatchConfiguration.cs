using LeagueOfStats.Domain.Matches;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Configurations.Matches;

public class MatchConfiguration : EntityConfiguration<Match>, IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        ConfigureDefaultProperties(builder);

        builder.Property(m => m.RiotMatchId);

        builder.Property(m => m.GameVersion);

        builder.Property(m => m.GameDuration);

        builder.Property(m => m.GameStartTimeStamp);

        builder.Property(m => m.GameEndTimestamp);

        builder.Property(m => m.GameMode);

        builder.Property(m => m.GameType);

        builder.Property(m => m.Map);

        builder.Property(m => m.PlatformId);

        builder.Property(m => m.TournamentCode);

        builder
            .HasMany(m => m.Participants)
            .WithOne(p => p.Match)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(m => m.Teams)
            .WithOne(t => t.Match)
            .OnDelete(DeleteBehavior.Cascade);
    }
}