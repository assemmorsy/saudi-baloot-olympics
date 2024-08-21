using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BalootOlympicsTeamsApi.Migrations
{
    /// <inheritdoc />
    public partial class AddStateAndCommentToPlayersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_players_teams_TeamId",
                table: "players");

            migrationBuilder.DropColumn(
                name: "city",
                table: "players");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "teams",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                table: "players",
                newName: "team_id");

            migrationBuilder.RenameIndex(
                name: "IX_players_TeamId",
                table: "players",
                newName: "IX_players_team_id");

            migrationBuilder.AlterColumn<int>(
                name: "team_id",
                table: "players",
                type: "INT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "comment",
                table: "players",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "state",
                table: "players",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_players_teams_team_id",
                table: "players",
                column: "team_id",
                principalTable: "teams",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_players_teams_team_id",
                table: "players");

            migrationBuilder.DropColumn(
                name: "comment",
                table: "players");

            migrationBuilder.DropColumn(
                name: "state",
                table: "players");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "teams",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "team_id",
                table: "players",
                newName: "TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_players_team_id",
                table: "players",
                newName: "IX_players_TeamId");

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "players",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "players",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_players_teams_TeamId",
                table: "players",
                column: "TeamId",
                principalTable: "teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
