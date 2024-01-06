using LeagueOfStats.Domain.Summoners;
using Microsoft.EntityFrameworkCore;

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Summoner> Summoners { get; set; }

    public DbSet<SummonerChampionMastery> SummonerChampionMasteries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}