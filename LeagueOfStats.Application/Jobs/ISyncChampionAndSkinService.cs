using LeagueOfStats.Domain.Champions;

namespace LeagueOfStats.Application.Jobs;

public interface ISyncChampionAndSkinService
{
    Task SyncAsync();
}