using LeagueOfStats.Domain.Common.Entities;

namespace LeagueOfStats.Domain.Skins;

public class Skin : AggregateRoot
{
    private readonly List<SkinChroma> _chromas = new();
    
    public Skin(AddSkinDto addSkinDto) : base(Guid.NewGuid())
    {
        RiotSkinId = addSkinDto.RiotSkinId;
        IsBase = addSkinDto.IsBase;
        Name = addSkinDto.Name;
        Description = addSkinDto.Description;
        SplashPath = addSkinDto.SplashPath;
        UncenteredSplashPath = addSkinDto.UncenteredSplashPath;
        TilePath = addSkinDto.TilePath;
        LoadScreenPath = addSkinDto.LoadScreenPath;
        LoadScreenVintagePath = addSkinDto.LoadScreenVintagePath;
        Rarity = addSkinDto.Rarity;
        IsLegacy = addSkinDto.IsLegacy;
        ChromaPath = addSkinDto.ChromaPath;
        
        _chromas.AddRange(addSkinDto.AddSkinChromaDtos.Select(addSkinChromaDto => new SkinChroma(
            addSkinChromaDto.RiotChromaId,
            addSkinDto.ChromaPath,
            addSkinChromaDto.ColorAsStrings,
            this)));
    }

    private Skin()
        : base(Guid.Empty)
    {
    }
    
    public int RiotSkinId { get; }
    
    public bool IsBase { get; }
    
    public string Name { get; }
    
    public string? Description { get; }
    
    public string SplashPath { get; }
    
    public string UncenteredSplashPath { get; }
    
    public string TilePath { get; }
    
    public string LoadScreenPath { get; }
    
    public string? LoadScreenVintagePath { get; }
    
    public string Rarity { get; }
    
    public bool IsLegacy { get; }
    
    public string? ChromaPath { get; }

    public List<SkinChroma> Chromas => _chromas;
}