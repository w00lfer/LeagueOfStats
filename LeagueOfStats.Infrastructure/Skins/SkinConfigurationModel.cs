using System.Text.Json.Serialization;

namespace LeagueOfStats.Infrastructure.Skins;

public record SkinConfigurationModel(
    Dictionary<string, SkinDataConfigurationModel> SkinDataConfigurationModels);