using LeagueOfStats.Domain.Common.Entities;
using LeagueOfStats.Domain.Matches.Enums;
using LeagueOfStats.Domain.Matches.Participants;
using LeagueOfStats.Domain.Matches.Teams;
using NodaTime;

namespace LeagueOfStats.Domain.Matches;

public class Match : AggregateRoot
{
    private readonly List<Participant> _participants = new();
    private readonly List<Team> _teams = new();
    
    internal Match(
        AddMatchDto addMatchDto) 
        : base(Guid.NewGuid())
    {
        RiotMatchId = addMatchDto.RiotMatchId;
        GameEndTimestamp = addMatchDto.GameEndTimestamp;
        GameVersion = addMatchDto.GameVersion;
        GameDuration = addMatchDto.GameDuration;
        GameStartTimeStamp = addMatchDto.GameStartTimeStamp;
        GameMode = addMatchDto.GameMode;
        GameType = addMatchDto.GameType;
        Map = addMatchDto.Map;
        PlatformId = addMatchDto.PlatformId;
        Queue = addMatchDto.Queue;
        TournamentCode = addMatchDto.TournamentCode;
        
        _participants.AddRange(addMatchDto.AddParticipantDtos
            .Select(addParticipantDto => new Participant(addParticipantDto, this)));
        _teams.AddRange(addMatchDto.AddTeamDtos
            .Select(addTeamDto => new Team(addTeamDto, this)));
    }
    
    public string RiotMatchId { get; }
    
    public string GameVersion { get; }
    
    public Duration GameDuration { get; }
    
    public Instant GameStartTimeStamp { get; }
    
    public Instant GameEndTimestamp { get; }
    
    public GameMode GameMode { get; }
    
    public GameType GameType { get; }
    
    public Map Map { get; }
    
    public string PlatformId { get; }
    
    public Queue Queue { get; }
    
    public string? TournamentCode { get; }

    public List<Participant> Participants => _participants.ToList();
    
    public List<Team> Teams => _teams.ToList();
}