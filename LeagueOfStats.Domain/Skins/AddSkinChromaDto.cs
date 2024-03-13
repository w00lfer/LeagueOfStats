namespace LeagueOfStats.Domain.Skins;

public record AddSkinChromaDto(
    int RiotChromaId,
    string ChromaPath,
    IEnumerable<string> ColorAsStrings);