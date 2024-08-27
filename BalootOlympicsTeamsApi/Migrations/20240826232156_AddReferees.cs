using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BalootOlympicsTeamsApi.Migrations
{
    /// <inheritdoc />
    public partial class AddReferees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "referees",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    phone = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("referees_pkey", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_matches_referee_id",
                table: "matches",
                column: "referee_id");

            migrationBuilder.CreateIndex(
                name: "player_phone_key1",
                table: "referees",
                column: "phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "player_username_key",
                table: "referees",
                column: "username",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_matches_referees_referee_id",
                table: "matches",
                column: "referee_id",
                principalTable: "referees",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_matches_referees_referee_id",
                table: "matches");

            migrationBuilder.DropTable(
                name: "referees");

            migrationBuilder.DropIndex(
                name: "IX_matches_referee_id",
                table: "matches");
        }
    }
}
