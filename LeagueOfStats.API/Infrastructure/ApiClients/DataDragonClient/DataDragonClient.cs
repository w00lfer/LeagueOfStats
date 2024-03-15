using LeagueOfStats.API.Common.Errors;
using LeagueOfStats.Application.ApiClients.DataDragonClient;
using LeagueOfStats.Domain.Common.Rails.Results;

namespace LeagueOfStats.API.Infrastructure.ApiClients.DataDragonClient;

public class DataDragonClient : IDataDragonClient
{
    private readonly HttpClient _httpClient;

    public DataDragonClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<IEnumerable<ChampionDto>>> GetChampionsAsync()
    {
        var dataDragonGetVersionsReponse = await _httpClient.GetFromJsonAsync<List<string>>("api/versions.json");

        if (dataDragonGetVersionsReponse is null || dataDragonGetVersionsReponse.Count == 0)
        {
            return new ApiError("Data Dragon can't be accessed.");
        }

        string currentVersion = dataDragonGetVersionsReponse.First();
        
        var communityDragonGetSkinsResponse = await _httpClient.GetFromJsonAsync<DataDragonChampionDto>($"cdn/{currentVersion}/data/en_US/champion.json");

        if (communityDragonGetSkinsResponse is null)
        {
            return new ApiError("Data Dragon can't be accessed.");
        }
        
        var championDtos = communityDragonGetSkinsResponse.ChampionDataConfigurationModels.Select(c =>
           new ChampionDto(
               int.Parse(c.Value.Id),
               c.Value.Name,
               c.Value.Title,
               c.Value.Description));

        return Result.Success(championDtos);
    }
}