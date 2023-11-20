using System.Text.Json.Serialization;

namespace LeagueOfStats.Persistence.Champions
{
    public record ChampionConfigurationModel(
        [property: JsonPropertyName("data")] 
        Dictionary<string, ChampionDataConfigurationModel> ChampionDataConfigurationModels);
}