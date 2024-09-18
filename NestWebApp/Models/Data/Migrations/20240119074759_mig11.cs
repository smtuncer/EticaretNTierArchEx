using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NestWebApp.Migrations
{
    /// <inheritdoc />
    public partial class mig11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ButtonLink",
                table: "Slider");

            migrationBuilder.DropColumn(
                name: "ButtonText",
                table: "Slider");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Blog");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ButtonLink",
                table: "Slider",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ButtonText",
                table: "Slider",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Blog",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
