using LanguageExt;
using LeagueOfStats.Application.RiotClient;
using LeagueOfStats.Domain.Common.Enums;
using LeagueOfStats.Domain.Common.Errors;
using LeagueOfStats.Domain.Summoners;

namespace LeagueOfStats.Application.Summoners;

public class SummonerApplicationService : ISummonerApplicationService
{
    private readonly ISummonerRepository _summonerRepository;
    private readonly IRiotClient _riotClient;

    public SummonerApplicationService(ISummonerRepository summonerRepository, IRiotClient riotClient)
    {
        _summonerRepository = summonerRepository;
        _riotClient = riotClient;
    }

    public Task<Either<Error, Summoner>> GetSummonerByGameNameAndTagLineAndRegion(string gameName, string tagLine, Region region) =>
        _riotClient.GetSummonerByGameNameAndTaglineAsync(gameName, tagLine, region)
            .BindAsync(async summonerFromRiotApi =>
            {
                Summoner? summoner = await _summonerRepository.GetByPuuid(summonerFromRiotApi.Puuid);

                if (summoner is null)
                {
                    summoner = new Summoner(
                        summonerFromRiotApi.Id,
                        summonerFromRiotApi.AccountId,
                        summonerFromRiotApi.Name,
                        summonerFromRiotApi.ProfileIconId,
                        summonerFromRiotApi.Puuid,
                        summonerFromRiotApi.SummonerLevel,
                        SummonerName.Create(gameName, tagLine),
                        region);
                    
                    await _summonerRepository.AddAsync(summoner);
                }
                else
                {
                    // not sure if name should be updated for now
                    summoner.Update(summonerFromRiotApi.ProfileIconId, summonerFromRiotApi.SummonerLevel);
                    await _summonerRepository.UpdateAsync(summoner);
                }

                return Either<Error, Summoner>.Right(summoner);
            });
    
    public async Task<Either<Error, Summoner>> GetSummonerById(Guid id)
    {
        Summoner? summoner = await _summonerRepository.GetByIdAsync(id);

        if (summoner is null)
        { 
            return new EntityNotFoundError($"Summoner with Id={id} does not exist.");
        }

        if (summoner.CanBeUpdated)
        {
            return await _riotClient.GetSummonerByPuuidAsync(summoner.Puuid, summoner.Region)
                .BindAsync(async summonerFromRiotApi =>
                {
                    summoner.Update(summonerFromRiotApi.ProfileIconId, summonerFromRiotApi.SummonerLevel);
                    await _summonerRepository.UpdateAsync(summoner);
                    
                    return Either<Error, Summoner>.Right(summoner); 
                });
        }

        return Either<Error, Summoner>.Right(summoner);
    }
}