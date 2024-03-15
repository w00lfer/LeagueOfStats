using System.Text.Json.Serialization;

namespace LeagueOfStats.Application.ApiClients.DataDragonClient;

public record DataDragonChampionDto(
    [property: JsonPropertyName("data")] 
    Dictionary<string, DataDragonChampionDataDto> ChampionDataConfigurationModels);
    
public record DataDragonChampionDataDto(
    [property: JsonPropertyName("key")] string Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("blurb")] string Description,
    [property: JsonPropertyName("image")] DataDragonChampionDataImageDto ChampionDataConfigurationImageModel);
    
public record DataDragonChampionDataImageDto(
    [property: JsonPropertyName("full")] string FullFileName,
    [property: JsonPropertyName("sprite")] string SpriteFileName,
    [property: JsonPropertyName("w")] int Width,
    [property: JsonPropertyName("h")] int Height);