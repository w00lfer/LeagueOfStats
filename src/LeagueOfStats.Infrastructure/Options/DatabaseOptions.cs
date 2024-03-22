namespace LeagueOfStats.Infrastructure.Options;

public class DatabaseOptions
{
    public string DatabaseConnectionString { get; set; } = string.Empty;

    public bool EnablesSensitiveDataLogging { get; init; } = false;
    
    public bool EnableDetailedErrors { get; init; } = false;

    public int CommandTimeout { get; init; } = 10;
}