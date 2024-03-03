using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class navigationfieldsnotnullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Navigation_ShortUrls_ShortUrlId",
                table: "Navigation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Navigation",
                table: "Navigation");

            migrationBuilder.RenameTable(
                name: "Navigation",
                newName: "Navigations");

            migrationBuilder.RenameIndex(
                name: "IX_Navigation_ShortUrlId",
                table: "Navigations",
                newName: "IX_Navigations_ShortUrlId");

            migrationBuilder.AlterColumn<string>(
                name: "Platform",
                table: "Navigations",
                type: "TEXT",
                maxLength: 64,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "Navigations",
                type: "TEXT",
                maxLength: 16,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 16,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CountryName",
                table: "Navigations",
                type: "TEXT",
                maxLength: 64,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CountryCode",
                table: "Navigations",
                type: "TEXT",
                maxLength: 5,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 5,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Browser",
                table: "Navigations",
                type: "TEXT",
                maxLength: 64,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Navigations",
                table: "Navigations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Navigations_ShortUrls_ShortUrlId",
                table: "Navigations",
                column: "ShortUrlId",
                principalTable: "ShortUrls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Navigations_ShortUrls_ShortUrlId",
                table: "Navigations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Navigations",
                table: "Navigations");

            migrationBuilder.RenameTable(
                name: "Navigations",
                newName: "Navigation");

            migrationBuilder.RenameIndex(
                name: "IX_Navigations_ShortUrlId",
                table: "Navigation",
                newName: "IX_Navigation_ShortUrlId");

            migrationBuilder.AlterColumn<string>(
                name: "Platform",
                table: "Navigation",
                type: "TEXT",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "Navigation",
                type: "TEXT",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 16);

            migrationBuilder.AlterColumn<string>(
                name: "CountryName",
                table: "Navigation",
                type: "TEXT",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "CountryCode",
                table: "Navigation",
                type: "TEXT",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 5);

            migrationBuilder.AlterColumn<string>(
                name: "Browser",
                table: "Navigation",
                type: "TEXT",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 64);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Navigation",
                table: "Navigation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Navigation_ShortUrls_ShortUrlId",
                table: "Navigation",
                column: "ShortUrlId",
                principalTable: "ShortUrls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
