using System.Text.Json.Serialization;

namespace LeagueOfStats.Persistence.Champions;

public record ChampionDataConfigurationModel(
    [property: JsonPropertyName("key")] string Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("blurb")] string Description,
    [property: JsonPropertyName("image")] ChampionConfigurationImageModel ChampionConfigurationImageModel);