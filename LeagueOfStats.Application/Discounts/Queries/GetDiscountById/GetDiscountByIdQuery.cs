using LeagueOfStats.Application.Common.Validators;
using LeagueOfStats.Application.Discounts.Queries.GetDiscountById.Dtos;
using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Discounts;
using LeagueOfStats.Domain.Skins;
using MediatR;

namespace LeagueOfStats.Application.Discounts.Queries.GetDiscountById;

public record GetDiscountByIdQuery(
    Guid Id)
    : IRequest<Result<DiscountDetailsDto>>;

public class GetDiscountByIdQueryHandler 
    : IRequestHandler<GetDiscountByIdQuery, Result<DiscountDetailsDto>>
{
    private readonly IValidator<GetDiscountByIdQuery> _getDiscountByIdQueryValidator;
    private readonly IDiscountDomainService _discountDomainService;
    private readonly IChampionRepository _championRepository;
    private readonly ISkinRepository _skinRepository;

    public GetDiscountByIdQueryHandler(
        IValidator<GetDiscountByIdQuery> getDiscountByIdQueryValidator,
        IDiscountDomainService discountDomainService,
        IChampionRepository championRepository,
        ISkinRepository skinRepository)
    {
        _getDiscountByIdQueryValidator = getDiscountByIdQueryValidator;
        _discountDomainService = discountDomainService;
        _championRepository = championRepository;
        _skinRepository = skinRepository;
    }

    public Task<Result<DiscountDetailsDto>> Handle(GetDiscountByIdQuery query, CancellationToken cancellationToken) =>
        _getDiscountByIdQueryValidator.ValidateAsync(query)
            .Bind(() => _discountDomainService.GetByIdAsync(query.Id))
            .Bind(async discount =>
            {
                var championIds = discount.DiscountedChampions.Select(dc => dc.ChampionId).ToArray();
                var championInDiscountsById = (await _championRepository.GetAllAsync(championIds))
                    .ToDictionary(c => c.Id, c => c);

                var skinIds = discount.DiscountedSkins.Select(ds => ds.SkinId).ToArray();
                var skinInDiscountsById = (await _skinRepository.GetAllAsync(skinIds))
                    .ToDictionary(c => c.Id, c => c);

                var discountDetailsDto = new DiscountDetailsDto(
                    discount.StartDateTime,
                    discount.EndDateTime,
                    discount.DiscountedChampions.Select(dc =>
                    {
                        var champion = championInDiscountsById[dc.ChampionId];
                        
                        return new DiscountedChampionDto(
                            champion.Name,
                            champion.ChampionImage.SplashUrl,
                            dc.OldPrice,
                            dc.NewPrice);
                    }),
                    discount.DiscountedSkins.Select(ds =>
                    {
                        var skin = skinInDiscountsById[ds.SkinId];

                        return new DiscountedSkinDto(
                            skin.Name,
                            skin.SplashUrl,
                            skin.Rarity,
                            ds.OldPrice,
                            ds.NewPrice);
                    }));
                
                return Result.Success(discountDetailsDto);
            });
}