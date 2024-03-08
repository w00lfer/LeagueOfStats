﻿// <auto-generated />
using System;
using System.Collections.Generic;
using LeagueOfStats.Infrastructure.ApplicationDbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240308215238_Add_skins_and_skinChromas")]
    partial class Add_skins_and_skinChromas
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LeagueOfStats.Domain.Champions.Champion", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RiotChampionId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ComplexProperty<Dictionary<string, object>>("ChampionImage", "LeagueOfStats.Domain.Champions.Champion.ChampionImage#ChampionImage", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("FullFileName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("Height")
                                .HasColumnType("int");

                            b1.Property<string>("SpriteFileName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("Width")
                                .HasColumnType("int");
                        });

                    b.HasKey("Id");

                    b.ToTable("Champions");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Matches.Match", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<TimeSpan>("GameDuration")
                        .HasColumnType("time");

                    b.Property<DateTime>("GameEndTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("GameMode")
                        .HasColumnType("int");

                    b.Property<DateTime>("GameStartTimeStamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("GameType")
                        .HasColumnType("int");

                    b.Property<string>("GameVersion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Map")
                        .HasColumnType("int");

                    b.Property<string>("PlatformId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RiotMatchId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TournamentCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Matches.Participants.Participant", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Assists")
                        .HasColumnType("int");

                    b.Property<int>("ChampLevel")
                        .HasColumnType("int");

                    b.Property<Guid>("ChampionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("DamageDealtToBuildings")
                        .HasColumnType("int");

                    b.Property<int>("DamageDealtToObjectives")
                        .HasColumnType("int");

                    b.Property<int>("DamageDealtToTurrets")
                        .HasColumnType("int");

                    b.Property<int>("DamageSelfMitigated")
                        .HasColumnType("int");

                    b.Property<int>("Deaths")
                        .HasColumnType("int");

                    b.Property<int>("DetectorWardsPlaced")
                        .HasColumnType("int");

                    b.Property<int>("DoubleKills")
                        .HasColumnType("int");

                    b.Property<bool>("FirstBloodKill")
                        .HasColumnType("bit");

                    b.Property<bool>("FirstTowerKill")
                        .HasColumnType("bit");

                    b.Property<bool>("GameEndedInEarlySurrender")
                        .HasColumnType("bit");

                    b.Property<bool>("GameEndedInSurrender")
                        .HasColumnType("bit");

                    b.Property<int>("GoldEarned")
                        .HasColumnType("int");

                    b.Property<int>("GoldSpent")
                        .HasColumnType("int");

                    b.Property<int>("Item0")
                        .HasColumnType("int");

                    b.Property<int>("Item1")
                        .HasColumnType("int");

                    b.Property<int>("Item2")
                        .HasColumnType("int");

                    b.Property<int>("Item3")
                        .HasColumnType("int");

                    b.Property<int>("Item4")
                        .HasColumnType("int");

                    b.Property<int>("Item5")
                        .HasColumnType("int");

                    b.Property<int>("Item6")
                        .HasColumnType("int");

                    b.Property<int>("ItemsPurchased")
                        .HasColumnType("int");

                    b.Property<int>("KillingSprees")
                        .HasColumnType("int");

                    b.Property<int>("Kills")
                        .HasColumnType("int");

                    b.Property<int>("LargestCriticalStrike")
                        .HasColumnType("int");

                    b.Property<int>("LargestKillingSpree")
                        .HasColumnType("int");

                    b.Property<int>("LargestMultiKill")
                        .HasColumnType("int");

                    b.Property<int>("LongestTimeSpentLiving")
                        .HasColumnType("int");

                    b.Property<int>("MagicDamageDealt")
                        .HasColumnType("int");

                    b.Property<int>("MagicDamageDealtToChampions")
                        .HasColumnType("int");

                    b.Property<int>("MagicDamageTaken")
                        .HasColumnType("int");

                    b.Property<Guid>("MatchId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("NeutralMinionsKilled")
                        .HasColumnType("int");

                    b.Property<int>("NexusKills")
                        .HasColumnType("int");

                    b.Property<int>("ObjectivesStolen")
                        .HasColumnType("int");

                    b.Property<int>("PentaKills")
                        .HasColumnType("int");

                    b.Property<int>("PhysicalDamageDealt")
                        .HasColumnType("int");

                    b.Property<int>("PhysicalDamageDealtToChampions")
                        .HasColumnType("int");

                    b.Property<int>("PhysicalDamageTaken")
                        .HasColumnType("int");

                    b.Property<int?>("Placement")
                        .HasColumnType("int");

                    b.Property<int?>("PlayerAugment1")
                        .HasColumnType("int");

                    b.Property<int?>("PlayerAugment2")
                        .HasColumnType("int");

                    b.Property<int?>("PlayerAugment3")
                        .HasColumnType("int");

                    b.Property<int?>("PlayerAugment4")
                        .HasColumnType("int");

                    b.Property<int?>("PlayerSubteamId")
                        .HasColumnType("int");

                    b.Property<int>("QuadraKills")
                        .HasColumnType("int");

                    b.Property<int>("Side")
                        .HasColumnType("int");

                    b.Property<int>("Spell1Casts")
                        .HasColumnType("int");

                    b.Property<int>("Spell2Casts")
                        .HasColumnType("int");

                    b.Property<int>("Spell3Casts")
                        .HasColumnType("int");

                    b.Property<int>("Spell4Casts")
                        .HasColumnType("int");

                    b.Property<int?>("SubteamPlacement")
                        .HasColumnType("int");

                    b.Property<int>("Summoner1Casts")
                        .HasColumnType("int");

                    b.Property<int>("Summoner1Id")
                        .HasColumnType("int");

                    b.Property<int>("Summoner2Casts")
                        .HasColumnType("int");

                    b.Property<int>("Summoner2Id")
                        .HasColumnType("int");

                    b.Property<Guid>("SummonerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("TeamEarlySurrendered")
                        .HasColumnType("bit");

                    b.Property<string>("TeamPosition")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TimeCCingOthers")
                        .HasColumnType("int");

                    b.Property<int>("TimePlayed")
                        .HasColumnType("int");

                    b.Property<int>("TotalDamageDealt")
                        .HasColumnType("int");

                    b.Property<int>("TotalDamageDealtToChampions")
                        .HasColumnType("int");

                    b.Property<int>("TotalDamageShieldedOnTeammates")
                        .HasColumnType("int");

                    b.Property<int>("TotalDamageTaken")
                        .HasColumnType("int");

                    b.Property<int>("TotalHeal")
                        .HasColumnType("int");

                    b.Property<int>("TotalHealsOnTeammates")
                        .HasColumnType("int");

                    b.Property<int>("TotalMinionsKilled")
                        .HasColumnType("int");

                    b.Property<int>("TotalTimeCCDealt")
                        .HasColumnType("int");

                    b.Property<int>("TotalTimeSpentDead")
                        .HasColumnType("int");

                    b.Property<int>("TotalUnitsHealed")
                        .HasColumnType("int");

                    b.Property<int>("TripleKills")
                        .HasColumnType("int");

                    b.Property<int>("TrueDamageDealt")
                        .HasColumnType("int");

                    b.Property<int>("TrueDamageDealtToChampions")
                        .HasColumnType("int");

                    b.Property<int>("TrueDamageTaken")
                        .HasColumnType("int");

                    b.Property<int>("TurretKills")
                        .HasColumnType("int");

                    b.Property<int?>("TurretTakedowns")
                        .HasColumnType("int");

                    b.Property<int?>("TurretsLost")
                        .HasColumnType("int");

                    b.Property<int>("VisionScore")
                        .HasColumnType("int");

                    b.Property<int>("VisionWardsBoughtInGame")
                        .HasColumnType("int");

                    b.Property<int>("WardsKilled")
                        .HasColumnType("int");

                    b.Property<int>("WardsPlaced")
                        .HasColumnType("int");

                    b.Property<bool>("Win")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ChampionId");

                    b.HasIndex("MatchId");

                    b.HasIndex("SummonerId");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Matches.Participants.PerkStats", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Defense")
                        .HasColumnType("int");

                    b.Property<int>("Flex")
                        .HasColumnType("int");

                    b.Property<int>("Offense")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("PerkStats");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Matches.Participants.PerkStyle", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PerksId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PerksId");

                    b.ToTable("PerkStyles");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Matches.Participants.PerkStyleSelection", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Perk")
                        .HasColumnType("int");

                    b.Property<Guid>("PerkStyleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Var1")
                        .HasColumnType("int");

                    b.Property<int>("Var2")
                        .HasColumnType("int");

                    b.Property<int>("Var3")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PerkStyleId");

                    b.ToTable("PerkStyleSelections");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Matches.Participants.Perks", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Perks");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Matches.Teams.Ban", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ChampionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("PickTurn")
                        .HasColumnType("int");

                    b.Property<Guid>("TeamId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ChampionId");

                    b.HasIndex("TeamId");

                    b.ToTable("Bans");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Matches.Teams.Objective", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("First")
                        .HasColumnType("bit");

                    b.Property<int>("Kills")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Objectives");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Matches.Teams.Objectives", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("TeamObjectives");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Matches.Teams.Team", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MatchId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Side")
                        .HasColumnType("int");

                    b.Property<bool>("Win")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("MatchId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Skins.Skin", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ChromaPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsBase")
                        .HasColumnType("bit");

                    b.Property<string>("LoadScreenPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LoadScreenVintagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rarity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RiotSkinId")
                        .HasColumnType("int");

                    b.Property<string>("SplashPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UncenteredSplashPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Skins");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Skins.SkinChroma", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ChromaPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ColorsAsStringSeparatedByComma")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RiotChromaId")
                        .HasColumnType("int");

                    b.Property<Guid>("SkinId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SkinId");

                    b.ToTable("SkinChromas");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Summoners.Summoner", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AccountId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProfileIconId")
                        .HasColumnType("int");

                    b.Property<string>("Puuid")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Region")
                        .HasColumnType("int");

                    b.Property<string>("SummonerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("SummonerLevel")
                        .HasColumnType("bigint");

                    b.ComplexProperty<Dictionary<string, object>>("SummonerName", "LeagueOfStats.Domain.Summoners.Summoner.SummonerName#SummonerName", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("GameName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("TagLine")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");
                        });

                    b.HasKey("Id");

                    b.ToTable("Summoners");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Summoners.SummonerChampionMastery", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ChampionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ChampionLevel")
                        .HasColumnType("int");

                    b.Property<int>("ChampionPoints")
                        .HasColumnType("int");

                    b.Property<long>("ChampionPointsSinceLastLevel")
                        .HasColumnType("bigint");

                    b.Property<long>("ChampionPointsUntilNextLevel")
                        .HasColumnType("bigint");

                    b.Property<bool>("ChestGranted")
                        .HasColumnType("bit");

                    b.Property<long>("LastPlayTime")
                        .HasColumnType("bigint");

                    b.Property<Guid>("SummonerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TokensEarned")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SummonerId");

                    b.ToTable("SummonerChampionMasteries");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Matches.Participants.Participant", b =>
                {
                    b.HasOne("LeagueOfStats.Domain.Champions.Champion", null)
                        .WithMany()
                        .HasForeignKey("ChampionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LeagueOfStats.Domain.Matches.Match", "Match")
                        .WithMany("Participants")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LeagueOfStats.Domain.Summoners.Summoner", null)
                        .WithMany()
                        .HasForeignKey("SummonerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Match");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Matches.Participants.PerkStats", b =>
                {
                    b.HasOne("LeagueOfStats.Domain.Matches.Participants.Perks", "Perks")
                        .WithOne("StatPerks")
                        .HasForeignKey("LeagueOfStats.Domain.Matches.Participants.PerkStats", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Perks");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Matches.Participants.PerkStyle", b =>
                {
                    b.HasOne("LeagueOfStats.Domain.Matches.Participants.Perks", "Perks")
                        .WithMany("Styles")
                        .HasForeignKey("PerksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Perks");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Matches.Participants.PerkStyleSelection", b =>
                {
                    b.HasOne("LeagueOfStats.Domain.Matches.Participants.PerkStyle", "PerkStyle")
                        .WithMany("Selections")
                        .HasForeignKey("PerkStyleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PerkStyle");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Matches.Participants.Perks", b =>
                {
                    b.HasOne("LeagueOfStats.Domain.Matches.Participants.Participant", "Participant")
                        .WithOne("Perks")
                        .HasForeignKey("LeagueOfStats.Domain.Matches.Participants.Perks", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Participant");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Matches.Teams.Ban", b =>
                {
                    b.HasOne("LeagueOfStats.Domain.Champions.Champion", null)
                        .WithMany()
                        .HasForeignKey("ChampionId");

                    b.HasOne("LeagueOfStats.Domain.Matches.Teams.Team", "Team")
                        .WithMany("Bans")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Team");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Matches.Teams.Objective", b =>
                {
                    b.HasOne("LeagueOfStats.Domain.Matches.Teams.Objectives", null)
                        .WithOne("TowerObjective")
                        .HasForeignKey("LeagueOfStats.Domain.Matches.Teams.Objective", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Matches.Teams.Objectives", b =>
                {
                    b.HasOne("LeagueOfStats.Domain.Matches.Teams.Team", "Team")
                        .WithOne("Objectives")
                        .HasForeignKey("LeagueOfStats.Domain.Matches.Teams.Objectives", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Team");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Matches.Teams.Team", b =>
                {
                    b.HasOne("LeagueOfStats.Domain.Matches.Match", "Match")
                        .WithMany("Teams")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Match");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Skins.SkinChroma", b =>
                {
                    b.HasOne("LeagueOfStats.Domain.Skins.Skin", "Skin")
                        .WithMany("Chromas")
                        .HasForeignKey("SkinId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Skin");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Summoners.SummonerChampionMastery", b =>
                {
                    b.HasOne("LeagueOfStats.Domain.Summoners.Summoner", "Summoner")
                        .WithMany("SummonerChampionMasteries")
                        .HasForeignKey("SummonerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Summoner");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Matches.Match", b =>
                {
                    b.Navigation("Participants");

                    b.Navigation("Teams");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Matches.Participants.Participant", b =>
                {
                    b.Navigation("Perks")
                        .IsRequired();
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Matches.Participants.PerkStyle", b =>
                {
                    b.Navigation("Selections");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Matches.Participants.Perks", b =>
                {
                    b.Navigation("StatPerks")
                        .IsRequired();

                    b.Navigation("Styles");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Matches.Teams.Objectives", b =>
                {
                    b.Navigation("TowerObjective")
                        .IsRequired();
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Matches.Teams.Team", b =>
                {
                    b.Navigation("Bans");

                    b.Navigation("Objectives")
                        .IsRequired();
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Skins.Skin", b =>
                {
                    b.Navigation("Chromas");
                });

            modelBuilder.Entity("LeagueOfStats.Domain.Summoners.Summoner", b =>
                {
                    b.Navigation("SummonerChampionMasteries");
                });
#pragma warning restore 612, 618
        }
    }
}
