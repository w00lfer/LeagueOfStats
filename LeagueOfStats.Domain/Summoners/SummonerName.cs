using LeagueOfStats.Domain.Common.Entities;

namespace LeagueOfStats.Domain.Summoners;

public class SummonerName : ValueObject
{
    internal static SummonerName Create(string gameName, string tagLine) =>
        new SummonerName(gameName, tagLine);
    
    public string GameName { get; }
    
    public string TagLine { get; }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }

    public override string ToString() =>
        $"{GameName}#{TagLine}";
    
    private SummonerName(string gameName, string tagLine)
    {
        GameName = gameName;
        TagLine = tagLine;
    }
}