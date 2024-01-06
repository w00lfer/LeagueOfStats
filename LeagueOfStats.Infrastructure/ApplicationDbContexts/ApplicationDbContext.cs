using LeagueOfStats.Domain.Matches;
using LeagueOfStats.Domain.Matches.Teams;
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

    public DbSet<Match> Matches { get; set; }

    public DbSet<Team> Teams { get; set; }

    public DbSet<Objectives> TeamObjectives { get; set; }

    public DbSet<Objective> Objectives { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}