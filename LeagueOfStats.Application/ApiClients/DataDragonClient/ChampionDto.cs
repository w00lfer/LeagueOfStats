namespace LeagueOfStats.Application.ApiClients.DataDragonClient;

public record ChampionDto(
    int RiotChampionId,
    string Name,
    string Title,
    string Description);