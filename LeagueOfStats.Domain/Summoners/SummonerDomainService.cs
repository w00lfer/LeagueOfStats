using LanguageExt;
using LeagueOfStats.Domain.Common.Rails.Errors;
using NodaTime;

namespace LeagueOfStats.Domain.Summoners;

public class SummonerDomainService : ISummonerDomainService
{
    private readonly ISummonerRepository _summonerRepository;
    private readonly IClock _clock;

    public SummonerDomainService(
        ISummonerRepository summonerRepository,
        IClock clock)
    {
        _summonerRepository = summonerRepository;
        _clock = clock;
    }

    public async Task<Either<Error, Summoner>> GetByIdAsync(Guid id)
    {
        Summoner? summoner = await _summonerRepository.GetByIdAsync(id);
        
        return summoner is not null
            ? summoner
            : new EntityNotFoundError($"Summoner with Id={id} does not exist.");
    }

    public async Task<Either<Error, Summoner>> GetByPuuidAsync(string puuid)
    {
        Summoner? summoner = await _summonerRepository.GetByPuuid(puuid);
        
        return summoner is not null
            ? summoner
            : new EntityNotFoundError($"Summoner with Puuid={puuid} does not exist.");
    }

    public async Task<Summoner> CreateAsync(CreateSummonerDto createSummonerDto)
    {
        var summoner = new Summoner(
            createSummonerDto.SummonerId,
            createSummonerDto.AccountId,
            createSummonerDto.Name,
            createSummonerDto.ProfileIconId,
            createSummonerDto.Puuid,
            createSummonerDto.SummonerLevel,
            SummonerName.Create(createSummonerDto.GameName, createSummonerDto.TagLine),
            createSummonerDto.Region,
            createSummonerDto.UpdateChampionMasteryDtos, _clock.GetCurrentInstant());
                    
        await _summonerRepository.AddAsync(summoner);

        return summoner;
    }
    
    public async Task UpdateDetailsAsync(Summoner summoner, UpdateDetailsSummonerDto updateDetailsSummonerDto)
    { 
        summoner.Update(updateDetailsSummonerDto.ProfileIconId, updateDetailsSummonerDto.SummonerLevel, updateDetailsSummonerDto.UpdateChampionMasteryDtos, _clock.GetCurrentInstant());
        await _summonerRepository.UpdateAsync(summoner);
    }
}