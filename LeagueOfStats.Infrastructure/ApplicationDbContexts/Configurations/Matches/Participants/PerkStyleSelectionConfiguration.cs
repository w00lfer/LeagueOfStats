using LeagueOfStats.Domain.Matches.Participants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Configurations.Matches.Participants;

public class PerkStyleSelectionConfiguration : IEntityTypeConfiguration<PerkStyleSelection>
{
    public void Configure(EntityTypeBuilder<PerkStyleSelection> builder)
    {
        builder.HasKey(pss => pss.Id);

        builder.Property(pss => pss.Perk);

        builder.Property(pss => pss.Var1);

        builder.Property(pss => pss.Var2);

        builder.Property(pss => pss.Var3);
    }
}