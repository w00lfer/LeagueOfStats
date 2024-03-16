using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeagueOfStats.Infrastructure.ApplicationDbContexts.Migrations
{
    /// <inheritdoc />
    public partial class MoveSkinImageToValueObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UncenteredSplashUrl",
                table: "Skins",
                newName: "SkinImage_UncenteredSplashUrl");

            migrationBuilder.RenameColumn(
                name: "TileUrl",
                table: "Skins",
                newName: "SkinImage_TileUrl");

            migrationBuilder.RenameColumn(
                name: "SplashUrl",
                table: "Skins",
                newName: "SkinImage_SplashUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SkinImage_UncenteredSplashUrl",
                table: "Skins",
                newName: "UncenteredSplashUrl");

            migrationBuilder.RenameColumn(
                name: "SkinImage_TileUrl",
                table: "Skins",
                newName: "TileUrl");

            migrationBuilder.RenameColumn(
                name: "SkinImage_SplashUrl",
                table: "Skins",
                newName: "SplashUrl");
        }
    }
}
