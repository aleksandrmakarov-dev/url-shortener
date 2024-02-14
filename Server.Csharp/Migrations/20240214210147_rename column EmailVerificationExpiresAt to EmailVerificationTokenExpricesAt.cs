using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Csharp.Migrations
{
    /// <inheritdoc />
    public partial class renamecolumnEmailVerificationExpiresAttoEmailVerificationTokenExpricesAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmailVerificationExpiresAt",
                table: "Users",
                newName: "EmailVerificationTokenExpiresAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmailVerificationTokenExpiresAt",
                table: "Users",
                newName: "EmailVerificationExpiresAt");
        }
    }
}
