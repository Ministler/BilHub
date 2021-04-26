using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BilHub.Migrations
{
    public partial class Mig0 : Migration
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
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CourseSemester = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinGroupSize = table.Column<int>(type: "int", nullable: false),
                    MaxGroupSize = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
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
                    DarkModeStatus = table.Column<bool>(type: "bit", nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
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
                    AffiliatedCourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sections_Courses_AffiliatedCourseId",
                        column: x => x.AffiliatedCourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseUsers",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseUsers", x => new { x.UserId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_CourseUsers_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AffiliatedSectionId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    AssignmentDescriptionFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AcceptedTypes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxFileSizeInBytes = table.Column<int>(type: "int", nullable: false),
                    VisibilityOfSubmission = table.Column<bool>(type: "bit", nullable: false),
                    CanBeGradedByStudents = table.Column<bool>(type: "bit", nullable: false),
                    IsItGraded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignments_Sections_AffiliatedSectionId",
                        column: x => x.AffiliatedSectionId,
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
                    AffiliatedSectionId = table.Column<int>(type: "int", nullable: false),
                    AffiliatedCourseId = table.Column<int>(type: "int", nullable: false),
                    ConfirmationState = table.Column<bool>(type: "bit", nullable: false),
                    ConfirmedUserNumber = table.Column<int>(type: "int", nullable: false),
                    ProjectInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfirmedGroupMembers = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectGroups_Courses_AffiliatedCourseId",
                        column: x => x.AffiliatedCourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectGroups_Sections_AffiliatedSectionId",
                        column: x => x.AffiliatedSectionId,
                        principalTable: "Sections",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JoinRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestingStudentId = table.Column<int>(type: "int", nullable: false),
                    RequestedGroupId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AcceptedNumber = table.Column<int>(type: "int", nullable: false),
                    Accepted = table.Column<bool>(type: "bit", nullable: false),
                    Resolved = table.Column<bool>(type: "bit", nullable: false),
                    VotedStudents = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JoinRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JoinRequests_ProjectGroups_RequestedGroupId",
                        column: x => x.RequestedGroupId,
                        principalTable: "ProjectGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JoinRequests_Users_RequestingStudentId",
                        column: x => x.RequestingStudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MergeRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderGroupId = table.Column<int>(type: "int", nullable: false),
                    ReceiverGroupId = table.Column<int>(type: "int", nullable: false),
                    VotedStudents = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Accepted = table.Column<bool>(type: "bit", nullable: false),
                    Resolved = table.Column<bool>(type: "bit", nullable: false)
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
                name: "PeerGrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AffiliatedSectionID = table.Column<int>(type: "int", nullable: false),
                    ReviewerId = table.Column<int>(type: "int", nullable: false),
                    RevieweeId = table.Column<int>(type: "int", nullable: false),
                    ProjectGroupId = table.Column<int>(type: "int", nullable: true),
                    MaxGrade = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Grade = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                        name: "FK_PeerGrades_Sections_AffiliatedSectionID",
                        column: x => x.AffiliatedSectionID,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectGrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradingUserId = table.Column<int>(type: "int", nullable: false),
                    GradedProjectGroupID = table.Column<int>(type: "int", nullable: false),
                    MaxGrade = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Grade = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectGrades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectGrades_ProjectGroups_GradedProjectGroupID",
                        column: x => x.GradedProjectGroupID,
                        principalTable: "ProjectGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectGrades_Users_GradingUserId",
                        column: x => x.GradingUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectGroupUsers",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProjectGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectGroupUsers", x => new { x.UserId, x.ProjectGroupId });
                    table.ForeignKey(
                        name: "FK_ProjectGroupUsers_ProjectGroups_ProjectGroupId",
                        column: x => x.ProjectGroupId,
                        principalTable: "ProjectGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectGroupUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AffiliatedAssignmentId = table.Column<int>(type: "int", nullable: false),
                    AffiliatedGroupId = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasSubmission = table.Column<bool>(type: "bit", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    SectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Submissions_Assignments_AffiliatedAssignmentId",
                        column: x => x.AffiliatedAssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Submissions_ProjectGroups_AffiliatedGroupId",
                        column: x => x.AffiliatedGroupId,
                        principalTable: "ProjectGroups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentedUserId = table.Column<int>(type: "int", nullable: false),
                    CommentedSubmissionId = table.Column<int>(type: "int", nullable: false),
                    CommentText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxGrade = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Grade = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileAttachmentAvailability = table.Column<bool>(type: "bit", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Submissions_CommentedSubmissionId",
                        column: x => x.CommentedSubmissionId,
                        principalTable: "Submissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_CommentedUserId",
                        column: x => x.CommentedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CourseInformation", "CourseSemester", "EndDate", "LockDate", "MaxGroupSize", "MinGroupSize", "Name", "StartDate" },
                values: new object[,]
                {
                    { 1, "Object Oriented Software Engineering", "spring", new DateTime(2021, 5, 15, 7, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 4, 27, 0, 0, 0, 0, DateTimeKind.Local), 6, 5, "CS 319", new DateTime(2021, 2, 15, 7, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Programming Languages", "spring", new DateTime(2021, 5, 15, 7, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 4, 27, 0, 0, 0, 0, DateTimeKind.Local), 3, 2, "CS 315", new DateTime(2021, 2, 15, 7, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Data Structures and Algorithms", "spring", new DateTime(2021, 5, 15, 7, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 4, 27, 0, 0, 0, 0, DateTimeKind.Local), 1, 1, "CS 202", new DateTime(2021, 2, 15, 7, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Computer Organization and Architecture", "spring", new DateTime(2021, 5, 15, 7, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 4, 27, 0, 0, 0, 0, DateTimeKind.Local), 3, 2, "CS 224", new DateTime(2021, 2, 15, 7, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "History", "spring", new DateTime(2021, 5, 15, 7, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 4, 27, 0, 0, 0, 0, DateTimeKind.Local), 5, 4, "Hist 200", new DateTime(2021, 2, 15, 7, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "Calculus", "spring", new DateTime(2021, 5, 15, 7, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 4, 27, 0, 0, 0, 0, DateTimeKind.Local), 1, 1, "Math 101", new DateTime(2021, 2, 15, 7, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DarkModeStatus", "Email", "Name", "PasswordHash", "PasswordSalt", "SecondPasswordHash", "UserType", "VerificationCode", "VerifiedStatus" },
                values: new object[,]
                {
                    { 15, false, "aynur@dayanik", "Aynur Dayanik", new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, new byte[] { 136, 71, 198, 175, 194, 249, 214, 155, 213, 112, 27, 133, 137, 182, 74, 97, 1, 133, 25, 11, 38, 169, 207, 146, 52, 74, 126, 69, 189, 237, 210, 155, 35, 234, 181, 45, 218, 246, 47, 168, 40, 39, 102, 96, 52, 148, 22, 147, 3, 214, 189, 91, 208, 82, 113, 237, 26, 133, 59, 13, 153, 137, 101, 86, 156, 26, 147, 45, 109, 35, 165, 178, 199, 101, 150, 99, 153, 255, 88, 167, 207, 3, 185, 188, 185, 164, 112, 216, 103, 152, 205, 172, 98, 214, 78, 153, 56, 56, 73, 95, 179, 87, 40, 133, 16, 133, 37, 152, 211, 167, 166, 80, 156, 115, 57, 15, 61, 64, 13, 111, 188, 116, 14, 52, 221, 69, 31, 239 }, new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, 1, "31", true },
                    { 16, false, "erdem@Tuna", "Erdem Tuna", new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, new byte[] { 136, 71, 198, 175, 194, 249, 214, 155, 213, 112, 27, 133, 137, 182, 74, 97, 1, 133, 25, 11, 38, 169, 207, 146, 52, 74, 126, 69, 189, 237, 210, 155, 35, 234, 181, 45, 218, 246, 47, 168, 40, 39, 102, 96, 52, 148, 22, 147, 3, 214, 189, 91, 208, 82, 113, 237, 26, 133, 59, 13, 153, 137, 101, 86, 156, 26, 147, 45, 109, 35, 165, 178, 199, 101, 150, 99, 153, 255, 88, 167, 207, 3, 185, 188, 185, 164, 112, 216, 103, 152, 205, 172, 98, 214, 78, 153, 56, 56, 73, 95, 179, 87, 40, 133, 16, 133, 37, 152, 211, 167, 166, 80, 156, 115, 57, 15, 61, 64, 13, 111, 188, 116, 14, 52, 221, 69, 31, 239 }, new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, 0, "31", true },
                    { 17, false, "elgun@ta", "Elgun TA", new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, new byte[] { 136, 71, 198, 175, 194, 249, 214, 155, 213, 112, 27, 133, 137, 182, 74, 97, 1, 133, 25, 11, 38, 169, 207, 146, 52, 74, 126, 69, 189, 237, 210, 155, 35, 234, 181, 45, 218, 246, 47, 168, 40, 39, 102, 96, 52, 148, 22, 147, 3, 214, 189, 91, 208, 82, 113, 237, 26, 133, 59, 13, 153, 137, 101, 86, 156, 26, 147, 45, 109, 35, 165, 178, 199, 101, 150, 99, 153, 255, 88, 167, 207, 3, 185, 188, 185, 164, 112, 216, 103, 152, 205, 172, 98, 214, 78, 153, 56, 56, 73, 95, 179, 87, 40, 133, 16, 133, 37, 152, 211, 167, 166, 80, 156, 115, 57, 15, 61, 64, 13, 111, 188, 116, 14, 52, 221, 69, 31, 239 }, new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, 0, "31", true },
                    { 18, false, "irem@reis", "Irem Reis", new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, new byte[] { 136, 71, 198, 175, 194, 249, 214, 155, 213, 112, 27, 133, 137, 182, 74, 97, 1, 133, 25, 11, 38, 169, 207, 146, 52, 74, 126, 69, 189, 237, 210, 155, 35, 234, 181, 45, 218, 246, 47, 168, 40, 39, 102, 96, 52, 148, 22, 147, 3, 214, 189, 91, 208, 82, 113, 237, 26, 133, 59, 13, 153, 137, 101, 86, 156, 26, 147, 45, 109, 35, 165, 178, 199, 101, 150, 99, 153, 255, 88, 167, 207, 3, 185, 188, 185, 164, 112, 216, 103, 152, 205, 172, 98, 214, 78, 153, 56, 56, 73, 95, 179, 87, 40, 133, 16, 133, 37, 152, 211, 167, 166, 80, 156, 115, 57, 15, 61, 64, 13, 111, 188, 116, 14, 52, 221, 69, 31, 239 }, new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, 0, "31", true },
                    { 21, false, "ercument@cicek", "Ercument Cicek", new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, new byte[] { 136, 71, 198, 175, 194, 249, 214, 155, 213, 112, 27, 133, 137, 182, 74, 97, 1, 133, 25, 11, 38, 169, 207, 146, 52, 74, 126, 69, 189, 237, 210, 155, 35, 234, 181, 45, 218, 246, 47, 168, 40, 39, 102, 96, 52, 148, 22, 147, 3, 214, 189, 91, 208, 82, 113, 237, 26, 133, 59, 13, 153, 137, 101, 86, 156, 26, 147, 45, 109, 35, 165, 178, 199, 101, 150, 99, 153, 255, 88, 167, 207, 3, 185, 188, 185, 164, 112, 216, 103, 152, 205, 172, 98, 214, 78, 153, 56, 56, 73, 95, 179, 87, 40, 133, 16, 133, 37, 152, 211, 167, 166, 80, 156, 115, 57, 15, 61, 64, 13, 111, 188, 116, 14, 52, 221, 69, 31, 239 }, new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, 1, "31", true },
                    { 20, false, "can@alkan", "Can Alkan", new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, new byte[] { 136, 71, 198, 175, 194, 249, 214, 155, 213, 112, 27, 133, 137, 182, 74, 97, 1, 133, 25, 11, 38, 169, 207, 146, 52, 74, 126, 69, 189, 237, 210, 155, 35, 234, 181, 45, 218, 246, 47, 168, 40, 39, 102, 96, 52, 148, 22, 147, 3, 214, 189, 91, 208, 82, 113, 237, 26, 133, 59, 13, 153, 137, 101, 86, 156, 26, 147, 45, 109, 35, 165, 178, 199, 101, 150, 99, 153, 255, 88, 167, 207, 3, 185, 188, 185, 164, 112, 216, 103, 152, 205, 172, 98, 214, 78, 153, 56, 56, 73, 95, 179, 87, 40, 133, 16, 133, 37, 152, 211, 167, 166, 80, 156, 115, 57, 15, 61, 64, 13, 111, 188, 116, 14, 52, 221, 69, 31, 239 }, new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, 1, "31", true },
                    { 14, false, "fuat@schwarzenegger", "Fuat Schwarzengger", new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, new byte[] { 136, 71, 198, 175, 194, 249, 214, 155, 213, 112, 27, 133, 137, 182, 74, 97, 1, 133, 25, 11, 38, 169, 207, 146, 52, 74, 126, 69, 189, 237, 210, 155, 35, 234, 181, 45, 218, 246, 47, 168, 40, 39, 102, 96, 52, 148, 22, 147, 3, 214, 189, 91, 208, 82, 113, 237, 26, 133, 59, 13, 153, 137, 101, 86, 156, 26, 147, 45, 109, 35, 165, 178, 199, 101, 150, 99, 153, 255, 88, 167, 207, 3, 185, 188, 185, 164, 112, 216, 103, 152, 205, 172, 98, 214, 78, 153, 56, 56, 73, 95, 179, 87, 40, 133, 16, 133, 37, 152, 211, 167, 166, 80, 156, 115, 57, 15, 61, 64, 13, 111, 188, 116, 14, 52, 221, 69, 31, 239 }, new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, 0, "31", true },
                    { 22, false, "alper@karel", "Alper Sarikan", new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, new byte[] { 136, 71, 198, 175, 194, 249, 214, 155, 213, 112, 27, 133, 137, 182, 74, 97, 1, 133, 25, 11, 38, 169, 207, 146, 52, 74, 126, 69, 189, 237, 210, 155, 35, 234, 181, 45, 218, 246, 47, 168, 40, 39, 102, 96, 52, 148, 22, 147, 3, 214, 189, 91, 208, 82, 113, 237, 26, 133, 59, 13, 153, 137, 101, 86, 156, 26, 147, 45, 109, 35, 165, 178, 199, 101, 150, 99, 153, 255, 88, 167, 207, 3, 185, 188, 185, 164, 112, 216, 103, 152, 205, 172, 98, 214, 78, 153, 56, 56, 73, 95, 179, 87, 40, 133, 16, 133, 37, 152, 211, 167, 166, 80, 156, 115, 57, 15, 61, 64, 13, 111, 188, 116, 14, 52, 221, 69, 31, 239 }, new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, 1, "31", true },
                    { 23, false, "altay@guvenir", "Altay Guvenir", new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, new byte[] { 136, 71, 198, 175, 194, 249, 214, 155, 213, 112, 27, 133, 137, 182, 74, 97, 1, 133, 25, 11, 38, 169, 207, 146, 52, 74, 126, 69, 189, 237, 210, 155, 35, 234, 181, 45, 218, 246, 47, 168, 40, 39, 102, 96, 52, 148, 22, 147, 3, 214, 189, 91, 208, 82, 113, 237, 26, 133, 59, 13, 153, 137, 101, 86, 156, 26, 147, 45, 109, 35, 165, 178, 199, 101, 150, 99, 153, 255, 88, 167, 207, 3, 185, 188, 185, 164, 112, 216, 103, 152, 205, 172, 98, 214, 78, 153, 56, 56, 73, 95, 179, 87, 40, 133, 16, 133, 37, 152, 211, 167, 166, 80, 156, 115, 57, 15, 61, 64, 13, 111, 188, 116, 14, 52, 221, 69, 31, 239 }, new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, 1, "31", true },
                    { 19, false, "fazli@can", "Fazli Can", new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, new byte[] { 136, 71, 198, 175, 194, 249, 214, 155, 213, 112, 27, 133, 137, 182, 74, 97, 1, 133, 25, 11, 38, 169, 207, 146, 52, 74, 126, 69, 189, 237, 210, 155, 35, 234, 181, 45, 218, 246, 47, 168, 40, 39, 102, 96, 52, 148, 22, 147, 3, 214, 189, 91, 208, 82, 113, 237, 26, 133, 59, 13, 153, 137, 101, 86, 156, 26, 147, 45, 109, 35, 165, 178, 199, 101, 150, 99, 153, 255, 88, 167, 207, 3, 185, 188, 185, 164, 112, 216, 103, 152, 205, 172, 98, 214, 78, 153, 56, 56, 73, 95, 179, 87, 40, 133, 16, 133, 37, 152, 211, 167, 166, 80, 156, 115, 57, 15, 61, 64, 13, 111, 188, 116, 14, 52, 221, 69, 31, 239 }, new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, 1, "31", true },
                    { 13, false, "onur@korkmaz", "Onur Korkmaz", new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, new byte[] { 136, 71, 198, 175, 194, 249, 214, 155, 213, 112, 27, 133, 137, 182, 74, 97, 1, 133, 25, 11, 38, 169, 207, 146, 52, 74, 126, 69, 189, 237, 210, 155, 35, 234, 181, 45, 218, 246, 47, 168, 40, 39, 102, 96, 52, 148, 22, 147, 3, 214, 189, 91, 208, 82, 113, 237, 26, 133, 59, 13, 153, 137, 101, 86, 156, 26, 147, 45, 109, 35, 165, 178, 199, 101, 150, 99, 153, 255, 88, 167, 207, 3, 185, 188, 185, 164, 112, 216, 103, 152, 205, 172, 98, 214, 78, 153, 56, 56, 73, 95, 179, 87, 40, 133, 16, 133, 37, 152, 211, 167, 166, 80, 156, 115, 57, 15, 61, 64, 13, 111, 188, 116, 14, 52, 221, 69, 31, 239 }, new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, 0, "31", true },
                    { 10, false, "hami@mert", "Hami Mert", new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, new byte[] { 136, 71, 198, 175, 194, 249, 214, 155, 213, 112, 27, 133, 137, 182, 74, 97, 1, 133, 25, 11, 38, 169, 207, 146, 52, 74, 126, 69, 189, 237, 210, 155, 35, 234, 181, 45, 218, 246, 47, 168, 40, 39, 102, 96, 52, 148, 22, 147, 3, 214, 189, 91, 208, 82, 113, 237, 26, 133, 59, 13, 153, 137, 101, 86, 156, 26, 147, 45, 109, 35, 165, 178, 199, 101, 150, 99, 153, 255, 88, 167, 207, 3, 185, 188, 185, 164, 112, 216, 103, 152, 205, 172, 98, 214, 78, 153, 56, 56, 73, 95, 179, 87, 40, 133, 16, 133, 37, 152, 211, 167, 166, 80, 156, 115, 57, 15, 61, 64, 13, 111, 188, 116, 14, 52, 221, 69, 31, 239 }, new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, 0, "31", true },
                    { 11, false, "cagri@eren", "Cagri Eren", new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, new byte[] { 136, 71, 198, 175, 194, 249, 214, 155, 213, 112, 27, 133, 137, 182, 74, 97, 1, 133, 25, 11, 38, 169, 207, 146, 52, 74, 126, 69, 189, 237, 210, 155, 35, 234, 181, 45, 218, 246, 47, 168, 40, 39, 102, 96, 52, 148, 22, 147, 3, 214, 189, 91, 208, 82, 113, 237, 26, 133, 59, 13, 153, 137, 101, 86, 156, 26, 147, 45, 109, 35, 165, 178, 199, 101, 150, 99, 153, 255, 88, 167, 207, 3, 185, 188, 185, 164, 112, 216, 103, 152, 205, 172, 98, 214, 78, 153, 56, 56, 73, 95, 179, 87, 40, 133, 16, 133, 37, 152, 211, 167, 166, 80, 156, 115, 57, 15, 61, 64, 13, 111, 188, 116, 14, 52, 221, 69, 31, 239 }, new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, 0, "31", true },
                    { 24, false, "tuna@derbeder", "Tuna Derbeder", new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, new byte[] { 136, 71, 198, 175, 194, 249, 214, 155, 213, 112, 27, 133, 137, 182, 74, 97, 1, 133, 25, 11, 38, 169, 207, 146, 52, 74, 126, 69, 189, 237, 210, 155, 35, 234, 181, 45, 218, 246, 47, 168, 40, 39, 102, 96, 52, 148, 22, 147, 3, 214, 189, 91, 208, 82, 113, 237, 26, 133, 59, 13, 153, 137, 101, 86, 156, 26, 147, 45, 109, 35, 165, 178, 199, 101, 150, 99, 153, 255, 88, 167, 207, 3, 185, 188, 185, 164, 112, 216, 103, 152, 205, 172, 98, 214, 78, 153, 56, 56, 73, 95, 179, 87, 40, 133, 16, 133, 37, 152, 211, 167, 166, 80, 156, 115, 57, 15, 61, 64, 13, 111, 188, 116, 14, 52, 221, 69, 31, 239 }, new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, 0, "31", true },
                    { 9, false, "funda@tan", "Funda Tan", new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, new byte[] { 136, 71, 198, 175, 194, 249, 214, 155, 213, 112, 27, 133, 137, 182, 74, 97, 1, 133, 25, 11, 38, 169, 207, 146, 52, 74, 126, 69, 189, 237, 210, 155, 35, 234, 181, 45, 218, 246, 47, 168, 40, 39, 102, 96, 52, 148, 22, 147, 3, 214, 189, 91, 208, 82, 113, 237, 26, 133, 59, 13, 153, 137, 101, 86, 156, 26, 147, 45, 109, 35, 165, 178, 199, 101, 150, 99, 153, 255, 88, 167, 207, 3, 185, 188, 185, 164, 112, 216, 103, 152, 205, 172, 98, 214, 78, 153, 56, 56, 73, 95, 179, 87, 40, 133, 16, 133, 37, 152, 211, 167, 166, 80, 156, 115, 57, 15, 61, 64, 13, 111, 188, 116, 14, 52, 221, 69, 31, 239 }, new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, 0, "31", true },
                    { 8, false, "berke@ceran", "HOCAM SIMDI BIZ SOYLE BI SISTEM DUSUNDUK DE", new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, new byte[] { 136, 71, 198, 175, 194, 249, 214, 155, 213, 112, 27, 133, 137, 182, 74, 97, 1, 133, 25, 11, 38, 169, 207, 146, 52, 74, 126, 69, 189, 237, 210, 155, 35, 234, 181, 45, 218, 246, 47, 168, 40, 39, 102, 96, 52, 148, 22, 147, 3, 214, 189, 91, 208, 82, 113, 237, 26, 133, 59, 13, 153, 137, 101, 86, 156, 26, 147, 45, 109, 35, 165, 178, 199, 101, 150, 99, 153, 255, 88, 167, 207, 3, 185, 188, 185, 164, 112, 216, 103, 152, 205, 172, 98, 214, 78, 153, 56, 56, 73, 95, 179, 87, 40, 133, 16, 133, 37, 152, 211, 167, 166, 80, 156, 115, 57, 15, 61, 64, 13, 111, 188, 116, 14, 52, 221, 69, 31, 239 }, new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, 0, "31", true },
                    { 7, false, "oguzhan@ozcelik", "oguzhan ozcelik", new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, new byte[] { 136, 71, 198, 175, 194, 249, 214, 155, 213, 112, 27, 133, 137, 182, 74, 97, 1, 133, 25, 11, 38, 169, 207, 146, 52, 74, 126, 69, 189, 237, 210, 155, 35, 234, 181, 45, 218, 246, 47, 168, 40, 39, 102, 96, 52, 148, 22, 147, 3, 214, 189, 91, 208, 82, 113, 237, 26, 133, 59, 13, 153, 137, 101, 86, 156, 26, 147, 45, 109, 35, 165, 178, 199, 101, 150, 99, 153, 255, 88, 167, 207, 3, 185, 188, 185, 164, 112, 216, 103, 152, 205, 172, 98, 214, 78, 153, 56, 56, 73, 95, 179, 87, 40, 133, 16, 133, 37, 152, 211, 167, 166, 80, 156, 115, 57, 15, 61, 64, 13, 111, 188, 116, 14, 52, 221, 69, 31, 239 }, new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, 0, "31", true },
                    { 6, false, "aybala@karakaya", "Aybala karakaya", new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, new byte[] { 136, 71, 198, 175, 194, 249, 214, 155, 213, 112, 27, 133, 137, 182, 74, 97, 1, 133, 25, 11, 38, 169, 207, 146, 52, 74, 126, 69, 189, 237, 210, 155, 35, 234, 181, 45, 218, 246, 47, 168, 40, 39, 102, 96, 52, 148, 22, 147, 3, 214, 189, 91, 208, 82, 113, 237, 26, 133, 59, 13, 153, 137, 101, 86, 156, 26, 147, 45, 109, 35, 165, 178, 199, 101, 150, 99, 153, 255, 88, 167, 207, 3, 185, 188, 185, 164, 112, 216, 103, 152, 205, 172, 98, 214, 78, 153, 56, 56, 73, 95, 179, 87, 40, 133, 16, 133, 37, 152, 211, 167, 166, 80, 156, 115, 57, 15, 61, 64, 13, 111, 188, 116, 14, 52, 221, 69, 31, 239 }, new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, 0, "31", true },
                    { 5, false, "yusuf@kawai", "Yusuf Uyar", new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, new byte[] { 136, 71, 198, 175, 194, 249, 214, 155, 213, 112, 27, 133, 137, 182, 74, 97, 1, 133, 25, 11, 38, 169, 207, 146, 52, 74, 126, 69, 189, 237, 210, 155, 35, 234, 181, 45, 218, 246, 47, 168, 40, 39, 102, 96, 52, 148, 22, 147, 3, 214, 189, 91, 208, 82, 113, 237, 26, 133, 59, 13, 153, 137, 101, 86, 156, 26, 147, 45, 109, 35, 165, 178, 199, 101, 150, 99, 153, 255, 88, 167, 207, 3, 185, 188, 185, 164, 112, 216, 103, 152, 205, 172, 98, 214, 78, 153, 56, 56, 73, 95, 179, 87, 40, 133, 16, 133, 37, 152, 211, 167, 166, 80, 156, 115, 57, 15, 61, 64, 13, 111, 188, 116, 14, 52, 221, 69, 31, 239 }, new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, 0, "31", true },
                    { 4, false, "ozgur@demir", "Ozgur Chadoglu", new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, new byte[] { 136, 71, 198, 175, 194, 249, 214, 155, 213, 112, 27, 133, 137, 182, 74, 97, 1, 133, 25, 11, 38, 169, 207, 146, 52, 74, 126, 69, 189, 237, 210, 155, 35, 234, 181, 45, 218, 246, 47, 168, 40, 39, 102, 96, 52, 148, 22, 147, 3, 214, 189, 91, 208, 82, 113, 237, 26, 133, 59, 13, 153, 137, 101, 86, 156, 26, 147, 45, 109, 35, 165, 178, 199, 101, 150, 99, 153, 255, 88, 167, 207, 3, 185, 188, 185, 164, 112, 216, 103, 152, 205, 172, 98, 214, 78, 153, 56, 56, 73, 95, 179, 87, 40, 133, 16, 133, 37, 152, 211, 167, 166, 80, 156, 115, 57, 15, 61, 64, 13, 111, 188, 116, 14, 52, 221, 69, 31, 239 }, new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, 0, "31", true },
                    { 3, false, "baris@ogun", "Baris Ogun", new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, new byte[] { 136, 71, 198, 175, 194, 249, 214, 155, 213, 112, 27, 133, 137, 182, 74, 97, 1, 133, 25, 11, 38, 169, 207, 146, 52, 74, 126, 69, 189, 237, 210, 155, 35, 234, 181, 45, 218, 246, 47, 168, 40, 39, 102, 96, 52, 148, 22, 147, 3, 214, 189, 91, 208, 82, 113, 237, 26, 133, 59, 13, 153, 137, 101, 86, 156, 26, 147, 45, 109, 35, 165, 178, 199, 101, 150, 99, 153, 255, 88, 167, 207, 3, 185, 188, 185, 164, 112, 216, 103, 152, 205, 172, 98, 214, 78, 153, 56, 56, 73, 95, 179, 87, 40, 133, 16, 133, 37, 152, 211, 167, 166, 80, 156, 115, 57, 15, 61, 64, 13, 111, 188, 116, 14, 52, 221, 69, 31, 239 }, new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, 0, "31", true },
                    { 2, false, "eray@tuzun", "Eray Tuzun", new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, new byte[] { 136, 71, 198, 175, 194, 249, 214, 155, 213, 112, 27, 133, 137, 182, 74, 97, 1, 133, 25, 11, 38, 169, 207, 146, 52, 74, 126, 69, 189, 237, 210, 155, 35, 234, 181, 45, 218, 246, 47, 168, 40, 39, 102, 96, 52, 148, 22, 147, 3, 214, 189, 91, 208, 82, 113, 237, 26, 133, 59, 13, 153, 137, 101, 86, 156, 26, 147, 45, 109, 35, 165, 178, 199, 101, 150, 99, 153, 255, 88, 167, 207, 3, 185, 188, 185, 164, 112, 216, 103, 152, 205, 172, 98, 214, 78, 153, 56, 56, 73, 95, 179, 87, 40, 133, 16, 133, 37, 152, 211, 167, 166, 80, 156, 115, 57, 15, 61, 64, 13, 111, 188, 116, 14, 52, 221, 69, 31, 239 }, new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, 0, "31", true },
                    { 1, false, "cagri@durgut", "Cagri Durgut", new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, new byte[] { 136, 71, 198, 175, 194, 249, 214, 155, 213, 112, 27, 133, 137, 182, 74, 97, 1, 133, 25, 11, 38, 169, 207, 146, 52, 74, 126, 69, 189, 237, 210, 155, 35, 234, 181, 45, 218, 246, 47, 168, 40, 39, 102, 96, 52, 148, 22, 147, 3, 214, 189, 91, 208, 82, 113, 237, 26, 133, 59, 13, 153, 137, 101, 86, 156, 26, 147, 45, 109, 35, 165, 178, 199, 101, 150, 99, 153, 255, 88, 167, 207, 3, 185, 188, 185, 164, 112, 216, 103, 152, 205, 172, 98, 214, 78, 153, 56, 56, 73, 95, 179, 87, 40, 133, 16, 133, 37, 152, 211, 167, 166, 80, 156, 115, 57, 15, 61, 64, 13, 111, 188, 116, 14, 52, 221, 69, 31, 239 }, new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, 0, "31", true },
                    { 12, false, "guven@gerger", "Guven Gerger", new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, new byte[] { 136, 71, 198, 175, 194, 249, 214, 155, 213, 112, 27, 133, 137, 182, 74, 97, 1, 133, 25, 11, 38, 169, 207, 146, 52, 74, 126, 69, 189, 237, 210, 155, 35, 234, 181, 45, 218, 246, 47, 168, 40, 39, 102, 96, 52, 148, 22, 147, 3, 214, 189, 91, 208, 82, 113, 237, 26, 133, 59, 13, 153, 137, 101, 86, 156, 26, 147, 45, 109, 35, 165, 178, 199, 101, 150, 99, 153, 255, 88, 167, 207, 3, 185, 188, 185, 164, 112, 216, 103, 152, 205, 172, 98, 214, 78, 153, 56, 56, 73, 95, 179, 87, 40, 133, 16, 133, 37, 152, 211, 167, 166, 80, 156, 115, 57, 15, 61, 64, 13, 111, 188, 116, 14, 52, 221, 69, 31, 239 }, new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, 0, "31", true },
                    { 25, false, "abdul@razak", "Abdul Razak", new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, new byte[] { 136, 71, 198, 175, 194, 249, 214, 155, 213, 112, 27, 133, 137, 182, 74, 97, 1, 133, 25, 11, 38, 169, 207, 146, 52, 74, 126, 69, 189, 237, 210, 155, 35, 234, 181, 45, 218, 246, 47, 168, 40, 39, 102, 96, 52, 148, 22, 147, 3, 214, 189, 91, 208, 82, 113, 237, 26, 133, 59, 13, 153, 137, 101, 86, 156, 26, 147, 45, 109, 35, 165, 178, 199, 101, 150, 99, 153, 255, 88, 167, 207, 3, 185, 188, 185, 164, 112, 216, 103, 152, 205, 172, 98, 214, 78, 153, 56, 56, 73, 95, 179, 87, 40, 133, 16, 133, 37, 152, 211, 167, 166, 80, 156, 115, 57, 15, 61, 64, 13, 111, 188, 116, 14, 52, 221, 69, 31, 239 }, new byte[] { 200, 169, 178, 206, 254, 155, 114, 103, 12, 107, 166, 90, 51, 73, 219, 153, 30, 187, 32, 186, 211, 78, 165, 167, 39, 111, 38, 198, 174, 24, 176, 17, 138, 223, 31, 51, 141, 234, 248, 212, 223, 233, 65, 238, 132, 30, 127, 242, 79, 143, 213, 55, 39, 197, 117, 130, 14, 246, 79, 67, 82, 219, 127, 176 }, 0, "31", true }
                });

            migrationBuilder.InsertData(
                table: "CourseUsers",
                columns: new[] { "CourseId", "UserId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 3, 21 },
                    { 6, 20 },
                    { 4, 19 },
                    { 2, 18 },
                    { 1, 17 },
                    { 1, 16 },
                    { 5, 15 },
                    { 3, 15 },
                    { 2, 23 },
                    { 4, 22 }
                });

            migrationBuilder.InsertData(
                table: "Sections",
                columns: new[] { "Id", "AffiliatedCourseId", "SectionNo", "SectionlessState" },
                values: new object[,]
                {
                    { 9, 6, 1, false },
                    { 8, 5, 2, false },
                    { 7, 4, 2, false },
                    { 6, 4, 1, false },
                    { 5, 3, 1, false },
                    { 4, 2, 2, true },
                    { 3, 2, 1, true },
                    { 2, 1, 2, false },
                    { 1, 1, 1, false }
                });

            migrationBuilder.InsertData(
                table: "Assignments",
                columns: new[] { "Id", "AcceptedTypes", "AffiliatedSectionId", "AssignmentDescriptionFile", "CanBeGradedByStudents", "CourseId", "CreatedAt", "DueDate", "IsItGraded", "MaxFileSizeInBytes", "VisibilityOfSubmission" },
                values: new object[,]
                {
                    { 3, "pdf-doc-docx", 1, "Odev iteration 1", true, 1, new DateTime(2021, 4, 27, 1, 12, 16, 882, DateTimeKind.Local).AddTicks(1672), new DateTime(2021, 4, 15, 7, 0, 0, 0, DateTimeKind.Unspecified), true, 4096, true },
                    { 4, "pdf-doc-docx", 1, "Odev iteration 2", true, 1, new DateTime(2021, 4, 27, 1, 12, 16, 883, DateTimeKind.Local).AddTicks(1618), new DateTime(2021, 4, 15, 7, 0, 0, 0, DateTimeKind.Unspecified), true, 4096, true },
                    { 1, "pdf,doc,docx", 1, "Odev", true, 1, new DateTime(2021, 4, 27, 1, 12, 16, 886, DateTimeKind.Local).AddTicks(4750), new DateTime(2021, 5, 15, 7, 0, 0, 0, DateTimeKind.Unspecified), true, 1024, true },
                    { 2, "pdf,doc,docx", 1, "315 proje", false, 2, new DateTime(2021, 4, 27, 1, 12, 16, 886, DateTimeKind.Local).AddTicks(4759), new DateTime(2021, 5, 15, 7, 0, 0, 0, DateTimeKind.Unspecified), false, 1024, false }
                });

            migrationBuilder.InsertData(
                table: "ProjectGroups",
                columns: new[] { "Id", "AffiliatedCourseId", "AffiliatedSectionId", "ConfirmationState", "ConfirmedGroupMembers", "ConfirmedUserNumber", "ProjectInformation" },
                values: new object[,]
                {
                    { 1, 1, 1, false, "", 0, "BilHub Class Helper" },
                    { 5, 1, 1, false, "", 0, "Abduls Class Helper" },
                    { 3, 2, 1, false, "", 0, "AGA Language" },
                    { 2, 1, 2, false, "", 0, "Classrom Helper" },
                    { 4, 2, 2, false, "", 0, "Satis Language" }
                });

            migrationBuilder.InsertData(
                table: "ProjectGroupUsers",
                columns: new[] { "ProjectGroupId", "UserId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 4, 5 },
                    { 2, 24 },
                    { 2, 13 },
                    { 2, 12 },
                    { 2, 11 },
                    { 3, 4 },
                    { 3, 3 },
                    { 3, 1 },
                    { 5, 25 },
                    { 1, 7 },
                    { 1, 6 },
                    { 1, 5 },
                    { 1, 4 },
                    { 1, 3 },
                    { 4, 6 },
                    { 4, 7 }
                });

            migrationBuilder.InsertData(
                table: "Submissions",
                columns: new[] { "Id", "AffiliatedAssignmentId", "AffiliatedGroupId", "CourseId", "FilePath", "HasSubmission", "SectionId", "UpdatedAt" },
                values: new object[] { 1, 1, 1, 1, null, false, 1, new DateTime(2021, 4, 27, 1, 12, 16, 884, DateTimeKind.Local).AddTicks(3763) });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "CommentText", "CommentedSubmissionId", "CommentedUserId", "CreatedAt", "FileAttachmentAvailability", "FilePath", "Grade", "MaxGrade" },
                values: new object[] { 1, "nays", 1, 2, new DateTime(2021, 4, 27, 1, 12, 16, 884, DateTimeKind.Local).AddTicks(6113), true, null, 9.4m, 10.0m });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AffiliatedSectionId",
                table: "Assignments",
                column: "AffiliatedSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CommentedSubmissionId",
                table: "Comments",
                column: "CommentedSubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CommentedUserId",
                table: "Comments",
                column: "CommentedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseUsers_CourseId",
                table: "CourseUsers",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_JoinRequests_RequestedGroupId",
                table: "JoinRequests",
                column: "RequestedGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_JoinRequests_RequestingStudentId",
                table: "JoinRequests",
                column: "RequestingStudentId");

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
                name: "IX_PeerGrades_AffiliatedSectionID",
                table: "PeerGrades",
                column: "AffiliatedSectionID");

            migrationBuilder.CreateIndex(
                name: "IX_PeerGrades_ProjectGroupId",
                table: "PeerGrades",
                column: "ProjectGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectGrades_GradedProjectGroupID",
                table: "ProjectGrades",
                column: "GradedProjectGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectGrades_GradingUserId",
                table: "ProjectGrades",
                column: "GradingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectGroups_AffiliatedCourseId",
                table: "ProjectGroups",
                column: "AffiliatedCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectGroups_AffiliatedSectionId",
                table: "ProjectGroups",
                column: "AffiliatedSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectGroupUsers_ProjectGroupId",
                table: "ProjectGroupUsers",
                column: "ProjectGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_AffiliatedCourseId",
                table: "Sections",
                column: "AffiliatedCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_AffiliatedAssignmentId",
                table: "Submissions",
                column: "AffiliatedAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_AffiliatedGroupId",
                table: "Submissions",
                column: "AffiliatedGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "CourseUsers");

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
                name: "ProjectGroupUsers");

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
