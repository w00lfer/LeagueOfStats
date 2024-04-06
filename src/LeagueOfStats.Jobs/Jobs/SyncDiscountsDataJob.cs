using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Common.Repositories;
using LeagueOfStats.Domain.Discounts;
using LeagueOfStats.Domain.Skins;
using LeagueOfStats.Jobs.ApiClients.RiotGamesShopClient;
using LeagueOfStats.Jobs.ApiClients.RiotGamesShopClient.Enums;
using LeagueOfStats.Jobs.Common.Errors;
using NodaTime;
using Quartz;

namespace LeagueOfStats.Jobs.Jobs;

[PersistJobDataAfterExecution]
public class SyncDiscountsDataJob : IJob
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRiotGamesShopClient _riotGamesShopClient;
    private readonly IDiscountRepository _discountRepository;
    private readonly IChampionRepository _championRepository;
    private readonly ISkinRepository _skinRepository;
    private readonly IClock _clock;

    public SyncDiscountsDataJob(
        IUnitOfWork unitOfWork,
        IRiotGamesShopClient riotGamesShopClient,
        IDiscountRepository discountRepository,
        IChampionRepository championRepository,
        ISkinRepository skinRepository,
        IClock clock)
    {
        _unitOfWork = unitOfWork;
        _riotGamesShopClient = riotGamesShopClient;
        _discountRepository = discountRepository;
        _championRepository = championRepository;
        _skinRepository = skinRepository;
        _clock = clock;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        using var transaction = _unitOfWork.BeginTransaction();
        
        try
        {
            var currentDateTime = _clock.GetCurrentInstant().InUtc().LocalDateTime;
            var isCurrentDiscountAlreadyPersisted = await _discountRepository.DoesDiscountInGivenLocalDateTimeExistAsync(currentDateTime);

            if (isCurrentDiscountAlreadyPersisted)
            {
                transaction.Commit();
                return;
            }
            
            var addNewDiscountResult = await AddNewDiscount();
            if (addNewDiscountResult.IsFailure)
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

    private async Task<Result> AddNewDiscount()
    {
        Result<IEnumerable<RiotGamesShopDiscountDto>> getCurrentDiscountsResult = await _riotGamesShopClient.GetCurrentDiscountsAsync();

        if (getCurrentDiscountsResult.IsFailure)
        {
            return new JobError("Riot Games Shop can't be accessed.");
        }

        if (getCurrentDiscountsResult.Value.Count() == 0)
        {
            return Result.Success();
        }
        
        var listOfRiotGamesShopDiscounts = getCurrentDiscountsResult.Value.ToList();

        if (ValidateDiscountsStartDate(listOfRiotGamesShopDiscounts) is false)
        {
            return new JobError("Discounted items should have same discount start date.");
        }

        if (ValidateDiscountsEndDate(listOfRiotGamesShopDiscounts) is false)
        {
            return new JobError("Discounted items should have same discount end date.");
        }

        var champions = (await _championRepository.GetAllAsync()).ToList();
        if (ValidateChampionsInDiscountAreAllPersisted(champions, listOfRiotGamesShopDiscounts) is false)
        {
            return new JobError("Discounted champions are different than persisted champions.");
        }

        var skins = (await _skinRepository.GetAllAsync()).ToList();
        if (ValidateSkinsInDiscountAreAllPersisted(skins, listOfRiotGamesShopDiscounts) is false)
        {
            return new JobError("Discounted skins are different than persisted skins.");
        }

        LocalDateTime discountStartDateTime = listOfRiotGamesShopDiscounts.Select(rd => rd.SalesStart).First();
        LocalDateTime discountEndDateTime = listOfRiotGamesShopDiscounts.Select(rd => rd.SalesEnd).First();

        var addDiscountedChampionDtos = listOfRiotGamesShopDiscounts
            .Where(rd => rd.DiscountType == DiscountType.Champion)
            .Select(rdc => new AddDiscountedChampionDto(
                champions.Single(c => c.RiotChampionId == rdc.RiotId),
                rdc.OriginalPrice,
                rdc.DiscountedPrice));

        var addDiscountedSkinDtos = listOfRiotGamesShopDiscounts
            .Where(rd => rd.DiscountType == DiscountType.ChampionSkin)
            .Select(rds => new AddDiscountedSkinDto(
                skins.Single(c => c.RiotSkinId == rds.RiotId),
                rds.OriginalPrice,
                rds.DiscountedPrice));

        AddDiscountDto addDiscountDto = new AddDiscountDto(
            discountStartDateTime,
            discountEndDateTime,
            addDiscountedChampionDtos,
            addDiscountedSkinDtos);

        var discount = new Discount(addDiscountDto);

        await _discountRepository.AddAsync(discount);
        return Result.Success();
    }
    
    private static bool ValidateSkinsInDiscountAreAllPersisted(List<Skin> skins, List<RiotGamesShopDiscountDto> listOfRiotgamesShopDiscounts)
    {
        var skinRiotIds = skins.Select(c => c.RiotSkinId);

        var allSkinIdsFromRiotGamesShopDiscountsMatchPersistedSkins = listOfRiotgamesShopDiscounts
            .Where(rd => rd.DiscountType == DiscountType.ChampionSkin)
            .All(rds => skinRiotIds.Contains(rds.RiotId));
        return allSkinIdsFromRiotGamesShopDiscountsMatchPersistedSkins;
    }

    private static bool ValidateChampionsInDiscountAreAllPersisted(List<Champion> champions, List<RiotGamesShopDiscountDto> listOfRiotgamesShopDiscounts)
    {
        var championRiotIds = champions.Select(c => c.RiotChampionId);

        var allChampionIdsFromRiotGamesShopDiscountsMatchPersistedChampions = listOfRiotgamesShopDiscounts
            .Where(rd => rd.DiscountType == DiscountType.Champion)
            .All(rdc => championRiotIds.Contains(rdc.RiotId));
        return allChampionIdsFromRiotGamesShopDiscountsMatchPersistedChampions;
    }

    private static bool ValidateDiscountsStartDate(List<RiotGamesShopDiscountDto> listOfRiotgamesShopDiscounts) => 
        listOfRiotgamesShopDiscounts.DistinctBy(rd => rd.SalesStart).Count() == 1;
    
    private static bool ValidateDiscountsEndDate(List<RiotGamesShopDiscountDto> listOfRiotgamesShopDiscounts) => 
        listOfRiotgamesShopDiscounts.DistinctBy(rd => rd.SalesEnd).Count() == 1;
}