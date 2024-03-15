using LeagueOfStats.Application.ApiClients.RiotGamesShopClient;
using LeagueOfStats.Application.ApiClients.RiotGamesShopClient.Enums;
using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Discounts;
using LeagueOfStats.Domain.Skins;
using LeagueOfStats.Infrastructure.ApplicationDbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NodaTime;
using Quartz;

namespace LeagueOfStats.Application.Jobs;

[PersistJobDataAfterExecution]
public class SyncDiscountsDataJob : IJob
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IRiotGamesShopClient _riotGamesShopClient;
    private readonly IClock _clock;


    public SyncDiscountsDataJob(
        ApplicationDbContext applicationDbContext,
        IRiotGamesShopClient riotGamesShopClient,
        IClock clock)
    {
        _applicationDbContext = applicationDbContext;
        _riotGamesShopClient = riotGamesShopClient;
        _clock = clock;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        IDbContextTransaction transaction = await _applicationDbContext.Database.BeginTransactionAsync();
        
        try
        {
            var currentDateTime = _clock.GetCurrentInstant().InUtc().LocalDateTime;
            var isCurrentDiscountAlreadyPersisted = await _applicationDbContext.Discounts
                .AnyAsync(d => currentDateTime >= d.StartDateTime && currentDateTime <= d.EndDateTime);

            if (isCurrentDiscountAlreadyPersisted)
            {
                await transaction.CommitAsync();
                return;
            }
            
            await AddNewDiscount();
            
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
        }
    }

    private async Task AddNewDiscount()
    {
        Result<IEnumerable<RiotGamesShopDiscount>> getCurrentDiscountsResult = await _riotGamesShopClient.GetCurrentDiscountsAsync();

        if (getCurrentDiscountsResult.IsFailure)
        {
            throw new Exception("");
        }
        
        var listOfRiotgamesShopDiscounts = getCurrentDiscountsResult.Value.ToList();

        if (ValidateDiscountsStartDate(listOfRiotgamesShopDiscounts) is false)
        {
            throw new Exception("Discounted items should have same discount start date.");
        }

        if (ValidateDiscountsEndDate(listOfRiotgamesShopDiscounts) is false)
        {
            throw new Exception("Discounted items should have same discount start end.");
        }

        var champions = await _applicationDbContext.Champions.ToListAsync();
        if (ValidateChampionsInDiscountAreAllPersisted(champions, listOfRiotgamesShopDiscounts) is false)
        {
            throw new Exception("Discounted champions are different than persisted champions.");
        }

        var skins = await _applicationDbContext.Skins.ToListAsync();
        if (ValidateSkinsInDiscountAreAllPersisted(skins, listOfRiotgamesShopDiscounts) is false)
        {
            throw new Exception("Discounted skins are different than persisted skins.");
        }

        LocalDateTime discountStartDateTime = listOfRiotgamesShopDiscounts.Select(rd => rd.SalesStart).First();
        LocalDateTime discountEndDateTime = listOfRiotgamesShopDiscounts.Select(rd => rd.SalesEnd).First();

        var addDiscountedChampionDtos = listOfRiotgamesShopDiscounts
            .Where(rd => rd.DiscountType == DiscountType.Champion)
            .Select(rdc => new AddDiscountedChampionDto(
                champions.Single(c => c.RiotChampionId == int.Parse(rdc.Id)),
                rdc.OriginalPrice,
                rdc.DiscountedPrice));

        var addDiscountedSkinDtos = listOfRiotgamesShopDiscounts
            .Where(rd => rd.DiscountType == DiscountType.ChampionSkin)
            .Select(rds => new AddDiscountedSkinDto(
                skins.Single(c => c.RiotSkinId == int.Parse(rds.Id)),
                rds.OriginalPrice,
                rds.DiscountedPrice));

        AddDiscountDto addDiscountDto = new AddDiscountDto(
            discountStartDateTime,
            discountEndDateTime,
            addDiscountedChampionDtos,
            addDiscountedSkinDtos);

        var discount = new Discount(addDiscountDto);

        await _applicationDbContext.Discounts.AddAsync(discount);
        await _applicationDbContext.SaveChangesAsync();
    }
    
    private static bool ValidateSkinsInDiscountAreAllPersisted(List<Skin> skins, List<RiotGamesShopDiscount> listOfRiotgamesShopDiscounts)
    {
        var skinRiotIds = skins.Select(c => c.RiotSkinId);

        var allSkinIdsFromRiotGamesShopDiscountsMatchPersistedSkins = listOfRiotgamesShopDiscounts
            .Where(rd => rd.DiscountType == DiscountType.ChampionSkin)
            .All(rds => skinRiotIds.Contains(int.Parse(rds.Id)));
        return allSkinIdsFromRiotGamesShopDiscountsMatchPersistedSkins;
    }

    private static bool ValidateChampionsInDiscountAreAllPersisted(List<Champion> champions, List<RiotGamesShopDiscount> listOfRiotgamesShopDiscounts)
    {
        var championRiotIds = champions.Select(c => c.RiotChampionId);

        var allChampionIdsFromRiotGamesShopDiscountsMatchPersistedChampions = listOfRiotgamesShopDiscounts
            .Where(rd => rd.DiscountType == DiscountType.Champion)
            .All(rdc => championRiotIds.Contains(int.Parse(rdc.Id)));
        return allChampionIdsFromRiotGamesShopDiscountsMatchPersistedChampions;
    }

    private static bool ValidateDiscountsStartDate(List<RiotGamesShopDiscount> listOfRiotgamesShopDiscounts) => 
        listOfRiotgamesShopDiscounts.DistinctBy(rd => rd.SalesStart).Count() == 1;
    
    private static bool ValidateDiscountsEndDate(List<RiotGamesShopDiscount> listOfRiotgamesShopDiscounts) => 
        listOfRiotgamesShopDiscounts.DistinctBy(rd => rd.SalesEnd).Count() == 1;
}