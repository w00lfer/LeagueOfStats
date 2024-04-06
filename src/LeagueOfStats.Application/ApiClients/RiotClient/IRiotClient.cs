using Camille.RiotGames.ChampionMasteryV4;
using Camille.RiotGames.MatchV5;
using Camille.RiotGames.SpectatorV4;
using Camille.RiotGames.SummonerV4;
using LeagueOfStats.Domain.Common.Enums;
using LeagueOfStats.Domain.Common.Rails.Results;

namespace LeagueOfStats.Application.ApiClients.RiotClient;

public interface IRiotClient
{
    Task<Result<Summoner>> GetSummonerByPuuidAsync(
        string puuid,
        Region region);

    Task<Result<Summoner>> GetSummonerByGameNameAndTaglineAsync(
        string gameName,
        string tagLine,
        Region region);
        
    Task<Result<ChampionMastery[]>> GetSummonerChampionMasteryByPuuidAsync(
        string puuid,
        Region region);

    Task<Result<IEnumerable<Match>>> GetSummonerMatchHistorySummaryAsync(
        GetSummonerMatchHistoryDto getSummonerMatchHistoryDto);

    Task<Result<CurrentGameInfo>> GetSummonerLiveGameAsync(
        GetSummonerLiveGameDto getSummonerLiveGameDto);
}