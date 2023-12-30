using LeagueOfStats.Domain.Common.Enums;
using LeagueOfStats.Domain.Matches.Participants;
using LeagueOfStats.Domain.Matches.Participants.Dtos;
using LeagueOfStats.Domain.Matches.Teams;
using LeagueOfStats.Domain.Matches.Teams.Dtos;
using NodaTime;

namespace LeagueOfStats.Domain.Matches;

public record AddMatchDto(
    string RiotMatchId, 
    string GameVersion,
    Duration GameDuration, 
    Instant GameStartTimeStamp,
    Instant GameEndTimestamp,
    GameMode GameMode,
    GameType GameType,
    Map Map,
    string PlatformId,
    Queue Queue,
    string? TournamentCode,
    IEnumerable<AddParticipantDto> AddParticipantDtos,
    IEnumerable<AddTeamDto> AddTeamDtos);