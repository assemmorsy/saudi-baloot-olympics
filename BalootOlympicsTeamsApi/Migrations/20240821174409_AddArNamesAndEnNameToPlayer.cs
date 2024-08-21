using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BalootOlympicsTeamsApi.Migrations
{
    /// <inheritdoc />
    public partial class AddArNamesAndEnNameToPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "players",
                newName: "name_en");

            migrationBuilder.AddColumn<string>(
                name: "name_ar",
                table: "players",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name_ar",
                table: "players");

            migrationBuilder.RenameColumn(
                name: "name_en",
                table: "players",
                newName: "name");
        }
    }
}
