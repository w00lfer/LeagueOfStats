using LeagueOfStats.Domain.Skins;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Configurations.Skins;

public class SkinConfiguration : EntityConfigurationBase<Skin>, IEntityTypeConfiguration<Skin>
{
    public void Configure(EntityTypeBuilder<Skin> builder)
    {
        ConfigureDefaultProperties(builder);

        builder.Property(s => s.RiotSkinId);
        
        builder.Property(s => s.IsBase);
        
        builder.Property(s => s.Name);
        
        builder.Property(s => s.Description);
        
        builder.Property(s => s.SplashUrl);
        
        builder.Property(s => s.UncenteredSplashUrl);
        
        builder.Property(s => s.TileUrl);
        
        builder.Property(s => s.Rarity);
        
        builder.Property(s => s.ChromaPath);

        builder
            .HasMany(s => s.Chromas)
            .WithOne(sc => sc.Skin)
            .OnDelete(DeleteBehavior.Cascade);
    }
}