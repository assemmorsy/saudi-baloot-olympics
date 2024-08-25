using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BalootOlympicsTeamsApi.Migrations
{
    /// <inheritdoc />
    public partial class AddGroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "teams",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "groups",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    check_in_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    start_play_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("groups_pkey", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_teams_GroupId",
                table: "teams",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_teams_groups_GroupId",
                table: "teams",
                column: "GroupId",
                principalTable: "groups",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_teams_groups_GroupId",
                table: "teams");

            migrationBuilder.DropTable(
                name: "groups");

            migrationBuilder.DropIndex(
                name: "IX_teams_GroupId",
                table: "teams");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "teams");
        }
    }
}
