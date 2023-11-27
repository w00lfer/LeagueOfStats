using System.Text.Json.Serialization;

namespace LeagueOfStats.Persistence.Champions;

public record ChampionConfigurationImageModel(
    [property: JsonPropertyName("full")] string FullFileName,
    [property: JsonPropertyName("sprite")] string SpriteFileName,
    [property: JsonPropertyName("w")] int Width,
    [property: JsonPropertyName("h")] int Height);