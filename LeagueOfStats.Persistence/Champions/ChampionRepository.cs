using System.Collections.Immutable;
using System.Text.Json;
using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Persistence.Extensions;
using LeagueOfStats.Persistence.JsonConfigurations;

namespace LeagueOfStats.Persistence.Champions
{
    public class ChampionRepository : IChampionRepository
    {
        private readonly ImmutableDictionary<int, Champion> _championsById;

        public ChampionRepository()
        {
            using (StreamReader r = new StreamReader(ConfigurationPaths.GetChampionConfigurationPath()))
            {
                var champions = JsonSerializer.Deserialize<ChampionConfigurationModel>(r.ReadToEnd());
                
                // TODO THROW ERROR WHEN NULL OR EMPTY!
                
                _championsById = champions!.ChampionDataConfigurationModels.ToImmutableDictionary(c => Int32.Parse(c.Value.Id), c =>
                    new Champion
                    {
                        Id = Int32.Parse(c.Value.Id), 
                        Name = c.Value.Name
                    });
            }
        }

        public Champion? GetById(int id) =>
            _championsById.TryGetValue(id, out var champion)
                ? champion
                : null;

        public IEnumerable<Champion> GetAll(params int[] ids) =>
            ids.Length > 0
                ? _championsById.GetMultiple(ids)
                : _championsById.Values;
    }
}