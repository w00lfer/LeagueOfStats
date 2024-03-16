namespace LeagueOfStats.Jobs.ApiClients.CommunityDragonClient;

public record SkinDto(
    int RiotSkinId,
    bool IsBase,
    string Name,
    string Description,
    string Rarity,
    bool IsLegacy,
    string ChromaPath,
    IEnumerable<SkinChromaDto> SkinChromaDtos);