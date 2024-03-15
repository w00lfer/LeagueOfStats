namespace LeagueOfStats.Application.ApiClients.CommunityDragonClient;

public record SkinChromaDto(
    int RiotChromaId,
    string ChromaPath,
    IEnumerable<string> ColorAsStrings);