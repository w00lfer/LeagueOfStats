using System.Text.Json.Serialization;

namespace LeagueOfStats.Infrastructure.Skins;

public record SkinDataConfigurationModel(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("isBase")] bool IsBase,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("splashPath")] string SplashPath,
    [property: JsonPropertyName("uncenteredSplashPath")] string UncenteredSplashPath,
    [property: JsonPropertyName("tilePath")] string TilePath,
    [property: JsonPropertyName("loadScreenPath")] string LoadScreenPath,
    [property: JsonPropertyName("loadScreenVintagePath")] string LoadScreenVintagePath,
    [property: JsonPropertyName("skinType")] string SkinType,
    [property: JsonPropertyName("rarity")] string Rarity,
    [property: JsonPropertyName("isLegacy")] bool IsLegacy,
    [property: JsonPropertyName("splashVideoPath")] object SplashVideoPath,
    [property: JsonPropertyName("collectionSplashVideoPath")] object CollectionSplashVideoPath,
    [property: JsonPropertyName("featuresText")] object FeaturesText,
    [property: JsonPropertyName("chromaPath")] string ChromaPath,
    [property: JsonPropertyName("chromas")] IReadOnlyList<SkinDataConfigurationChromaModel> SkinDataConfigurationChromas,
    [property: JsonPropertyName("emblems")] object Emblems,
    [property: JsonPropertyName("regionRarityId")] int RegionRarityId,
    [property: JsonPropertyName("rarityGemPath")] object RarityGemPath,
    [property: JsonPropertyName("skinAugments")] object SkinAugments,
    [property: JsonPropertyName("description")] string Description);