using LeagueOfStats.Domain.Common.Entities;
using NodaTime;

namespace LeagueOfStats.Domain.Matches;

public class Match : AggregateRoot
{
    private readonly List<Guid> _summonerIds = new();
    
    internal Match(string riotMatchId, IEnumerable<Guid> summonerIds, Instant gameEndTimestamp) : base(Guid.NewGuid())
    {
        RiotMatchId = riotMatchId;
        GameEndTimestamp = gameEndTimestamp;
        _summonerIds = summonerIds.ToList();
    }
    
    public string RiotMatchId { get; }
    
    public Instant GameEndTimestamp { get; }
    
    public bool IsReadyToShow { get; }

    public List<Guid> SummonerIds => _summonerIds.ToList();
}