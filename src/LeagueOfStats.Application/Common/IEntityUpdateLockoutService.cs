namespace LeagueOfStats.Application.Common;

public interface IEntityUpdateLockoutService
{
    int GetSummonerUpdateLockoutInMinutes();
}