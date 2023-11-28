using System.Text.Json.Serialization;

namespace LeagueOfStats.Infrastructure.Champions;

public record ChampionConfigurationModel(
    [property: JsonPropertyName("data")] 
    Dictionary<string, ChampionDataConfigurationModel> ChampionDataConfigurationModels);