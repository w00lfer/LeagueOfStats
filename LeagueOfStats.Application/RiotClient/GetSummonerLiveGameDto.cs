using LeagueOfStats.Domain.Common.Enums;
using LeagueOfStats.Domain.Summoners;

namespace LeagueOfStats.Application.RiotClient;

public record GetSummonerLiveGameDto(
    SummonerName SummonerName,
    string RiotSummonerId,
    Region Region);