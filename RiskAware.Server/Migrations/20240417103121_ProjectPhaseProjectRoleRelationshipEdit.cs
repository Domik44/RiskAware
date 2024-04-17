using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskAware.Server.Migrations
{
    /// <inheritdoc />
    public partial class ProjectPhaseProjectRoleRelationshipEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPhases_ProjectRoles_ProjectRoleId",
                table: "ProjectPhases");

            migrationBuilder.DropIndex(
                name: "IX_ProjectPhases_ProjectRoleId",
                table: "ProjectPhases");

            migrationBuilder.DropColumn(
                name: "ProjectRoleId",
                table: "ProjectPhases");

            migrationBuilder.AddColumn<int>(
                name: "ProjectPhaseId",
                table: "ProjectRoles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRoles_ProjectPhaseId",
                table: "ProjectRoles",
                column: "ProjectPhaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectRoles_ProjectPhases_ProjectPhaseId",
                table: "ProjectRoles",
                column: "ProjectPhaseId",
                principalTable: "ProjectPhases",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectRoles_ProjectPhases_ProjectPhaseId",
                table: "ProjectRoles");

            migrationBuilder.DropIndex(
                name: "IX_ProjectRoles_ProjectPhaseId",
                table: "ProjectRoles");

            migrationBuilder.DropColumn(
                name: "ProjectPhaseId",
                table: "ProjectRoles");

            migrationBuilder.AddColumn<int>(
                name: "ProjectRoleId",
                table: "ProjectPhases",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPhases_ProjectRoleId",
                table: "ProjectPhases",
                column: "ProjectRoleId",
                unique: true,
                filter: "[ProjectRoleId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPhases_ProjectRoles_ProjectRoleId",
                table: "ProjectPhases",
                column: "ProjectRoleId",
                principalTable: "ProjectRoles",
                principalColumn: "Id");
        }
    }
}
