using LeagueOfStats.Domain.Discounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Configurations.Discounts;

public class DiscountedChampionConfiguration : EntityConfigurationBase<DiscountedChampion>, IEntityTypeConfiguration<DiscountedChampion>
{
    public void Configure(EntityTypeBuilder<DiscountedChampion> builder)
    {
        ConfigureDefaultProperties(builder);

        builder.Property(dc => dc.ChampionId);

        builder.Property(dc => dc.OldPrice);
        
        builder.Property(dc => dc.NewPrice);
    }
}