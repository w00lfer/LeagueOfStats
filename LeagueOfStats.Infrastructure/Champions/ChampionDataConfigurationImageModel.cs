using System.Text.Json.Serialization;

namespace LeagueOfStats.Infrastructure.Champions;

public record ChampionDataConfigurationImageModel(
    [property: JsonPropertyName("full")] string FullFileName,
    [property: JsonPropertyName("sprite")] string SpriteFileName,
    [property: JsonPropertyName("w")] int Width,
    [property: JsonPropertyName("h")] int Height);