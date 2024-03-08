using System.Text.Json.Serialization;

namespace LeagueOfStats.Infrastructure.Skins;

public record SkinDataConfigurationChromaModel(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("chromaPath")] string ChromaPath,
    [property: JsonPropertyName("colors")] IReadOnlyList<string> Colors
);