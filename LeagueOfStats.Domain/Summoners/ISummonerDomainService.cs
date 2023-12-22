using LanguageExt;
using LeagueOfStats.Domain.Common.Rails.Errors;
using LeagueOfStats.Domain.Common.Rails.Results;

namespace LeagueOfStats.Domain.Summoners;

public interface ISummonerDomainService
{
    Task<Either<Error, Summoner>> GetByIdAsync(Guid id);
    
    Task<Result<Summoner>> GetByIdAsyncTwo(Guid id);
    
    Task<Either<Error, Summoner>> GetByPuuidAsync(string puuid);
    
    Task<Summoner> CreateAsync(CreateSummonerDto createSummonerDto);
    
    Task UpdateDetailsAsync(Summoner summoner, UpdateDetailsSummonerDto updateDetailsSummonerDto);
}