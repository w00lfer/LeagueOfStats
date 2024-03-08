using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Migrations
{
    /// <inheritdoc />
    public partial class Add_skins_and_skinChromas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Skins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RiotSkinId = table.Column<int>(type: "int", nullable: false),
                    IsBase = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SplashPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UncenteredSplashPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoadScreenPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoadScreenVintagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rarity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChromaPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SkinChromas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkinId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RiotChromaId = table.Column<int>(type: "int", nullable: false),
                    ChromaPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ColorsAsStringSeparatedByComma = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkinChromas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkinChromas_Skins_SkinId",
                        column: x => x.SkinId,
                        principalTable: "Skins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SkinChromas_SkinId",
                table: "SkinChromas",
                column: "SkinId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SkinChromas");

            migrationBuilder.DropTable(
                name: "Skins");
        }
    }
}
