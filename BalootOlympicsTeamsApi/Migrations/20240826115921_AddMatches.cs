using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BalootOlympicsTeamsApi.Migrations
{
    /// <inheritdoc />
    public partial class AddMatches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "match",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    state = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    level = table.Column<int>(type: "integer", nullable: false),
                    TableNumber = table.Column<int>(type: "integer", nullable: false),
                    GroupId = table.Column<int>(type: "integer", nullable: false),
                    qydha_game_id = table.Column<Guid>(type: "uuid", nullable: true),
                    referee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    winner = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    UsTeamId = table.Column<int>(type: "integer", nullable: true),
                    MatchQualifyUsTeamId = table.Column<int>(type: "integer", nullable: true),
                    ThemTeamId = table.Column<int>(type: "integer", nullable: true),
                    MatchQualifyThemTeamId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("match_pkey", x => x.id);
                    table.ForeignKey(
                        name: "FK_match_groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_match_match_MatchQualifyThemTeamId",
                        column: x => x.MatchQualifyThemTeamId,
                        principalTable: "match",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_match_match_MatchQualifyUsTeamId",
                        column: x => x.MatchQualifyUsTeamId,
                        principalTable: "match",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_match_teams_ThemTeamId",
                        column: x => x.ThemTeamId,
                        principalTable: "teams",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_match_teams_UsTeamId",
                        column: x => x.UsTeamId,
                        principalTable: "teams",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_match_GroupId",
                table: "match",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_match_MatchQualifyThemTeamId",
                table: "match",
                column: "MatchQualifyThemTeamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_match_MatchQualifyUsTeamId",
                table: "match",
                column: "MatchQualifyUsTeamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_match_ThemTeamId",
                table: "match",
                column: "ThemTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_match_UsTeamId",
                table: "match",
                column: "UsTeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "match");
        }
    }
}
