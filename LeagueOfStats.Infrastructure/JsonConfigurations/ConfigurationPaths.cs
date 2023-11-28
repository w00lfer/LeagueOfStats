namespace LeagueOfStats.Infrastructure.JsonConfigurations;

public static class ConfigurationPaths
{
    private const string ConfigurationsPath = "JsonConfigurations";
    private static readonly string CurrentDirectory = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;

    public static string GetChampionConfigurationPath() => Path.Combine(CurrentDirectory, ConfigurationsPath, "champion.json");
}