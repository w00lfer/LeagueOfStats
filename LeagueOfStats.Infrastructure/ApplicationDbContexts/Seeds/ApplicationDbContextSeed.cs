using System.Text.Json;
using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Skins;
using LeagueOfStats.Infrastructure.Champions;
using LeagueOfStats.Infrastructure.JsonConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Seeds;

public static class ApplicationDbContextSeed
{
    public static async Task SeedDataAsync(ApplicationDbContext applicationDbContext)
    {
        IDbContextTransaction transaction = applicationDbContext.Database.BeginTransaction();
        
        try
        { 
            if (await applicationDbContext.Champions.AnyAsync() is false)
            {
                await applicationDbContext.Champions.AddRangeAsync(GetChampions());
            }

            if (await applicationDbContext.Skins.AnyAsync() is false)
            {
                await applicationDbContext.Skins.AddRangeAsync(GetSkins());
            }
            
            await applicationDbContext.SaveChangesAsync();
            
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
        }
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
                    c.Value.ChampionDataConfigurationImageModel.FullFileName,
                    c.Value.ChampionDataConfigurationImageModel.SpriteFileName,
                    c.Value.ChampionDataConfigurationImageModel.Height,
                    c.Value.ChampionDataConfigurationImageModel.Width)));

        return champions.ToList();
    }

    private static List<Skin> GetSkins()
    {
        using StreamReader r = new StreamReader(ConfigurationPaths.GetSkinsConfigurationPath());
        var skinConfigurationModel = JsonSerializer.Deserialize<Dictionary<string, SkinDataConfigurationModel>>(r.ReadToEnd());
        var skins = skinConfigurationModel.Values.Select(s =>
            new Skin(new AddSkinDto(
                s.Id,
                s.IsBase,
                s.Name,
                s.Description,
                s.SplashPath,
                s.UncenteredSplashPath,
                s.TilePath,
                s.LoadScreenPath,
                s.LoadScreenVintagePath,
                s.Rarity,
                s.IsLegacy,
                s.ChromaPath,
                s.SkinDataConfigurationChromas is null
                    ? Enumerable.Empty<AddSkinChromaDto>()
                    : s.SkinDataConfigurationChromas.Select(sc => new AddSkinChromaDto(
                        sc.Id,
                        sc.ChromaPath,
                        sc.Colors)))));

        return skins.ToList();
    }
}