using LeagueOfStats.API.Options;
using LeagueOfStats.Application.Common;
using Microsoft.Extensions.Options;

namespace LeagueOfStats.API.Environments;

internal class EntityUpdateLockoutService : IEntityUpdateLockoutService
{
    private readonly int _summonerUpdateLockout;
    
    public EntityUpdateLockoutService(IOptions<EntityUpdateLockoutOptions> config)
    {
        _summonerUpdateLockout = config.Value.SummonerUpdateLockout;
    }

    public int GetSummonerUpdateLockoutInMinutes() => 
        _summonerUpdateLockout;
}