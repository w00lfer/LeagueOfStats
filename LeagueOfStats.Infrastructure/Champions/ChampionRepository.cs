using System.Collections.Immutable;
using System.Text.Json;
using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Infrastructure.Extensions;
using LeagueOfStats.Infrastructure.JsonConfigurations;

namespace LeagueOfStats.Infrastructure.Champions;

public class ChampionRepository : IChampionRepository
{
    private readonly ImmutableDictionary<int, Champion> _championsById;

    public ChampionRepository()
    {
        using (StreamReader r = new StreamReader(ConfigurationPaths.GetChampionConfigurationPath()))
        {
            var champions = JsonSerializer.Deserialize<ChampionConfigurationModel>(r.ReadToEnd());
                
            _championsById = champions!.ChampionDataConfigurationModels.ToImmutableDictionary(
                c => Int32.Parse(c.Value.Id), c =>
                    new Champion(
                        Int32.Parse(c.Value.Id),
                        c.Value.Name,
                        c.Value.Title,
                        c.Value.Description,
                        new ChampionImage(
                            c.Value.ChampionConfigurationImageModel.FullFileName,
                            c.Value.ChampionConfigurationImageModel.SpriteFileName,
                            c.Value.ChampionConfigurationImageModel.Height,
                            c.Value.ChampionConfigurationImageModel.Width)));
        }
    }

    public Task<Champion?> GetByIdAsync(int id) =>
        Task.FromResult(
            _championsById.TryGetValue(id, out var champion)
                ? champion
                : null);

    public Task<IEnumerable<Champion>> GetAllAsync(params int[] ids) =>
        Task.FromResult(ids.Length > 0
            ? _championsById.GetMultiple(ids)
            : _championsById.Values);
}