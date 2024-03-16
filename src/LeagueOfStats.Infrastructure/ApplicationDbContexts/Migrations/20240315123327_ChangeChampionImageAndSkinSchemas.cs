using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Migrations
{
    /// <inheritdoc />
    public partial class ChangeChampionImageAndSkinSchemas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoadScreenPath",
                table: "Skins");

            migrationBuilder.DropColumn(
                name: "LoadScreenVintagePath",
                table: "Skins");

            migrationBuilder.DropColumn(
                name: "ChampionImage_Height",
                table: "Champions");

            migrationBuilder.DropColumn(
                name: "ChampionImage_Width",
                table: "Champions");

            migrationBuilder.RenameColumn(
                name: "UncenteredSplashPath",
                table: "Skins",
                newName: "UncenteredSplashUrl");

            migrationBuilder.RenameColumn(
                name: "TilePath",
                table: "Skins",
                newName: "TileUrl");

            migrationBuilder.RenameColumn(
                name: "SplashPath",
                table: "Skins",
                newName: "SplashUrl");

            migrationBuilder.RenameColumn(
                name: "ChampionImage_SpriteFileName",
                table: "Champions",
                newName: "ChampionImage_UncenteredSplashUrl");

            migrationBuilder.RenameColumn(
                name: "ChampionImage_FullFileName",
                table: "Champions",
                newName: "ChampionImage_TileUrl");

            migrationBuilder.AddColumn<string>(
                name: "ChampionImage_IconUrl",
                table: "Champions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ChampionImage_SplashUrl",
                table: "Champions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChampionImage_IconUrl",
                table: "Champions");

            migrationBuilder.DropColumn(
                name: "ChampionImage_SplashUrl",
                table: "Champions");

            migrationBuilder.RenameColumn(
                name: "UncenteredSplashUrl",
                table: "Skins",
                newName: "UncenteredSplashPath");

            migrationBuilder.RenameColumn(
                name: "TileUrl",
                table: "Skins",
                newName: "TilePath");

            migrationBuilder.RenameColumn(
                name: "SplashUrl",
                table: "Skins",
                newName: "SplashPath");

            migrationBuilder.RenameColumn(
                name: "ChampionImage_UncenteredSplashUrl",
                table: "Champions",
                newName: "ChampionImage_SpriteFileName");

            migrationBuilder.RenameColumn(
                name: "ChampionImage_TileUrl",
                table: "Champions",
                newName: "ChampionImage_FullFileName");

            migrationBuilder.AddColumn<string>(
                name: "LoadScreenPath",
                table: "Skins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LoadScreenVintagePath",
                table: "Skins",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChampionImage_Height",
                table: "Champions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ChampionImage_Width",
                table: "Champions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
