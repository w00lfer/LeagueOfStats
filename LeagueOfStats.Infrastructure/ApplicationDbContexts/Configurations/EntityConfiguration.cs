using LeagueOfStats.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Configurations;

public abstract class EntityConfiguration<T> where T : Entity
{
    public void ConfigureDefaultProperties(EntityTypeBuilder<T> builder)
    {
        builder.Property(e => e.Id)
            .ValueGeneratedNever();
    }
}