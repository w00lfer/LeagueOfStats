using System.Text.Json;
using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Infrastructure.Champions;
using LeagueOfStats.Infrastructure.JsonConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Seeds;

public static class ApplicationDbContextSeed
{
    public static async Task SeedDataAsync(ApplicationDbContext applicationDbContext)
    {
        if (await applicationDbContext.Champions.AnyAsync() is false)
        {
            await SeedChampionsAsync(applicationDbContext);
        }
    }

    private static async Task SeedChampionsAsync(ApplicationDbContext applicationDbContext)
    {
        await applicationDbContext.Champions.AddRangeAsync(GetChampions());

        await applicationDbContext.SaveChangesAsync();
    }

    private static List<Champion> GetChampions()
    {
        using StreamReader r = new StreamReader(ConfigurationPaths.GetChampionConfigurationPath());
        var championConfigurationModel = JsonSerializer.Deserialize<ChampionConfigurationModel>(r.ReadToEnd());
        var champions = championConfigurationModel.ChampionDataConfigurationModels.Select(c =>
            new Champion(
                Int32.Parse(c.Value.Id),
                c.Value.Name,
                c.Value.Title,
                c.Value.Description,
                ChampionImage.Create(
                    c.Value.ChampionConfigurationImageModel.FullFileName,
                    c.Value.ChampionConfigurationImageModel.SpriteFileName,
                    c.Value.ChampionConfigurationImageModel.Height,
                    c.Value.ChampionConfigurationImageModel.Width)));

        return champions.ToList();
    }
}