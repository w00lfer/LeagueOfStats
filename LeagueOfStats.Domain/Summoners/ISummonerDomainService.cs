using LeagueOfStats.Domain.Common.Rails.Results;

namespace LeagueOfStats.Domain.Summoners;

public interface ISummonerDomainService
{
    Task<Result<Summoner>> GetByIdAsyncTwo(Guid id);
    
    Task<Result<Summoner>> GetByPuuidAsync(string puuid);
    
    Task<Summoner> CreateAsync(CreateSummonerDto createSummonerDto);
    
    Task UpdateDetailsAsync(Summoner summoner, UpdateDetailsSummonerDto updateDetailsSummonerDto);
}