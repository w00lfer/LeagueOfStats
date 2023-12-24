using LeagueOfStats.Domain.Common.Entities;
using NodaTime;

namespace LeagueOfStats.Domain.Matches;

public class Match : AggregateRoot
{
    public Match(string riotMatchId, Instant gameEndTimestamp) : base(Guid.NewGuid())
    {
        RiotMatchId = riotMatchId;
        GameEndTimestamp = gameEndTimestamp;
    }
    
    public string RiotMatchId { get; }
    
    public Instant GameEndTimestamp { get; }
}