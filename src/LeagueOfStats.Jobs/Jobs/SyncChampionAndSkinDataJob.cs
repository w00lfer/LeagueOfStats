using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Common.Repositories;
using LeagueOfStats.Domain.Skins;
using LeagueOfStats.Jobs.ApiClients.CommunityDragonClient;
using LeagueOfStats.Jobs.ApiClients.DataDragonClient;
using LeagueOfStats.Jobs.Common;
using LeagueOfStats.Jobs.Common.Errors;
using Quartz;

namespace LeagueOfStats.Jobs.Jobs;

[PersistJobDataAfterExecution]
public class SyncChampionAndSkinDataJob : IJob
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IChampionRepository _championRepository;
    private readonly ISkinRepository _skinRepository;
    private readonly IDataDragonClient _dataDragonClient;
    private readonly ICommunityDragonClient _communityDragonClient;

    public SyncChampionAndSkinDataJob(
        IUnitOfWork unitOfWork,
        IChampionRepository championRepository,
        ISkinRepository skinRepository,
        IDataDragonClient dataDragonClient,
        ICommunityDragonClient communityDragonClient)
    {
        _unitOfWork = unitOfWork;
        _championRepository = championRepository;
        _skinRepository = skinRepository;
        _dataDragonClient = dataDragonClient;
        _communityDragonClient = communityDragonClient;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        using var transaction = _unitOfWork.BeginTransaction();
        
        try
        {
            var addAnyNewChampionsResult = await AddAnyNewChampions();
            if (addAnyNewChampionsResult.IsFailure)
            {
                transaction.Rollback();
                return;
            }

            var addAnyNewSkinsResult = await AddAnyNewSkins();
            if (addAnyNewSkinsResult.IsFailure)
            {
                transaction.Rollback();
                return;
            }
            
            await _unitOfWork.SaveChangesAsync();
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
        }
    }
    
    private async Task<Result> AddAnyNewChampions()
    {
        Result<IEnumerable<ChampionDto>> getChampionsResult = await _dataDragonClient.GetChampionsAsync();

        if (getChampionsResult.IsFailure)
        {
            return new JobError("Data Dragon can't be accessed.");
        }
        
        var existingChampions = (await _championRepository.GetAllAsync()).ToList();
        var existingChampionRiotIds = existingChampions.Select(c => c.RiotChampionId).ToList();

        var championDtosToAdd = getChampionsResult.Value.Where(c => existingChampionRiotIds.Contains(c.RiotChampionId) is false);
        
        var championsToAdd = championDtosToAdd.Select(c => new Champion(
            c.RiotChampionId,
            c.Name,
            c.Title,
            c.Description,
            RiotUrlBuilder.GetChampionSplashByRiotChampionId(c.RiotChampionId),
            RiotUrlBuilder.GetChampionUncenteredSplashByRiotChampionId(c.RiotChampionId),
            RiotUrlBuilder.GetChampionIconByRiotChampionId(c.RiotChampionId),
            RiotUrlBuilder.GetChampionTileByRiotChampionId(c.RiotChampionId)));

        await _championRepository.AddRangeAsync(championsToAdd);
        
        return Result.Success();
    }

    private async Task<Result> AddAnyNewSkins()
    {
        Result<IEnumerable<SkinDto>> getSkinsResult = await _communityDragonClient.GetSkinsAsync();

        if (getSkinsResult.IsFailure)
        {
            return new JobError("Community Dragon can't be accessed.");
        }
        
        var existingSkins = (await _skinRepository.GetAllAsync()).ToList();
        var existingSkinRiotIds = existingSkins.Select(c => c.RiotSkinId).ToList();

        var skinDtosToAdd = getSkinsResult.Value.Where(c => existingSkinRiotIds.Contains(c.RiotSkinId) is false);
        
        var skinsToAdd = skinDtosToAdd.Select(s =>
            new Skin(new AddSkinDto(
                s.RiotSkinId,
                s.IsBase,
                s.Name,
                s.Description,
                RiotUrlBuilder.GetChampionSkinSplashByRiotChampionId(s.RiotSkinId),
                RiotUrlBuilder.GetChampionSkinUncenteredSplashByRiotChampionId(s.RiotSkinId),
                RiotUrlBuilder.GetChampionSkinTileByRiotChampionId(s.RiotSkinId),
                s.Rarity,
                s.IsLegacy,
                s.ChromaPath,
                s.SkinChromaDtos.Select(sc => new AddSkinChromaDto(
                    sc.RiotChromaId,
                    sc.ChromaPath,
                    sc.ColorAsStrings)))));

        await _skinRepository.AddRangeAsync(skinsToAdd);

        return Result.Success();
    }
    
}