using Microsoft.EntityFrameworkCore.Migrations;

namespace BilHub.Data.Migrations
{
    public partial class testMigraton2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UnvotedJoinRequest",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    JoinRequestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnvotedJoinRequest", x => new { x.StudentId, x.JoinRequestId });
                    table.ForeignKey(
                        name: "FK_UnvotedJoinRequest_JoinRequest_JoinRequestId",
                        column: x => x.JoinRequestId,
                        principalTable: "JoinRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnvotedJoinRequest_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnvotedJoinRequest_JoinRequestId",
                table: "UnvotedJoinRequest",
                column: "JoinRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnvotedJoinRequest");
        }
    }
}
