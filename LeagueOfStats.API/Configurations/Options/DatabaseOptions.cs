namespace LeagueOfStats.API.Configurations.Options;

public class DatabaseOptions
{
    public string DatabaseConnectionString { get; init; } = string.Empty;

    public string DatabaseAdminPassword { get; init; } = string.Empty;
}