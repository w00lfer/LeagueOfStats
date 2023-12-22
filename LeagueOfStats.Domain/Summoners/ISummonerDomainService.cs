using LanguageExt;
using LeagueOfStats.Domain.Common.Rails.Errors;

namespace LeagueOfStats.Domain.Summoners;

public interface ISummonerDomainService
{
    Task<Either<Error, Summoner>> GetByIdAsync(Guid id);
    
    Task<Either<Error, Summoner>> GetByPuuidAsync(string puuid);
    
    Task<Summoner> CreateAsync(CreateSummonerDto createSummonerDto);
    
    Task UpdateDetailsAsync(Summoner summoner, UpdateDetailsSummonerDto updateDetailsSummonerDto);
}