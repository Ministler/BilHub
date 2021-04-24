using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BilHub.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LockDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CourseSemester = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupSizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Size = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupSizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupSizes_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PeerGradeAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    AssignmentDescriptionFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeerGradeAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PeerGradeAssignments_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionlessState = table.Column<bool>(type: "bit", nullable: false),
                    SectionNo = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sections_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionId = table.Column<int>(type: "int", nullable: false),
                    AssignmentDescriptionFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AcceptedTypes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxFileSizeInBytes = table.Column<int>(type: "int", nullable: false),
                    VisibilityOfSubmission = table.Column<bool>(type: "bit", nullable: false),
                    CanBeGradedByStudents = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignments_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionId = table.Column<int>(type: "int", nullable: false),
                    ConfirmationState = table.Column<bool>(type: "bit", nullable: false),
                    ConfirmedUserNumber = table.Column<int>(type: "int", nullable: false),
                    GroupSize = table.Column<int>(type: "int", nullable: false),
                    ProjectInformation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectGroups_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerificationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    SecondPasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    VerifiedStatus = table.Column<bool>(type: "bit", nullable: false),
                    LoginStatus = table.Column<bool>(type: "bit", nullable: false),
                    DarkModeStatus = table.Column<bool>(type: "bit", nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    SectionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MergeRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderGroupId = table.Column<int>(type: "int", nullable: false),
                    ReceiverGroupId = table.Column<int>(type: "int", nullable: false),
                    VotedStudents = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MergeRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MergeRequests_ProjectGroups_ReceiverGroupId",
                        column: x => x.ReceiverGroupId,
                        principalTable: "ProjectGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MergeRequests_ProjectGroups_SenderGroupId",
                        column: x => x.SenderGroupId,
                        principalTable: "ProjectGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AffiliatedAssignmentId = table.Column<int>(type: "int", nullable: true),
                    ProjectGroupId = table.Column<int>(type: "int", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Submissions_Assignments_AffiliatedAssignmentId",
                        column: x => x.AffiliatedAssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Submissions_ProjectGroups_ProjectGroupId",
                        column: x => x.ProjectGroupId,
                        principalTable: "ProjectGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseUser",
                columns: table => new
                {
                    InstructedCoursesId = table.Column<int>(type: "int", nullable: false),
                    InstructorsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseUser", x => new { x.InstructedCoursesId, x.InstructorsId });
                    table.ForeignKey(
                        name: "FK_CourseUser_Courses_InstructedCoursesId",
                        column: x => x.InstructedCoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseUser_Users_InstructorsId",
                        column: x => x.InstructorsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JoinRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProjectGroupId = table.Column<int>(type: "int", nullable: false),
                    AcceptedNumber = table.Column<int>(type: "int", nullable: false),
                    RejectedNumber = table.Column<int>(type: "int", nullable: false),
                    VotedStudents = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JoinRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JoinRequests_ProjectGroups_ProjectGroupId",
                        column: x => x.ProjectGroupId,
                        principalTable: "ProjectGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JoinRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PeerGrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReviewerId = table.Column<int>(type: "int", nullable: false),
                    RevieweeId = table.Column<int>(type: "int", nullable: false),
                    ProjectGroupId = table.Column<int>(type: "int", nullable: true),
                    MaxGrade = table.Column<double>(type: "float", nullable: false),
                    Grade = table.Column<double>(type: "float", nullable: false),
                    AdditionalComment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeerGrades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PeerGrades_ProjectGroups_ProjectGroupId",
                        column: x => x.ProjectGroupId,
                        principalTable: "ProjectGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PeerGrades_Users_RevieweeId",
                        column: x => x.RevieweeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PeerGrades_Users_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectGrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProjectGroupID = table.Column<int>(type: "int", nullable: false),
                    MaxGrade = table.Column<double>(type: "float", nullable: false),
                    Grade = table.Column<double>(type: "float", nullable: false),
                    AdditionalComment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectGrades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectGrades_ProjectGroups_ProjectGroupID",
                        column: x => x.ProjectGroupID,
                        principalTable: "ProjectGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectGrades_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectGroupUser",
                columns: table => new
                {
                    GroupMembersId = table.Column<int>(type: "int", nullable: false),
                    ProjectGroupsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectGroupUser", x => new { x.GroupMembersId, x.ProjectGroupsId });
                    table.ForeignKey(
                        name: "FK_ProjectGroupUser_ProjectGroups_ProjectGroupsId",
                        column: x => x.ProjectGroupsId,
                        principalTable: "ProjectGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectGroupUser_Users_GroupMembersId",
                        column: x => x.GroupMembersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentedUserId = table.Column<int>(type: "int", nullable: true),
                    SubmissionId = table.Column<int>(type: "int", nullable: false),
                    CommentText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GradeStatus = table.Column<bool>(type: "bit", nullable: false),
                    MaxGrade = table.Column<double>(type: "float", nullable: false),
                    Grade = table.Column<double>(type: "float", nullable: false),
                    FileAttachmentAvailability = table.Column<bool>(type: "bit", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Submissions_SubmissionId",
                        column: x => x.SubmissionId,
                        principalTable: "Submissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_CommentedUserId",
                        column: x => x.CommentedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_SectionId",
                table: "Assignments",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CommentedUserId",
                table: "Comments",
                column: "CommentedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_SubmissionId",
                table: "Comments",
                column: "SubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseUser_InstructorsId",
                table: "CourseUser",
                column: "InstructorsId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupSizes_CourseId",
                table: "GroupSizes",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_JoinRequests_ProjectGroupId",
                table: "JoinRequests",
                column: "ProjectGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_JoinRequests_UserId",
                table: "JoinRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MergeRequests_ReceiverGroupId",
                table: "MergeRequests",
                column: "ReceiverGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MergeRequests_SenderGroupId",
                table: "MergeRequests",
                column: "SenderGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PeerGradeAssignments_CourseId",
                table: "PeerGradeAssignments",
                column: "CourseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PeerGrades_ProjectGroupId",
                table: "PeerGrades",
                column: "ProjectGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PeerGrades_RevieweeId",
                table: "PeerGrades",
                column: "RevieweeId");

            migrationBuilder.CreateIndex(
                name: "IX_PeerGrades_ReviewerId",
                table: "PeerGrades",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectGrades_ProjectGroupID",
                table: "ProjectGrades",
                column: "ProjectGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectGrades_UserId",
                table: "ProjectGrades",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectGroups_SectionId",
                table: "ProjectGroups",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectGroupUser_ProjectGroupsId",
                table: "ProjectGroupUser",
                column: "ProjectGroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_CourseId",
                table: "Sections",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_AffiliatedAssignmentId",
                table: "Submissions",
                column: "AffiliatedAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_ProjectGroupId",
                table: "Submissions",
                column: "ProjectGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_SectionId",
                table: "Users",
                column: "SectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "CourseUser");

            migrationBuilder.DropTable(
                name: "GroupSizes");

            migrationBuilder.DropTable(
                name: "JoinRequests");

            migrationBuilder.DropTable(
                name: "MergeRequests");

            migrationBuilder.DropTable(
                name: "PeerGradeAssignments");

            migrationBuilder.DropTable(
                name: "PeerGrades");

            migrationBuilder.DropTable(
                name: "ProjectGrades");

            migrationBuilder.DropTable(
                name: "ProjectGroupUser");

            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "ProjectGroups");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "Courses");
        }
    }
}
