using LeagueOfStats.Domain.Matches.Teams.Enums;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerMatchById;

public record MatchDetailsTeamDto(
    IEnumerable<MatchDetailsTeamParticipantDto> Participants,
    Side Side,
    bool Win);