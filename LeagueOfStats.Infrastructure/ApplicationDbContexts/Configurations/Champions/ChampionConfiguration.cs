using LeagueOfStats.Domain.Champions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Configurations.Champions;

public class ChampionConfiguration :EntityConfigurationBase<Champion>, IEntityTypeConfiguration<Champion>
{
    public void Configure(EntityTypeBuilder<Champion> builder)
    {
        ConfigureDefaultProperties(builder);

        builder.Property(c => c.RiotChampionId);

        builder.Property(c => c.Name);

        builder.Property(c => c.Title);

        builder.Property(c => c.Description);

        builder.ComplexProperty(c => c.ChampionImage, ciBuilder =>
        {
            ciBuilder.Property(ci => ci.SplashUrl);

            ciBuilder.Property(ci => ci.IconUrl);
        });
    }
}