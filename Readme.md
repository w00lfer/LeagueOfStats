## Configuring Azure App Service WebApp

* Set **WEBSITES_PORT** application setting to **8080**

## Configuring AppSettings

* "KeyVaultURL": *string* - Url to AzureKeyVault
* "ManagedIdentityClientId": *string* - ManagedIdentityClientId used to connect to AzureKeyVault
* "DatabaseOptions"
    - "DatabaseConnectionString": *string* - connection string to database where password is argumented {0} like this.
    - "DatabaseAdminPassword": *string* - password to datbase in connection string
    - "EnablesSensitiveDataLogging": *boolean* - sensitive logging for ef core
    - "EnableDetailedErrors": *boolean* - detailed login for ef core
    - "CommandTimeout": *integer* - timeout for ef core

* "RiotApiKeyOptions"
    - RiotApiKey": *string* - ApiKey from RiotGames
      
* "EntityUpdateLockoutOptions"
    "SummonerUpdateLockout": *integer* - lockout in minutes to prevent extensive querying summoners

## Chron jobs

* Chron job every week to sync discounts :white_check_mark:
* Chron job every 2 week to sync patch data like new skins, new champions :white_check_mark:

## Features

Summoner
* Search summoner by name, taglinie, region :white_check_mark:
    - Id,
    - RiotAccountId
    - RiotSummonerId
    - Name
    - IconId
    - IconUrl
    - UniqueRiotId
    - Name (name#tagline)
    - Level
    - LastUpdated
    - When can be updated (Refresh)

* Search summoner by ResourceId :white_check_mark:
    - Id,
    - RiotAccountId
    - RiotSummonerId
    - Name
    - IconId
    - IconUrl
    - UniqueRiotId
    - Name (name#tagline)
    - Level
    - LastUpdated
    - When can be updated (Refresh)

* Search summoner's live game by ResourceId :white_check_mark:
    - Banned champs
    - Teams with players and champions
    - How many minutes in game
    - Type of game (5v5, aram)
    - When game has started
    - Custom, Normal, Tutorial
    - Map

* Refresh summoner's data by ResourceId
    - Schedules job to refresh summoner's champion masteries and details like icon, level etc

ChampionMasteries
* Summoner Champion Masteries by ResurceId :white_check_mark:
    - Lists all of masteries earned on champions for Summoner
    - Level of champ
    - ImageUrl
    - Number of points
    - Has earned chest in this split

SummonerMatchHistory
* MatchHistorySummary by ResourceId :white_check_mark: 
    - Shortened match history summary (default to 5 games) by daty and type of queue
    - Info about summoner
    - Info about summoners in game
    - Game version
    - How many minutes it longed
    - When started
    - When ended
    - Type of game (5v5, aram)
    - Custom, Normal, Tutorial
    - Map

* MatchHistory by ResourceId :white_check_mark:
    - Same as above but more in detail. More coming soon

SummonerChallenges
* Challenges :x:
    - Info about challenges in current split / season (and in older if data was persisted)

ChampionRotation
* ChampionRotation :x:
    - List of all persisted champion rotations
    - Date

* ChampionRotation by ResourceId :x:
    - Detailed champion rotation
    - Champ info
    - Date

Leagues
* Master's ladder per region

* GrandMaster's ladder per region :x:

* Cutoff for GrandMaster per region :x:

* Challenger's ladder per region :x:

* Cutooff for GrandMaster per region :x:

Discounts
* Discounts :white_check_mark:
    - List of Discoutns with dates

* Discounts by ResourceId :white_check_mark:
    - List of skins and champion on sale
