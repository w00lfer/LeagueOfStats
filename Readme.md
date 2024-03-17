## Configuring Azure App Service WebApp

* Set **WEBSITES_PORT** application setting to **8080**

## Chron jobs

* Chron job every week to sync discounts 
* Chron job every 2 week to sync patch data like new skins, new champions

## Features

Summoner
* Search summoner by name, taglinie, regionie
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

* Search summoner by ResourceId
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

* Search summoner's live game by ResourceId
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
* Summoner Champion Masteries by ResurceId
    - Lists all of masteries earned on champions for Summoner
    - Level of champ
    - ImageUrl
    - Number of points
    - Has earned chest in this split

SummonerMatchHistory
* MatchHistorySummary by ResourceId
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

* MatchHistory by ResourceId
    - Same as above but more in detail. More coming soon

SummonerChallenges
* Challenges
    - Info about challenges in current split / season (and in older if data was persisted)

ChampionRotation
* ChampionRotation
    - List of all persisted champion rotations
    - Date

* ChampionRotation by ResourceId
    - Detailed champion rotation
    - Champ info
    - Date

Leagues
* Master's ladder per region

* GrandMaster's ladder per region

* Cutoff for GrandMaster per region

* Challenger's ladder per region

* Cutooff for GrandMaster per region

Discounts
* Discounts
    - List of Discoutns with dates

* Discounts by ResourceId
    - List of skins and champion on sale
