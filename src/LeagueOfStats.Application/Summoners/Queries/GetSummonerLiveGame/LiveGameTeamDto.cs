using LeagueOfStats.Domain.Matches.Teams.Enums;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerLiveGame;

public record LiveGameTeamDto(
    IEnumerable<LiveGameTeamParticipantDto> Participants,
    Side Side);