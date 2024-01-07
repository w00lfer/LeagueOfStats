using LeagueOfStats.Domain.Matches.Teams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Configurations.Matches.Teams;

public class BanConfiguration : EntityConfiguration<Ban>, IEntityTypeConfiguration<Ban>
{
    public void Configure(EntityTypeBuilder<Ban> builder)
    {
        ConfigureDefaultProperties(builder);

        builder.Property(b => b.ChampionId);

        builder.Property(b => b.PickTurn);
    }
}