using LeagueOfStats.Domain.Matches.Teams.Enums;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerMatchHistory;

public record MatchHistorySummaryTeamDto(
    IEnumerable<MatchHistorySummaryTeamParticipantDto> Participants,
    Side Side,
    bool Win);