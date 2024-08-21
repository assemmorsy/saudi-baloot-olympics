using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BalootOlympicsTeamsApi.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "players",
                columns: table => new
                {
                    national_id = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("players_pkey", x => x.national_id);
                });

            migrationBuilder.CreateTable(
                name: "confirmation_request",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    first_player_national_id = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    first_player_otp = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    first_player_confirmation_sent_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    first_player_confirmed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    second_player_national_id = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    second_player_otp = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: true),
                    second_player_confirmation_sent_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    second_player_confirmed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("confirmation_request_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_first_player_id",
                        column: x => x.first_player_national_id,
                        principalTable: "players",
                        principalColumn: "national_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_second_player_id",
                        column: x => x.second_player_national_id,
                        principalTable: "players",
                        principalColumn: "national_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_confirmation_request_first_player_national_id",
                table: "confirmation_request",
                column: "first_player_national_id");

            migrationBuilder.CreateIndex(
                name: "IX_confirmation_request_second_player_national_id",
                table: "confirmation_request",
                column: "second_player_national_id");

            migrationBuilder.CreateIndex(
                name: "player_email_key",
                table: "players",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "player_phone_key",
                table: "players",
                column: "phone",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "confirmation_request");

            migrationBuilder.DropTable(
                name: "players");
        }
    }
}
