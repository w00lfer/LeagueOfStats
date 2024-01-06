using LeagueOfStats.Domain.Matches.Teams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Configurations.Matches.Teams;

public class BanConfiguration : IEntityTypeConfiguration<Ban>
{
    public void Configure(EntityTypeBuilder<Ban> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.ChampionId);

        builder.Property(b => b.PickTurn);
    }
}