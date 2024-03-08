namespace LeagueOfStats.Domain.Skins;

public record AddSkinDto(
    int RiotSkinId,
    bool IsBase,
    string Name,
    string Description,
    string SplashPath,
    string UncenteredSplashPath,
    string TilePath,
    string LoadScreenPath,
    string LoadScreenVintagePath,
    string Rarity,
    bool IsLegacy,
    string ChromaPath,
    IEnumerable<AddSkinChromaDto> AddSkinChromaDtos);