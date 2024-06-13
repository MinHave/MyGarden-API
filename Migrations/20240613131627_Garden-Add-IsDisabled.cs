using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyGarden_API.Migrations
{
    /// <inheritdoc />
    public partial class GardenAddIsDisabled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "Gardens",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "Gardens");
        }
    }
}
