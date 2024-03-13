using LeagueOfStats.Domain.Discounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Configurations.Discounts;

public class DiscountConfiguration : EntityConfigurationBase<Discount>, IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        ConfigureDefaultProperties(builder);

        builder.Property(d => d.StartDateTime);

        builder.Property(d => d.EndDateTime);
        
        builder
            .HasMany(d => d.DiscountedChampions)
            .WithOne(dc => dc.Discount)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasMany(d => d.DiscountedSkins)
            .WithOne(ds => ds.Discount)
            .OnDelete(DeleteBehavior.Cascade);
    }
}