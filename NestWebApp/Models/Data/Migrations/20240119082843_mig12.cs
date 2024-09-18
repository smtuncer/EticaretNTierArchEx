using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NestWebApp.Migrations
{
    /// <inheritdoc />
    public partial class mig12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MailSettingsId",
                table: "MailSettings",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MailSettings",
                newName: "MailSettingsId");
        }
    }
}
