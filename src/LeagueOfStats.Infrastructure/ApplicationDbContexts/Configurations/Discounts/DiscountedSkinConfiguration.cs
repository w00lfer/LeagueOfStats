using LeagueOfStats.Domain.Discounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Configurations.Discounts;

public class DiscountedSkinConfiguration : EntityConfigurationBase<DiscountedSkin>, IEntityTypeConfiguration<DiscountedSkin>
{
    public void Configure(EntityTypeBuilder<DiscountedSkin> builder)
    {
        ConfigureDefaultProperties(builder);

        builder.Property(ds => ds.SkinId);

        builder.Property(ds => ds.OldPrice);
        
        builder.Property(d => d.NewPrice);
    }
}