using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Champions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RiotChampionId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChampionImage_FullFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChampionImage_Height = table.Column<int>(type: "int", nullable: false),
                    ChampionImage_SpriteFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChampionImage_Width = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Champions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RiotMatchId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GameVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GameDuration = table.Column<TimeSpan>(type: "time", nullable: false),
                    GameStartTimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GameEndTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GameMode = table.Column<int>(type: "int", nullable: false),
                    GameType = table.Column<int>(type: "int", nullable: false),
                    Map = table.Column<int>(type: "int", nullable: false),
                    PlatformId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TournamentCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Summoners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SummonerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Puuid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileIconId = table.Column<int>(type: "int", nullable: false),
                    SummonerLevel = table.Column<long>(type: "bigint", nullable: false),
                    Region = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SummonerName_GameName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SummonerName_TagLine = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Summoners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Side = table.Column<int>(type: "int", nullable: false),
                    Win = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChampionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SummonerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Assists = table.Column<int>(type: "int", nullable: false),
                    ChampLevel = table.Column<int>(type: "int", nullable: false),
                    DamageDealtToBuildings = table.Column<int>(type: "int", nullable: true),
                    DamageDealtToObjectives = table.Column<int>(type: "int", nullable: false),
                    DamageDealtToTurrets = table.Column<int>(type: "int", nullable: false),
                    DamageSelfMitigated = table.Column<int>(type: "int", nullable: false),
                    Deaths = table.Column<int>(type: "int", nullable: false),
                    DetectorWardsPlaced = table.Column<int>(type: "int", nullable: false),
                    DoubleKills = table.Column<int>(type: "int", nullable: false),
                    FirstBloodKill = table.Column<bool>(type: "bit", nullable: false),
                    FirstTowerKill = table.Column<bool>(type: "bit", nullable: false),
                    GameEndedInEarlySurrender = table.Column<bool>(type: "bit", nullable: false),
                    GameEndedInSurrender = table.Column<bool>(type: "bit", nullable: false),
                    GoldEarned = table.Column<int>(type: "int", nullable: false),
                    GoldSpent = table.Column<int>(type: "int", nullable: false),
                    Item0 = table.Column<int>(type: "int", nullable: false),
                    Item1 = table.Column<int>(type: "int", nullable: false),
                    Item2 = table.Column<int>(type: "int", nullable: false),
                    Item3 = table.Column<int>(type: "int", nullable: false),
                    Item4 = table.Column<int>(type: "int", nullable: false),
                    Item5 = table.Column<int>(type: "int", nullable: false),
                    Item6 = table.Column<int>(type: "int", nullable: false),
                    ItemsPurchased = table.Column<int>(type: "int", nullable: false),
                    KillingSprees = table.Column<int>(type: "int", nullable: false),
                    Kills = table.Column<int>(type: "int", nullable: false),
                    LargestCriticalStrike = table.Column<int>(type: "int", nullable: false),
                    LargestKillingSpree = table.Column<int>(type: "int", nullable: false),
                    LargestMultiKill = table.Column<int>(type: "int", nullable: false),
                    LongestTimeSpentLiving = table.Column<int>(type: "int", nullable: false),
                    MagicDamageDealt = table.Column<int>(type: "int", nullable: false),
                    MagicDamageDealtToChampions = table.Column<int>(type: "int", nullable: false),
                    MagicDamageTaken = table.Column<int>(type: "int", nullable: false),
                    NeutralMinionsKilled = table.Column<int>(type: "int", nullable: false),
                    NexusKills = table.Column<int>(type: "int", nullable: false),
                    ObjectivesStolen = table.Column<int>(type: "int", nullable: false),
                    PentaKills = table.Column<int>(type: "int", nullable: false),
                    PhysicalDamageDealt = table.Column<int>(type: "int", nullable: false),
                    PhysicalDamageDealtToChampions = table.Column<int>(type: "int", nullable: false),
                    PhysicalDamageTaken = table.Column<int>(type: "int", nullable: false),
                    Placement = table.Column<int>(type: "int", nullable: true),
                    PlayerAugment1 = table.Column<int>(type: "int", nullable: true),
                    PlayerAugment2 = table.Column<int>(type: "int", nullable: true),
                    PlayerAugment3 = table.Column<int>(type: "int", nullable: true),
                    PlayerAugment4 = table.Column<int>(type: "int", nullable: true),
                    PlayerSubteamId = table.Column<int>(type: "int", nullable: true),
                    QuadraKills = table.Column<int>(type: "int", nullable: false),
                    Spell1Casts = table.Column<int>(type: "int", nullable: false),
                    Spell2Casts = table.Column<int>(type: "int", nullable: false),
                    Spell3Casts = table.Column<int>(type: "int", nullable: false),
                    Spell4Casts = table.Column<int>(type: "int", nullable: false),
                    SubteamPlacement = table.Column<int>(type: "int", nullable: true),
                    Summoner1Casts = table.Column<int>(type: "int", nullable: false),
                    Summoner1Id = table.Column<int>(type: "int", nullable: false),
                    Summoner2Casts = table.Column<int>(type: "int", nullable: false),
                    Summoner2Id = table.Column<int>(type: "int", nullable: false),
                    TeamEarlySurrendered = table.Column<bool>(type: "bit", nullable: false),
                    Side = table.Column<int>(type: "int", nullable: false),
                    TeamPosition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeCCingOthers = table.Column<int>(type: "int", nullable: false),
                    TimePlayed = table.Column<int>(type: "int", nullable: false),
                    TotalDamageDealt = table.Column<int>(type: "int", nullable: false),
                    TotalDamageDealtToChampions = table.Column<int>(type: "int", nullable: false),
                    TotalDamageShieldedOnTeammates = table.Column<int>(type: "int", nullable: false),
                    TotalDamageTaken = table.Column<int>(type: "int", nullable: false),
                    TotalHeal = table.Column<int>(type: "int", nullable: false),
                    TotalHealsOnTeammates = table.Column<int>(type: "int", nullable: false),
                    TotalMinionsKilled = table.Column<int>(type: "int", nullable: false),
                    TotalTimeCCDealt = table.Column<int>(type: "int", nullable: false),
                    TotalTimeSpentDead = table.Column<int>(type: "int", nullable: false),
                    TotalUnitsHealed = table.Column<int>(type: "int", nullable: false),
                    TripleKills = table.Column<int>(type: "int", nullable: false),
                    TrueDamageDealt = table.Column<int>(type: "int", nullable: false),
                    TrueDamageDealtToChampions = table.Column<int>(type: "int", nullable: false),
                    TrueDamageTaken = table.Column<int>(type: "int", nullable: false),
                    TurretKills = table.Column<int>(type: "int", nullable: false),
                    TurretsLost = table.Column<int>(type: "int", nullable: true),
                    TurretTakedowns = table.Column<int>(type: "int", nullable: true),
                    VisionScore = table.Column<int>(type: "int", nullable: false),
                    VisionWardsBoughtInGame = table.Column<int>(type: "int", nullable: false),
                    WardsKilled = table.Column<int>(type: "int", nullable: false),
                    WardsPlaced = table.Column<int>(type: "int", nullable: false),
                    Win = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participants_Champions_ChampionId",
                        column: x => x.ChampionId,
                        principalTable: "Champions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participants_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participants_Summoners_SummonerId",
                        column: x => x.SummonerId,
                        principalTable: "Summoners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SummonerChampionMasteries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SummonerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChampionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChampionLevel = table.Column<int>(type: "int", nullable: false),
                    ChampionPoints = table.Column<int>(type: "int", nullable: false),
                    ChampionPointsSinceLastLevel = table.Column<long>(type: "bigint", nullable: false),
                    ChampionPointsUntilNextLevel = table.Column<long>(type: "bigint", nullable: false),
                    ChestGranted = table.Column<bool>(type: "bit", nullable: false),
                    LastPlayTime = table.Column<long>(type: "bigint", nullable: false),
                    TokensEarned = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SummonerChampionMasteries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SummonerChampionMasteries_Summoners_SummonerId",
                        column: x => x.SummonerId,
                        principalTable: "Summoners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChampionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PickTurn = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bans_Champions_ChampionId",
                        column: x => x.ChampionId,
                        principalTable: "Champions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bans_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamObjectives",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamObjectives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamObjectives_Teams_Id",
                        column: x => x.Id,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Perks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Perks_Participants_Id",
                        column: x => x.Id,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Objectives",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    First = table.Column<bool>(type: "bit", nullable: false),
                    Kills = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Objectives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Objectives_TeamObjectives_Id",
                        column: x => x.Id,
                        principalTable: "TeamObjectives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PerkStats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Defense = table.Column<int>(type: "int", nullable: false),
                    Flex = table.Column<int>(type: "int", nullable: false),
                    Offense = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerkStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerkStats_Perks_Id",
                        column: x => x.Id,
                        principalTable: "Perks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PerkStyles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PerksId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerkStyles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerkStyles_Perks_PerksId",
                        column: x => x.PerksId,
                        principalTable: "Perks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PerkStyleSelections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PerkStyleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Perk = table.Column<int>(type: "int", nullable: false),
                    Var1 = table.Column<int>(type: "int", nullable: false),
                    Var2 = table.Column<int>(type: "int", nullable: false),
                    Var3 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerkStyleSelections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerkStyleSelections_PerkStyles_PerkStyleId",
                        column: x => x.PerkStyleId,
                        principalTable: "PerkStyles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bans_ChampionId",
                table: "Bans",
                column: "ChampionId");

            migrationBuilder.CreateIndex(
                name: "IX_Bans_TeamId",
                table: "Bans",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_ChampionId",
                table: "Participants",
                column: "ChampionId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_MatchId",
                table: "Participants",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_SummonerId",
                table: "Participants",
                column: "SummonerId");

            migrationBuilder.CreateIndex(
                name: "IX_PerkStyles_PerksId",
                table: "PerkStyles",
                column: "PerksId");

            migrationBuilder.CreateIndex(
                name: "IX_PerkStyleSelections_PerkStyleId",
                table: "PerkStyleSelections",
                column: "PerkStyleId");

            migrationBuilder.CreateIndex(
                name: "IX_SummonerChampionMasteries_SummonerId",
                table: "SummonerChampionMasteries",
                column: "SummonerId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_MatchId",
                table: "Teams",
                column: "MatchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bans");

            migrationBuilder.DropTable(
                name: "Objectives");

            migrationBuilder.DropTable(
                name: "PerkStats");

            migrationBuilder.DropTable(
                name: "PerkStyleSelections");

            migrationBuilder.DropTable(
                name: "SummonerChampionMasteries");

            migrationBuilder.DropTable(
                name: "TeamObjectives");

            migrationBuilder.DropTable(
                name: "PerkStyles");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Perks");

            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "Champions");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Summoners");
        }
    }
}
