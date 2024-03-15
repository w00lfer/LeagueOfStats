using LeagueOfStats.API.Common.Errors;
using LeagueOfStats.Application.ApiClients.CommunityDragonClient;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Jobs.ApiClients.CommunityDragonClient;

namespace LeagueOfStats.API.Infrastructure.ApiClients.CommunityDragonClient;

public class CommunityDragonClient : ICommunityDragonClient
{
    private readonly HttpClient _httpClient;

    public CommunityDragonClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<IEnumerable<SkinDto>>> GetSkinsAsync()
    {
        var communityDragonGetSkinsResponse = await _httpClient.GetFromJsonAsync<Dictionary<String, CommunityDragonSkinDataDto>>("skins.json");

        if (communityDragonGetSkinsResponse is null)
        {
            return new ApiError("Community Dragon can't be accessed.");
        }
        
        var skinDtos = communityDragonGetSkinsResponse.Values.Select(s =>
            new SkinDto(
                s.Id,
                s.IsBase,
                s.Name,
                s.Description,
                s.Rarity,
                s.IsLegacy,
                s.ChromaPath,
                s.SkinDataConfigurationChromas is null
                    ? Enumerable.Empty<SkinChromaDto>()
                    : s.SkinDataConfigurationChromas.Select(sc => new SkinChromaDto(
                        sc.Id,
                        sc.ChromaPath,
                        sc.Colors))));

        return Result.Success(skinDtos);
    }
}