using LeagueOfStats.Application.Common.Errors;
using LeagueOfStats.Application.RiotGamesShopClient;
using LeagueOfStats.Application.RiotGamesShopClient.Enums;
using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Discounts;
using LeagueOfStats.Domain.Skins;
using MediatR;
using NodaTime;

namespace LeagueOfStats.Application.Discounts.Queries.GetDiscounts;

public record GetDiscountsQuery()
    : IRequest<Result<IEnumerable<DiscountSummaryDto>>>;

public class GetDiscountsQueryHandler 
    : IRequestHandler<GetDiscountsQuery, Result<IEnumerable<DiscountSummaryDto>>>
{
    private readonly IDiscountDomainService _discountDomainService;
    private readonly IClock _clock;
    private readonly IRiotGamesShopClient _riotGamesShopClient;
    private readonly IChampionRepository _championRepository;
    private readonly ISkinRepository _skinRepository;

    public GetDiscountsQueryHandler(
        IDiscountDomainService discountDomainService,
        IClock clock, 
        IRiotGamesShopClient riotGamesShopClient,
        IChampionRepository championRepository,
        ISkinRepository skinRepository)
    {
        _discountDomainService = discountDomainService;
        _clock = clock;
        _riotGamesShopClient = riotGamesShopClient;
        _championRepository = championRepository;
        _skinRepository = skinRepository;
    }

    public async Task<Result<IEnumerable<DiscountSummaryDto>>> Handle(GetDiscountsQuery request, CancellationToken cancellationToken)
    {
        List<Discount> discounts = (await _discountDomainService.GetAllAsync()).ToList();

        var currentDate = _clock.GetCurrentInstant().InUtc().LocalDateTime;
        if (discounts.SingleOrDefault(d => currentDate >= d.StartDateTime && currentDate <= d.EndDateTime) is not null)
        {
            return Result.Success(MapDiscountsToDiscountSummaries(discounts));
        }

        return await SyncCurrentDiscountAndReturnDiscounts(discounts)
            .Map(MapDiscountsToDiscountSummaries);
    }

    private Task<Result<IEnumerable<Discount>>> SyncCurrentDiscountAndReturnDiscounts(List<Discount> discounts) =>
        _riotGamesShopClient.GetCurrentDiscountsAsync()
            .Bind(async riotGamesShopDiscounts =>
            {
                var listOfRiotgamesShopDiscounts = riotGamesShopDiscounts.ToList();
                
                if (ValidateDiscountsStartDate(listOfRiotgamesShopDiscounts) is false)
                {
                    return new ApplicationError("Discounted items should have same discount start date.");
                }

                if (ValidateDiscountsEndDate(listOfRiotgamesShopDiscounts) is false)
                {
                    return new ApplicationError("Discounted items should have same discount start end.");
                }

                var champions = (await _championRepository.GetAllAsync()).ToList();
                if (ValidateChampionsInDiscountAreAllPersisted(champions, listOfRiotgamesShopDiscounts) is false)
                {
                    return new ApplicationError("Discounted champions are different than persisted champions.");
                }

                var skins = (await _skinRepository.GetAllAsync()).ToList();
                if (ValidateSkinsInDiscountAreAllPersisted(skins, listOfRiotgamesShopDiscounts) is false)
                {
                    return new ApplicationError("Discounted skins are different than persisted skins.");
                }

                LocalDateTime discountStartDateTime = listOfRiotgamesShopDiscounts.Select(rd => rd.SalesStart).First();
                LocalDateTime discountEndDateTime = listOfRiotgamesShopDiscounts.Select(rd => rd.SalesEnd).First();
                
                var addDiscountedChampionDtos = listOfRiotgamesShopDiscounts
                    .Where(rd => rd.DiscountType == DiscountType.Champion)
                    .Select(rdc => new AddDiscountedChampionDto(
                        champions.Single(c => c.RiotChampionId == Int32.Parse(rdc.Id)),
                        rdc.OriginalPrice,
                        rdc.DiscountedPrice));
                
                var addDiscountedSkinDtos = listOfRiotgamesShopDiscounts
                    .Where(rd => rd.DiscountType == DiscountType.ChampionSkin)
                    .Select(rds => new AddDiscountedSkinDto(
                        skins.Single(c => c.RiotSkinId == Int32.Parse(rds.Id)),
                        rds.OriginalPrice,
                        rds.DiscountedPrice));
                
                AddDiscountDto addDiscountDto = new AddDiscountDto(
                    discountStartDateTime,
                    discountEndDateTime,
                    addDiscountedChampionDtos,
                    addDiscountedSkinDtos);

                var addedDiscount = await _discountDomainService.AddAsync(addDiscountDto);

                discounts.Add(addedDiscount);

                return Result.Success<IEnumerable<Discount>>(discounts);
            });

    private static bool ValidateSkinsInDiscountAreAllPersisted(List<Skin> skins, List<RiotGamesShopDiscount> listOfRiotgamesShopDiscounts)
    {
        var skinRiotIds = skins.Select(c => c.RiotSkinId);

        var allSkinIdsFromRiotGamesShopDiscountsMatchPersistedSkins = listOfRiotgamesShopDiscounts
            .Where(rd => rd.DiscountType == DiscountType.ChampionSkin)
            .All(rds => skinRiotIds.Contains(Int32.Parse(rds.Id)));
        return allSkinIdsFromRiotGamesShopDiscountsMatchPersistedSkins;
    }

    private static bool ValidateChampionsInDiscountAreAllPersisted(List<Champion> champions, List<RiotGamesShopDiscount> listOfRiotgamesShopDiscounts)
    {
        var championRiotIds = champions.Select(c => c.RiotChampionId);

        var allChampionIdsFromRiotGamesShopDiscountsMatchPersistedChampions = listOfRiotgamesShopDiscounts
            .Where(rd => rd.DiscountType == DiscountType.Champion)
            .All(rdc => championRiotIds.Contains(Int32.Parse(rdc.Id)));
        return allChampionIdsFromRiotGamesShopDiscountsMatchPersistedChampions;
    }

    private static bool ValidateDiscountsStartDate(List<RiotGamesShopDiscount> listOfRiotgamesShopDiscounts) => 
        listOfRiotgamesShopDiscounts.DistinctBy(rd => rd.SalesStart).Count() == 1;
    
    private static bool ValidateDiscountsEndDate(List<RiotGamesShopDiscount> listOfRiotgamesShopDiscounts) => 
        listOfRiotgamesShopDiscounts.DistinctBy(rd => rd.SalesEnd).Count() == 1;


    private static IEnumerable<DiscountSummaryDto> MapDiscountsToDiscountSummaries(IEnumerable<Discount> discounts) =>
        discounts.Select(d => new DiscountSummaryDto(
            d.Id,
            d.StartDateTime,
            d.EndDateTime));
}