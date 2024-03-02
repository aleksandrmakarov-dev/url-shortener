using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class addcountycodebrowserandplatformrenamecountytocountryName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Navigation");

            migrationBuilder.AddColumn<string>(
                name: "Browser",
                table: "Navigation",
                type: "TEXT",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "Navigation",
                type: "TEXT",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryName",
                table: "Navigation",
                type: "TEXT",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Platform",
                table: "Navigation",
                type: "TEXT",
                maxLength: 64,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Browser",
                table: "Navigation");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "Navigation");

            migrationBuilder.DropColumn(
                name: "CountryName",
                table: "Navigation");

            migrationBuilder.DropColumn(
                name: "Platform",
                table: "Navigation");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Navigation",
                type: "TEXT",
                maxLength: 64,
                nullable: false,
                defaultValue: "");
        }
    }
}
