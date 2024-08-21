using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BalootOlympicsTeamsApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTeamsAncChangeRequestEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_first_player_id",
                table: "confirmation_request");

            migrationBuilder.DropForeignKey(
                name: "fk_second_player_id",
                table: "confirmation_request");

            migrationBuilder.DropIndex(
                name: "IX_confirmation_request_second_player_national_id",
                table: "confirmation_request");

            migrationBuilder.DropColumn(
                name: "first_player_confirmed_at",
                table: "confirmation_request");

            migrationBuilder.DropColumn(
                name: "second_player_confirmation_sent_at",
                table: "confirmation_request");

            migrationBuilder.DropColumn(
                name: "second_player_national_id",
                table: "confirmation_request");

            migrationBuilder.RenameColumn(
                name: "national_id",
                table: "players",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "second_player_confirmed_at",
                table: "confirmation_request",
                newName: "confirmed_at");

            migrationBuilder.RenameColumn(
                name: "first_player_national_id",
                table: "confirmation_request",
                newName: "second_player_id");

            migrationBuilder.RenameColumn(
                name: "first_player_confirmation_sent_at",
                table: "confirmation_request",
                newName: "sent_at");

            migrationBuilder.RenameIndex(
                name: "IX_confirmation_request_first_player_national_id",
                table: "confirmation_request",
                newName: "IX_confirmation_request_second_player_id");

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "players",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "players",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "players",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "second_player_otp",
                table: "confirmation_request",
                type: "character varying(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(6)",
                oldMaxLength: 6,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "first_player_id",
                table: "confirmation_request",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("teams_pkey", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_players_TeamId",
                table: "players",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_confirmation_request_first_player_id",
                table: "confirmation_request",
                column: "first_player_id");

            migrationBuilder.AddForeignKey(
                name: "fk_first_player_id",
                table: "confirmation_request",
                column: "first_player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_second_player_id",
                table: "confirmation_request",
                column: "second_player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_players_teams_TeamId",
                table: "players",
                column: "TeamId",
                principalTable: "teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_first_player_id",
                table: "confirmation_request");

            migrationBuilder.DropForeignKey(
                name: "fk_second_player_id",
                table: "confirmation_request");

            migrationBuilder.DropForeignKey(
                name: "FK_players_teams_TeamId",
                table: "players");

            migrationBuilder.DropTable(
                name: "teams");

            migrationBuilder.DropIndex(
                name: "IX_players_TeamId",
                table: "players");

            migrationBuilder.DropIndex(
                name: "IX_confirmation_request_first_player_id",
                table: "confirmation_request");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "players");

            migrationBuilder.DropColumn(
                name: "city",
                table: "players");

            migrationBuilder.DropColumn(
                name: "name",
                table: "players");

            migrationBuilder.DropColumn(
                name: "first_player_id",
                table: "confirmation_request");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "players",
                newName: "national_id");

            migrationBuilder.RenameColumn(
                name: "sent_at",
                table: "confirmation_request",
                newName: "first_player_confirmation_sent_at");

            migrationBuilder.RenameColumn(
                name: "second_player_id",
                table: "confirmation_request",
                newName: "first_player_national_id");

            migrationBuilder.RenameColumn(
                name: "confirmed_at",
                table: "confirmation_request",
                newName: "second_player_confirmed_at");

            migrationBuilder.RenameIndex(
                name: "IX_confirmation_request_second_player_id",
                table: "confirmation_request",
                newName: "IX_confirmation_request_first_player_national_id");

            migrationBuilder.AlterColumn<string>(
                name: "second_player_otp",
                table: "confirmation_request",
                type: "character varying(6)",
                maxLength: 6,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(6)",
                oldMaxLength: 6);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "first_player_confirmed_at",
                table: "confirmation_request",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "second_player_confirmation_sent_at",
                table: "confirmation_request",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "second_player_national_id",
                table: "confirmation_request",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_confirmation_request_second_player_national_id",
                table: "confirmation_request",
                column: "second_player_national_id");

            migrationBuilder.AddForeignKey(
                name: "fk_first_player_id",
                table: "confirmation_request",
                column: "first_player_national_id",
                principalTable: "players",
                principalColumn: "national_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_second_player_id",
                table: "confirmation_request",
                column: "second_player_national_id",
                principalTable: "players",
                principalColumn: "national_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
