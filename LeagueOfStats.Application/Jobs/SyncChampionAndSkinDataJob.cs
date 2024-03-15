using LeagueOfStats.Application.ApiClients.CommunityDragonClient;
using LeagueOfStats.Application.ApiClients.DataDragonClient;
using LeagueOfStats.Application.Common.RiotUrls;
using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Skins;
using LeagueOfStats.Infrastructure.ApplicationDbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Quartz;

namespace LeagueOfStats.Application.Jobs;

[PersistJobDataAfterExecution]
public class SyncChampionAndSkinDataJob : IJob
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IDataDragonClient _dataDragonClient;
    private readonly ICommunityDragonClient _communityDragonClient;

    public SyncChampionAndSkinDataJob(
        ApplicationDbContext applicationDbContext,
        IDataDragonClient dataDragonClient,
        ICommunityDragonClient communityDragonClient)
    {
        _applicationDbContext = applicationDbContext;
        _dataDragonClient = dataDragonClient;
        _communityDragonClient = communityDragonClient;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        IDbContextTransaction transaction = await _applicationDbContext.Database.BeginTransactionAsync();
        
        try
        {
            await AddAnyNewChampions();

            await AddAnyNewSkins();
            
            await _applicationDbContext.SaveChangesAsync();
            
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
        }
    }
    
    private async Task AddAnyNewChampions()
    {
        Result<IEnumerable<ChampionDto>> getChampionsResult = await _dataDragonClient.GetChampionsAsync();

        if (getChampionsResult.IsFailure)
        {
            throw new Exception();
        }
        
        var existingChampions = await _applicationDbContext.Champions.ToListAsync();
        var existingChampionRiotIds = existingChampions.Select(c => c.RiotChampionId).ToList();

        var championDtosToAdd = getChampionsResult.Value.Where(c => existingChampionRiotIds.Contains(c.RiotChampionId) is false);
        
        var championsToAdd = championDtosToAdd.Select(c => new Champion(
            c.RiotChampionId,
            c.Name,
            c.Title,
            c.Description,
            ChampionImage.Create(
                RiotUrlBuilder.GetChampionSplashByRiotChampionId(c.RiotChampionId),
                RiotUrlBuilder.GetChampionUncenteredSplashByRiotChampionId(c.RiotChampionId),
                RiotUrlBuilder.GetChampionIconByRiotChampionId(c.RiotChampionId),
                RiotUrlBuilder.GetChampionTileByRiotChampionId(c.RiotChampionId))));

        await _applicationDbContext.Champions.AddRangeAsync(championsToAdd);
    }

    private async Task AddAnyNewSkins()
    {
        Result<IEnumerable<SkinDto>> getSkinsResult = await _communityDragonClient.GetSkinsAsync();

        if (getSkinsResult.IsFailure)
        {
            throw new Exception();
        }
        
        var existingSkins = await _applicationDbContext.Skins.ToListAsync();
        var existingSkinRiotIds = existingSkins.Select(c => c.RiotSkinId).ToList();

        var skinDtosToAdd = getSkinsResult.Value.Where(c => existingSkinRiotIds.Contains(c.RiotSkinId) is false);
        
        var skinsToAdd = skinDtosToAdd.Select(s =>
            new Skin(new AddSkinDto(
                s.RiotSkinId,
                s.IsBase,
                s.Name,
                s.Description,
                RiotUrlBuilder.GetChampionSkinSplashByRiotChampionId(s.RiotSkinId),
                RiotUrlBuilder.GetChampionUncenteredSplashByRiotChampionId(s.RiotSkinId),
                RiotUrlBuilder.GetChampionSkinTileByRiotChampionId(s.RiotSkinId),
                s.Rarity,
                s.IsLegacy,
                s.ChromaPath,
                s.SkinChromaDtos is null
                    ? Enumerable.Empty<AddSkinChromaDto>()
                    : s.SkinChromaDtos.Select(sc => new AddSkinChromaDto(
                        sc.RiotChromaId,
                        sc.ChromaPath,
                        sc.ColorAsStrings)))));

        await _applicationDbContext.Skins.AddRangeAsync(skinsToAdd);
    }
    
}