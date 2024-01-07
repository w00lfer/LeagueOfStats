using LeagueOfStats.Application.Common;
using LeagueOfStats.Application.Common.Validators;
using LeagueOfStats.Application.RiotClient;
using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Enums;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Summoners;
using LeagueOfStats.Domain.Summoners.Dtos;
using MediatR;
using NodaTime;

namespace LeagueOfStats.Application.Summoners.Queries.SearchSummonerByGameNameAndTagLineAndRegion;

public record SearchSummonerByGameNameAndTagLineAndRegionQuery(
    string GameName,
    string TagLine,
    Region Region)
    : IRequest<Result<SummonerDto>>;

public class SearchSummonerByGameNameAndTagLineAndRegionQueryHandler
    : IRequestHandler<SearchSummonerByGameNameAndTagLineAndRegionQuery, Result<SummonerDto>>
{
    private readonly IValidator<SearchSummonerByGameNameAndTagLineAndRegionQuery> _searchSummonerByGameNameAndTagLineAndRegionQueryValidator;
    private readonly IRiotClient _riotClient;
    private readonly ISummonerDomainService _summonerDomainService;
    private readonly IEntityUpdateLockoutService _entityUpdateLockoutService;
    private readonly IChampionRepository _championRepository;

    public SearchSummonerByGameNameAndTagLineAndRegionQueryHandler(
        IValidator<SearchSummonerByGameNameAndTagLineAndRegionQuery> searchSummonerByGameNameAndTagLineAndRegionQueryValidator,
        IRiotClient riotClient,
        ISummonerDomainService summonerDomainService,
        IEntityUpdateLockoutService entityUpdateLockoutService,
        IChampionRepository championRepository)
    {
        _searchSummonerByGameNameAndTagLineAndRegionQueryValidator = searchSummonerByGameNameAndTagLineAndRegionQueryValidator;
        _riotClient = riotClient;
        _summonerDomainService = summonerDomainService;
        _entityUpdateLockoutService = entityUpdateLockoutService;
        _championRepository = championRepository;
    }

    public Task<Result<SummonerDto>> Handle(
        SearchSummonerByGameNameAndTagLineAndRegionQuery query,
        CancellationToken cancellationToken) =>
        _searchSummonerByGameNameAndTagLineAndRegionQueryValidator.ValidateAsync(query)
            .Bind(() => _riotClient.GetSummonerByGameNameAndTaglineAsync(query.GameName,
                query.TagLine,
                query.Region))
            .Bind(summonerFromRiotApi => _summonerDomainService.GetByPuuidAsync(summonerFromRiotApi.Puuid)
                .Match(
                    summoner => Task.FromResult(Result.Success(summoner)),
                    () => CreateSummonerUsingDataFromRiotApiAsync(summonerFromRiotApi,
                        query.GameName,
                        query.TagLine,
                        query.Region)))
            .Map(MapToSummonerDto);

    private Task<Result<Summoner>> CreateSummonerUsingDataFromRiotApiAsync(
        Camille.RiotGames.SummonerV4.Summoner summonerFromRiotApi,
        string gameName,
        string tagLine,
        Region region) => 
            _riotClient.GetSummonerChampionMasteryByPuuid(summonerFromRiotApi.Puuid, region)
                .Bind(async summonerChampionMasteriesFromRiotApi =>
                {
                    var champions = (await _championRepository.GetAllAsync()).ToList();
                    
                    var createSummonerDto = new CreateSummonerDto(
                        summonerFromRiotApi.Id,
                        summonerFromRiotApi.AccountId,
                        summonerFromRiotApi.Name,
                        summonerFromRiotApi.ProfileIconId,
                        summonerFromRiotApi.Puuid,
                        summonerFromRiotApi.SummonerLevel,
                        gameName,
                        tagLine,
                        region,
                        summonerChampionMasteriesFromRiotApi.Select(cm =>
                            new UpdateChampionMasteryDto(
                                champions.Single(c => c.RiotChampionId == (int)cm.ChampionId),
                                cm.ChampionLevel,
                                cm.ChampionPoints,
                                cm.ChampionPointsSinceLastLevel,
                                cm.ChampionPointsUntilNextLevel,
                                cm.ChestGranted,
                                cm.LastPlayTime,
                                cm.TokensEarned)));

                   var summoner = await _summonerDomainService.CreateAsync(createSummonerDto);

                   return Result.Success(summoner);
                });

    private SummonerDto MapToSummonerDto(Summoner summoner) => 
        new(
            summoner.Id,
            summoner.AccountId,
            summoner.SummonerId,
            summoner.Name,
            summoner.ProfileIconId,
            summoner.Puuid,
            summoner.SummonerLevel,
            summoner.SummonerName,
            summoner.LastUpdated,
            summoner.LastUpdated.Plus(Duration.FromMinutes(_entityUpdateLockoutService.GetSummonerUpdateLockoutInMinutes())));
}