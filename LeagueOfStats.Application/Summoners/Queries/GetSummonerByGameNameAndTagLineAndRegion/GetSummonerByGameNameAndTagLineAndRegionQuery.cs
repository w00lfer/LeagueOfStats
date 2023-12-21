using LanguageExt;
using LeagueOfStats.Application.Common.Validators;
using LeagueOfStats.Application.RiotClient;
using LeagueOfStats.Domain.Common.Enums;
using LeagueOfStats.Domain.Common.Errors;
using LeagueOfStats.Domain.Summoners;
using MediatR;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerByGameNameAndTagLineAndRegion;

public record GetSummonerByGameNameAndTagLineAndRegionQuery(
    string GameName,
    string TagLine,
    Region Region)
: IRequest<Either<Error, SummonerDto>>;

public class GetSummonerByGameNameAndTagLineAndRegionQueryHandler : IRequestHandler<GetSummonerByGameNameAndTagLineAndRegionQuery, Either<Error, SummonerDto>>
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

    public Task<Either<Error, SummonerDto>> Handle(GetSummonerByGameNameAndTagLineAndRegionQuery query,
        CancellationToken cancellationToken) =>
        _getSummonerByGameNameAndTagLineAndRegionQueryValidator.ValidateAsync(query)
            .MatchAsync(
                error => error,
                () => _riotClient.GetSummonerByGameNameAndTaglineAsync(query.GameName, query.TagLine, query.Region)
                    .BindAsync(summonerFromRiotApi => _summonerDomainService.GetByPuuidAsync(summonerFromRiotApi.Puuid).ToAsync()
                        .MatchAsync(
                            summoner => Task.FromResult(Either<Error, Summoner>.Right(summoner)),
                            _ => CreateSummonerUsingDataFromRiotApiAsync(summonerFromRiotApi, query.GameName, query.TagLine, query.Region)))
                    .BindAsync(summoner => Either<Error, SummonerDto>.Right(MapToSummonerDto(summoner))));

    private Task<Either<Error, Summoner>> CreateSummonerUsingDataFromRiotApiAsync(
        Camille.RiotGames.SummonerV4.Summoner summonerFromRiotApi,
        string gameName,
        string tagLine,
        Region region) => 
            _riotClient.GetSummonerChampionMasteryByPuuid(summonerFromRiotApi.Puuid, region)
                .BindAsync(async summonerChampionMasteriesFromRiotApi =>
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

                    return Either<Error, Summoner>.Right(summoner);
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