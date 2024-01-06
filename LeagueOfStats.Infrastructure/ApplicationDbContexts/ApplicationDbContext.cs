using LeagueOfStats.Domain.Matches;
using LeagueOfStats.Domain.Matches.Participants;
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

    public DbSet<Ban> Bans { get; set; }

    public DbSet<Participant> Participants { get; set; }

    public DbSet<Perks> Perks { get; set; }

    public DbSet<PerkStats> PerkStats { get; set; }

    public DbSet<PerkStyle> PerkStyles { get; set; }

    public DbSet<PerkStyleSelection> PerkStyleSelections { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}