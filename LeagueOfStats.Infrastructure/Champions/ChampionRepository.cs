using System.Collections.Immutable;
using System.Text.Json;
using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Infrastructure.Extensions;
using LeagueOfStats.Infrastructure.JsonConfigurations;

namespace LeagueOfStats.Infrastructure.Champions;

public class ChampionRepository : IChampionRepository
{
    private readonly ImmutableDictionary<Guid, Champion> _championsById;

    public ChampionRepository()
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
            
        _championsById = champions.ToImmutableDictionary(c => c.Id, c => c);
    }

    public Task<Champion?> GetByIdAsync(Guid id) =>
        Task.FromResult(
            _championsById.TryGetValue(id, out var champion)
                ? champion
                : null);

    public Task<IEnumerable<Champion>> GetAllAsync(params Guid[] ids) =>
        Task.FromResult(ids.Length > 0
            ? _championsById.GetMultiple(ids)
            : _championsById.Values);
}