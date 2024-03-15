using LeagueOfStats.Domain.Skins;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Configurations.Skins;

public class SkinChromaConfiguration : EntityConfigurationBase<SkinChroma>, IEntityTypeConfiguration<SkinChroma>
{
    public void Configure(EntityTypeBuilder<SkinChroma> builder)
    {
        ConfigureDefaultProperties(builder);

        builder.Property(sc => sc.RiotChromaId);

        builder.Property(sc => sc.ChromaPath);

        builder.Property(sc => sc.ColorsAsStringSeparatedByComma);
    }
}