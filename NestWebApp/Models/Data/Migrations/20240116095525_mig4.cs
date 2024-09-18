using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NestWebApp.Migrations
{
    /// <inheritdoc />
    public partial class mig4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpeningTime",
                table: "Contact");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OpeningTime",
                table: "Contact",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
