using LeagueOfStats.Domain.Common.Entities;

namespace LeagueOfStats.Domain.Summoners;

public class SummonerName : ValueObject
{
    private SummonerName(string gameName, string tagLine)
    {
        GameName = gameName;
        TagLine = tagLine;
    }
    
    internal static SummonerName Create(string gameName, string tagLine) =>
        new(gameName, tagLine);
    
    public string GameName { get; }
    
    public string TagLine { get; }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return GameName;
        yield return TagLine;
    }

    public override string ToString() =>
        $"{GameName}#{TagLine}";
}