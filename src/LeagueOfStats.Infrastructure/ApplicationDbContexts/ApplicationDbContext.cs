using AppAny.Quartz.EntityFrameworkCore.Migrations;
using AppAny.Quartz.EntityFrameworkCore.Migrations.SqlServer;
using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Discounts;
using LeagueOfStats.Domain.Matches;
using LeagueOfStats.Domain.Matches.Participants;
using LeagueOfStats.Domain.Matches.Teams;
using LeagueOfStats.Domain.Skins;
using LeagueOfStats.Domain.Summoners;
using Microsoft.EntityFrameworkCore;

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Champion> Champions { get; set; }
    
    public DbSet<Skin> Skins { get; set; }
    
    public DbSet<SkinChroma> SkinChromas { get; set; }

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
    
    public DbSet<Discount> Discounts { get; set; }

    public DbSet<DiscountedChampion> DiscountedChampions { get; set; }
    
    public DbSet<DiscountedSkin> DiscountedSkins { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.AddQuartz(builder => builder.UseSqlServer("QRTZ_", null));
    }
}