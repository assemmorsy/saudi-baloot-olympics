using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BalootOlympicsTeamsApi.Migrations
{
    /// <inheritdoc />
    public partial class changeTableMatchToMatches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_match_groups_GroupId",
                table: "match");

            migrationBuilder.DropForeignKey(
                name: "FK_match_match_MatchQualifyThemTeamId",
                table: "match");

            migrationBuilder.DropForeignKey(
                name: "FK_match_match_MatchQualifyUsTeamId",
                table: "match");

            migrationBuilder.DropForeignKey(
                name: "FK_match_teams_ThemTeamId",
                table: "match");

            migrationBuilder.DropForeignKey(
                name: "FK_match_teams_UsTeamId",
                table: "match");

            migrationBuilder.RenameTable(
                name: "match",
                newName: "matches");

            migrationBuilder.RenameIndex(
                name: "IX_match_UsTeamId",
                table: "matches",
                newName: "IX_matches_UsTeamId");

            migrationBuilder.RenameIndex(
                name: "IX_match_ThemTeamId",
                table: "matches",
                newName: "IX_matches_ThemTeamId");

            migrationBuilder.RenameIndex(
                name: "IX_match_MatchQualifyUsTeamId",
                table: "matches",
                newName: "IX_matches_MatchQualifyUsTeamId");

            migrationBuilder.RenameIndex(
                name: "IX_match_MatchQualifyThemTeamId",
                table: "matches",
                newName: "IX_matches_MatchQualifyThemTeamId");

            migrationBuilder.RenameIndex(
                name: "IX_match_GroupId",
                table: "matches",
                newName: "IX_matches_GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_matches_groups_GroupId",
                table: "matches",
                column: "GroupId",
                principalTable: "groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_matches_matches_MatchQualifyThemTeamId",
                table: "matches",
                column: "MatchQualifyThemTeamId",
                principalTable: "matches",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_matches_matches_MatchQualifyUsTeamId",
                table: "matches",
                column: "MatchQualifyUsTeamId",
                principalTable: "matches",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_matches_teams_ThemTeamId",
                table: "matches",
                column: "ThemTeamId",
                principalTable: "teams",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_matches_teams_UsTeamId",
                table: "matches",
                column: "UsTeamId",
                principalTable: "teams",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_matches_groups_GroupId",
                table: "matches");

            migrationBuilder.DropForeignKey(
                name: "FK_matches_matches_MatchQualifyThemTeamId",
                table: "matches");

            migrationBuilder.DropForeignKey(
                name: "FK_matches_matches_MatchQualifyUsTeamId",
                table: "matches");

            migrationBuilder.DropForeignKey(
                name: "FK_matches_teams_ThemTeamId",
                table: "matches");

            migrationBuilder.DropForeignKey(
                name: "FK_matches_teams_UsTeamId",
                table: "matches");

            migrationBuilder.RenameTable(
                name: "matches",
                newName: "match");

            migrationBuilder.RenameIndex(
                name: "IX_matches_UsTeamId",
                table: "match",
                newName: "IX_match_UsTeamId");

            migrationBuilder.RenameIndex(
                name: "IX_matches_ThemTeamId",
                table: "match",
                newName: "IX_match_ThemTeamId");

            migrationBuilder.RenameIndex(
                name: "IX_matches_MatchQualifyUsTeamId",
                table: "match",
                newName: "IX_match_MatchQualifyUsTeamId");

            migrationBuilder.RenameIndex(
                name: "IX_matches_MatchQualifyThemTeamId",
                table: "match",
                newName: "IX_match_MatchQualifyThemTeamId");

            migrationBuilder.RenameIndex(
                name: "IX_matches_GroupId",
                table: "match",
                newName: "IX_match_GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_match_groups_GroupId",
                table: "match",
                column: "GroupId",
                principalTable: "groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_match_match_MatchQualifyThemTeamId",
                table: "match",
                column: "MatchQualifyThemTeamId",
                principalTable: "match",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_match_match_MatchQualifyUsTeamId",
                table: "match",
                column: "MatchQualifyUsTeamId",
                principalTable: "match",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_match_teams_ThemTeamId",
                table: "match",
                column: "ThemTeamId",
                principalTable: "teams",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_match_teams_UsTeamId",
                table: "match",
                column: "UsTeamId",
                principalTable: "teams",
                principalColumn: "id");
        }
    }
}
