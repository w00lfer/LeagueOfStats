using System.Collections.Immutable;
using System.Text.Json;
using LeagueOfStats.Domain.Champions;
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

        public Champion? GetById(int id)
        {
            var hasChampion = _championsById.ContainsKey(id);

            return hasChampion
                ? _championsById[id]
                : null;
        }

        public IEnumerable<Champion> GetAll() =>
            _championsById.Values;
    }
}