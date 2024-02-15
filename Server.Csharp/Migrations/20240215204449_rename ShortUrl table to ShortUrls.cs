using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Csharp.Migrations
{
    /// <inheritdoc />
    public partial class renameShortUrltabletoShortUrls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShortUrl_Users_UserId",
                table: "ShortUrl");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShortUrl",
                table: "ShortUrl");

            migrationBuilder.RenameTable(
                name: "ShortUrl",
                newName: "ShortUrls");

            migrationBuilder.RenameIndex(
                name: "IX_ShortUrl_UserId",
                table: "ShortUrls",
                newName: "IX_ShortUrls_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShortUrls",
                table: "ShortUrls",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShortUrls_Users_UserId",
                table: "ShortUrls",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShortUrls_Users_UserId",
                table: "ShortUrls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShortUrls",
                table: "ShortUrls");

            migrationBuilder.RenameTable(
                name: "ShortUrls",
                newName: "ShortUrl");

            migrationBuilder.RenameIndex(
                name: "IX_ShortUrls_UserId",
                table: "ShortUrl",
                newName: "IX_ShortUrl_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShortUrl",
                table: "ShortUrl",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShortUrl_Users_UserId",
                table: "ShortUrl",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
