using LeagueOfStats.Application.Common.Validators;
using LeagueOfStats.Application.RiotClient;
using LeagueOfStats.Domain.Common.Enums;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Summoners;
using MediatR;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerByGameNameAndTagLineAndRegion;

public record GetSummonerByGameNameAndTagLineAndRegionQuery(
    string GameName,
    string TagLine,
    Region Region)
: IRequest<Result<SummonerDto>>;

public class GetSummonerByGameNameAndTagLineAndRegionQueryHandler : IRequestHandler<GetSummonerByGameNameAndTagLineAndRegionQuery, Result<SummonerDto>>
{
    private readonly IValidator<GetSummonerByGameNameAndTagLineAndRegionQuery> _getSummonerByGameNameAndTagLineAndRegionQueryValidator;
    private readonly IRiotClient _riotClient;
    private readonly ISummonerDomainService _summonerDomainService;

    public GetSummonerByGameNameAndTagLineAndRegionQueryHandler(
        IValidator<GetSummonerByGameNameAndTagLineAndRegionQuery> getSummonerByGameNameAndTagLineAndRegionQueryValidator,
        IRiotClient riotClient,
        ISummonerDomainService summonerDomainService)
    {
        _getSummonerByGameNameAndTagLineAndRegionQueryValidator = getSummonerByGameNameAndTagLineAndRegionQueryValidator;
        _riotClient = riotClient;
        _summonerDomainService = summonerDomainService;
    }

    public Task<Result<SummonerDto>> Handle(GetSummonerByGameNameAndTagLineAndRegionQuery query, CancellationToken cancellationToken) =>
        _getSummonerByGameNameAndTagLineAndRegionQueryValidator.ValidateAsyncTwo(query)
            .Bind(() => _riotClient.GetSummonerByGameNameAndTaglineAsync(query.GameName, query.TagLine, query.Region))
            .Bind(summonerFromRiotApi => _summonerDomainService.GetByPuuidAsync(summonerFromRiotApi.Puuid)
                .Match(
                    summoner => Task.FromResult(Result.Success(summoner)),
                    () => CreateSummonerUsingDataFromRiotApiAsync(summonerFromRiotApi, query.GameName, query.TagLine, query.Region)))
            .Map(MapToSummonerDto);

    private Task<Result<Summoner>> CreateSummonerUsingDataFromRiotApiAsync(
        Camille.RiotGames.SummonerV4.Summoner summonerFromRiotApi,
        string gameName,
        string tagLine,
        Region region) => 
            _riotClient.GetSummonerChampionMasteryByPuuid(summonerFromRiotApi.Puuid, region)
                .Bind(async summonerChampionMasteriesFromRiotApi =>
                {
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
                        summonerChampionMasteriesFromRiotApi.Select(c =>
                            new UpdateChampionMasteryDto(
                                (int)c.ChampionId,
                                c.ChampionLevel,
                                c.ChampionPoints,
                                c.ChampionPointsSinceLastLevel,
                                c.ChampionPointsUntilNextLevel,
                                c.ChestGranted,
                                c.LastPlayTime,
                                c.TokensEarned)));

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
            summoner.LastUpdated);
}