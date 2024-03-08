using LeagueOfStats.Domain.Common.Entities;

namespace LeagueOfStats.Domain.Skins;

public class SkinChroma : Entity
{
    internal SkinChroma(
        int riotChromaId,
        string chromaPath,
        IEnumerable<string> colorAsStrings,
        Skin skin) : base(Guid.NewGuid())
    {
        Skin = skin;
        RiotChromaId = riotChromaId;
        ChromaPath = chromaPath;
        ColorsAsStringSeparatedByComma = string.Join(',', colorAsStrings);
    }

    private SkinChroma()
        : base(Guid.Empty)
    {
        
    }
    
    public Skin Skin { get; }
    
    public int RiotChromaId { get; }
    
    public string ChromaPath { get; }
    
    public string ColorsAsStringSeparatedByComma { get; }
}