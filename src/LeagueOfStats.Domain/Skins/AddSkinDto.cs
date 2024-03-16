namespace LeagueOfStats.Domain.Skins;

public record AddSkinDto(
    int RiotSkinId,
    bool IsBase,
    string Name,
    string Description,
    string SplashUrl,
    string UncenteredSplashUrl, 
    string TileUrl,
    string Rarity,
    bool IsLegacy,
    string ChromaPath,
    IEnumerable<AddSkinChromaDto> AddSkinChromaDtos);