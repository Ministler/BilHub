using Microsoft.EntityFrameworkCore.Migrations;

namespace BilHub.Migrations
{
    public partial class JoinRequestDbContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JoinRequests_ProjectGroups_ProjectGroupId",
                table: "JoinRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_JoinRequests_ProjectGroups_ProjectGroupId",
                table: "JoinRequests",
                column: "ProjectGroupId",
                principalTable: "ProjectGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JoinRequests_ProjectGroups_ProjectGroupId",
                table: "JoinRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_JoinRequests_ProjectGroups_ProjectGroupId",
                table: "JoinRequests",
                column: "ProjectGroupId",
                principalTable: "ProjectGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
