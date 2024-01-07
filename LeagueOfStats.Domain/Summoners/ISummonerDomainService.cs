using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Summoners.Dtos;

namespace LeagueOfStats.Domain.Summoners;

public interface ISummonerDomainService
{
    Task<Result<Summoner>> GetByIdAsync(Guid id);

    Task<Result<Summoner>> GetByPuuidAsync(string puuid);

    Task<Summoner> CreateAsync(CreateSummonerDto createSummonerDto);

    Task<IEnumerable<Summoner>> CreateMultipleAsync(IEnumerable<CreateSummonerDto> createSummonerDtos);

    Task UpdateDetailsAsync(Summoner summoner, UpdateDetailsSummonerDto updateDetailsSummonerDto);
}