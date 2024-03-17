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
        Rarity = addSkinDto.Rarity;
        IsLegacy = addSkinDto.IsLegacy;
        ChromaPath = addSkinDto.ChromaPath;
        SkinImage = SkinImage.Create(
            addSkinDto.SplashUrl,
            addSkinDto.UncenteredSplashUrl,
            addSkinDto.TileUrl);
        
        _chromas.AddRange(addSkinDto.AddSkinChromaDtos.Select(addSkinChromaDto => new SkinChroma(
            addSkinChromaDto.RiotChromaId,
            addSkinChromaDto.ChromaPath,
            addSkinChromaDto.ColorAsStrings,
            this)));
    }

    protected Skin()
        : base(Guid.Empty)
    {
    }
    
    public int RiotSkinId { get; }
    
    public bool IsBase { get; }
    
    public string Name { get; }
    
    public string? Description { get; }
    
    public string SplashUrl { get; }
    
    public string UncenteredSplashUrl { get; }
    
    public string TileUrl { get; }
    
    public string Rarity { get; }
    
    public bool IsLegacy { get; }
    
    public string? ChromaPath { get; }
    
    public SkinImage SkinImage { get; }

    public List<SkinChroma> Chromas => _chromas;
}