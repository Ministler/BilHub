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
                    AffiliatedCourseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sections_Courses_AffiliatedCourseId",
                        column: x => x.AffiliatedCourseId,
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
                name: "Submissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AffiliatedAssignmentId = table.Column<int>(type: "int", nullable: true),
                    AffiliatedGroupId = table.Column<int>(type: "int", nullable: true),
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Submissions_ProjectGroups_AffiliatedGroupId",
                        column: x => x.AffiliatedGroupId,
                        principalTable: "ProjectGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentedUserId = table.Column<int>(type: "int", nullable: false),
                    CommentedSubmissionId = table.Column<int>(type: "int", nullable: true),
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
                        onDelete: ReferentialAction.Restrict);
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
                    { 1, "Object Oriented Software Engineering", "spring", new DateTime(2021, 5, 15, 7, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 4, 26, 0, 0, 0, 0, DateTimeKind.Local), 6, 5, "CS 319", new DateTime(2021, 2, 15, 7, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Programming Languages", "spring", new DateTime(2021, 5, 15, 7, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 4, 26, 0, 0, 0, 0, DateTimeKind.Local), 3, 2, "CS 315", new DateTime(2021, 2, 15, 7, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Data Structures and Algorithms", "spring", new DateTime(2021, 5, 15, 7, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 4, 26, 0, 0, 0, 0, DateTimeKind.Local), 1, 1, "CS 202", new DateTime(2021, 2, 15, 7, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Computer Organization and Architecture", "spring", new DateTime(2021, 5, 15, 7, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 4, 26, 0, 0, 0, 0, DateTimeKind.Local), 3, 2, "CS 224", new DateTime(2021, 2, 15, 7, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "History", "spring", new DateTime(2021, 5, 15, 7, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 4, 26, 0, 0, 0, 0, DateTimeKind.Local), 5, 4, "Hist 200", new DateTime(2021, 2, 15, 7, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "Calculus", "spring", new DateTime(2021, 5, 15, 7, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 4, 26, 0, 0, 0, 0, DateTimeKind.Local), 1, 1, "Math 101", new DateTime(2021, 2, 15, 7, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DarkModeStatus", "Email", "Name", "PasswordHash", "PasswordSalt", "SecondPasswordHash", "SectionId", "UserType", "VerificationCode", "VerifiedStatus" },
                values: new object[,]
                {
                    { 14, false, "fuat@schwarzenegger", "Fuat Schwarzengger", new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, new byte[] { 61, 213, 21, 54, 160, 195, 189, 2, 176, 76, 30, 165, 65, 163, 165, 29, 244, 226, 246, 136, 236, 43, 57, 219, 212, 43, 209, 14, 102, 151, 145, 58, 27, 217, 85, 57, 105, 7, 81, 82, 122, 33, 139, 207, 164, 138, 228, 147, 180, 77, 160, 126, 1, 14, 39, 208, 6, 171, 85, 152, 11, 22, 140, 88, 54, 164, 244, 61, 59, 88, 142, 157, 86, 37, 143, 29, 203, 197, 239, 196, 120, 127, 3, 203, 48, 182, 11, 231, 83, 106, 49, 0, 148, 73, 235, 23, 107, 204, 241, 244, 128, 119, 157, 167, 132, 214, 215, 65, 219, 218, 241, 0, 215, 188, 116, 36, 232, 167, 221, 153, 233, 146, 34, 228, 204, 91, 95, 192 }, new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, null, 0, "31", true },
                    { 15, false, "aynur@dayanik", "Aynur Dayanik", new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, new byte[] { 61, 213, 21, 54, 160, 195, 189, 2, 176, 76, 30, 165, 65, 163, 165, 29, 244, 226, 246, 136, 236, 43, 57, 219, 212, 43, 209, 14, 102, 151, 145, 58, 27, 217, 85, 57, 105, 7, 81, 82, 122, 33, 139, 207, 164, 138, 228, 147, 180, 77, 160, 126, 1, 14, 39, 208, 6, 171, 85, 152, 11, 22, 140, 88, 54, 164, 244, 61, 59, 88, 142, 157, 86, 37, 143, 29, 203, 197, 239, 196, 120, 127, 3, 203, 48, 182, 11, 231, 83, 106, 49, 0, 148, 73, 235, 23, 107, 204, 241, 244, 128, 119, 157, 167, 132, 214, 215, 65, 219, 218, 241, 0, 215, 188, 116, 36, 232, 167, 221, 153, 233, 146, 34, 228, 204, 91, 95, 192 }, new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, null, 1, "31", true },
                    { 16, false, "erdem@Tuna", "Erdem Tuna", new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, new byte[] { 61, 213, 21, 54, 160, 195, 189, 2, 176, 76, 30, 165, 65, 163, 165, 29, 244, 226, 246, 136, 236, 43, 57, 219, 212, 43, 209, 14, 102, 151, 145, 58, 27, 217, 85, 57, 105, 7, 81, 82, 122, 33, 139, 207, 164, 138, 228, 147, 180, 77, 160, 126, 1, 14, 39, 208, 6, 171, 85, 152, 11, 22, 140, 88, 54, 164, 244, 61, 59, 88, 142, 157, 86, 37, 143, 29, 203, 197, 239, 196, 120, 127, 3, 203, 48, 182, 11, 231, 83, 106, 49, 0, 148, 73, 235, 23, 107, 204, 241, 244, 128, 119, 157, 167, 132, 214, 215, 65, 219, 218, 241, 0, 215, 188, 116, 36, 232, 167, 221, 153, 233, 146, 34, 228, 204, 91, 95, 192 }, new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, null, 0, "31", true },
                    { 17, false, "elgun@ta", "Elgun TA", new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, new byte[] { 61, 213, 21, 54, 160, 195, 189, 2, 176, 76, 30, 165, 65, 163, 165, 29, 244, 226, 246, 136, 236, 43, 57, 219, 212, 43, 209, 14, 102, 151, 145, 58, 27, 217, 85, 57, 105, 7, 81, 82, 122, 33, 139, 207, 164, 138, 228, 147, 180, 77, 160, 126, 1, 14, 39, 208, 6, 171, 85, 152, 11, 22, 140, 88, 54, 164, 244, 61, 59, 88, 142, 157, 86, 37, 143, 29, 203, 197, 239, 196, 120, 127, 3, 203, 48, 182, 11, 231, 83, 106, 49, 0, 148, 73, 235, 23, 107, 204, 241, 244, 128, 119, 157, 167, 132, 214, 215, 65, 219, 218, 241, 0, 215, 188, 116, 36, 232, 167, 221, 153, 233, 146, 34, 228, 204, 91, 95, 192 }, new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, null, 0, "31", true },
                    { 20, false, "can@alkan", "Can Alkan", new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, new byte[] { 61, 213, 21, 54, 160, 195, 189, 2, 176, 76, 30, 165, 65, 163, 165, 29, 244, 226, 246, 136, 236, 43, 57, 219, 212, 43, 209, 14, 102, 151, 145, 58, 27, 217, 85, 57, 105, 7, 81, 82, 122, 33, 139, 207, 164, 138, 228, 147, 180, 77, 160, 126, 1, 14, 39, 208, 6, 171, 85, 152, 11, 22, 140, 88, 54, 164, 244, 61, 59, 88, 142, 157, 86, 37, 143, 29, 203, 197, 239, 196, 120, 127, 3, 203, 48, 182, 11, 231, 83, 106, 49, 0, 148, 73, 235, 23, 107, 204, 241, 244, 128, 119, 157, 167, 132, 214, 215, 65, 219, 218, 241, 0, 215, 188, 116, 36, 232, 167, 221, 153, 233, 146, 34, 228, 204, 91, 95, 192 }, new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, null, 1, "31", true },
                    { 19, false, "fazli@can", "Fazli Can", new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, new byte[] { 61, 213, 21, 54, 160, 195, 189, 2, 176, 76, 30, 165, 65, 163, 165, 29, 244, 226, 246, 136, 236, 43, 57, 219, 212, 43, 209, 14, 102, 151, 145, 58, 27, 217, 85, 57, 105, 7, 81, 82, 122, 33, 139, 207, 164, 138, 228, 147, 180, 77, 160, 126, 1, 14, 39, 208, 6, 171, 85, 152, 11, 22, 140, 88, 54, 164, 244, 61, 59, 88, 142, 157, 86, 37, 143, 29, 203, 197, 239, 196, 120, 127, 3, 203, 48, 182, 11, 231, 83, 106, 49, 0, 148, 73, 235, 23, 107, 204, 241, 244, 128, 119, 157, 167, 132, 214, 215, 65, 219, 218, 241, 0, 215, 188, 116, 36, 232, 167, 221, 153, 233, 146, 34, 228, 204, 91, 95, 192 }, new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, null, 1, "31", true },
                    { 13, false, "onur@korkmaz", "Onur Korkmaz", new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, new byte[] { 61, 213, 21, 54, 160, 195, 189, 2, 176, 76, 30, 165, 65, 163, 165, 29, 244, 226, 246, 136, 236, 43, 57, 219, 212, 43, 209, 14, 102, 151, 145, 58, 27, 217, 85, 57, 105, 7, 81, 82, 122, 33, 139, 207, 164, 138, 228, 147, 180, 77, 160, 126, 1, 14, 39, 208, 6, 171, 85, 152, 11, 22, 140, 88, 54, 164, 244, 61, 59, 88, 142, 157, 86, 37, 143, 29, 203, 197, 239, 196, 120, 127, 3, 203, 48, 182, 11, 231, 83, 106, 49, 0, 148, 73, 235, 23, 107, 204, 241, 244, 128, 119, 157, 167, 132, 214, 215, 65, 219, 218, 241, 0, 215, 188, 116, 36, 232, 167, 221, 153, 233, 146, 34, 228, 204, 91, 95, 192 }, new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, null, 0, "31", true },
                    { 21, false, "ercument@cicek", "Ercument Cicek", new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, new byte[] { 61, 213, 21, 54, 160, 195, 189, 2, 176, 76, 30, 165, 65, 163, 165, 29, 244, 226, 246, 136, 236, 43, 57, 219, 212, 43, 209, 14, 102, 151, 145, 58, 27, 217, 85, 57, 105, 7, 81, 82, 122, 33, 139, 207, 164, 138, 228, 147, 180, 77, 160, 126, 1, 14, 39, 208, 6, 171, 85, 152, 11, 22, 140, 88, 54, 164, 244, 61, 59, 88, 142, 157, 86, 37, 143, 29, 203, 197, 239, 196, 120, 127, 3, 203, 48, 182, 11, 231, 83, 106, 49, 0, 148, 73, 235, 23, 107, 204, 241, 244, 128, 119, 157, 167, 132, 214, 215, 65, 219, 218, 241, 0, 215, 188, 116, 36, 232, 167, 221, 153, 233, 146, 34, 228, 204, 91, 95, 192 }, new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, null, 1, "31", true },
                    { 22, false, "alper@karel", "Alper Sarikan", new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, new byte[] { 61, 213, 21, 54, 160, 195, 189, 2, 176, 76, 30, 165, 65, 163, 165, 29, 244, 226, 246, 136, 236, 43, 57, 219, 212, 43, 209, 14, 102, 151, 145, 58, 27, 217, 85, 57, 105, 7, 81, 82, 122, 33, 139, 207, 164, 138, 228, 147, 180, 77, 160, 126, 1, 14, 39, 208, 6, 171, 85, 152, 11, 22, 140, 88, 54, 164, 244, 61, 59, 88, 142, 157, 86, 37, 143, 29, 203, 197, 239, 196, 120, 127, 3, 203, 48, 182, 11, 231, 83, 106, 49, 0, 148, 73, 235, 23, 107, 204, 241, 244, 128, 119, 157, 167, 132, 214, 215, 65, 219, 218, 241, 0, 215, 188, 116, 36, 232, 167, 221, 153, 233, 146, 34, 228, 204, 91, 95, 192 }, new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, null, 1, "31", true },
                    { 18, false, "irem@reis", "Irem Reis", new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, new byte[] { 61, 213, 21, 54, 160, 195, 189, 2, 176, 76, 30, 165, 65, 163, 165, 29, 244, 226, 246, 136, 236, 43, 57, 219, 212, 43, 209, 14, 102, 151, 145, 58, 27, 217, 85, 57, 105, 7, 81, 82, 122, 33, 139, 207, 164, 138, 228, 147, 180, 77, 160, 126, 1, 14, 39, 208, 6, 171, 85, 152, 11, 22, 140, 88, 54, 164, 244, 61, 59, 88, 142, 157, 86, 37, 143, 29, 203, 197, 239, 196, 120, 127, 3, 203, 48, 182, 11, 231, 83, 106, 49, 0, 148, 73, 235, 23, 107, 204, 241, 244, 128, 119, 157, 167, 132, 214, 215, 65, 219, 218, 241, 0, 215, 188, 116, 36, 232, 167, 221, 153, 233, 146, 34, 228, 204, 91, 95, 192 }, new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, null, 0, "31", true },
                    { 12, false, "guven@gerger", "Guven Gerger", new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, new byte[] { 61, 213, 21, 54, 160, 195, 189, 2, 176, 76, 30, 165, 65, 163, 165, 29, 244, 226, 246, 136, 236, 43, 57, 219, 212, 43, 209, 14, 102, 151, 145, 58, 27, 217, 85, 57, 105, 7, 81, 82, 122, 33, 139, 207, 164, 138, 228, 147, 180, 77, 160, 126, 1, 14, 39, 208, 6, 171, 85, 152, 11, 22, 140, 88, 54, 164, 244, 61, 59, 88, 142, 157, 86, 37, 143, 29, 203, 197, 239, 196, 120, 127, 3, 203, 48, 182, 11, 231, 83, 106, 49, 0, 148, 73, 235, 23, 107, 204, 241, 244, 128, 119, 157, 167, 132, 214, 215, 65, 219, 218, 241, 0, 215, 188, 116, 36, 232, 167, 221, 153, 233, 146, 34, 228, 204, 91, 95, 192 }, new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, null, 0, "31", true },
                    { 9, false, "funda@tan", "Funda Tan", new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, new byte[] { 61, 213, 21, 54, 160, 195, 189, 2, 176, 76, 30, 165, 65, 163, 165, 29, 244, 226, 246, 136, 236, 43, 57, 219, 212, 43, 209, 14, 102, 151, 145, 58, 27, 217, 85, 57, 105, 7, 81, 82, 122, 33, 139, 207, 164, 138, 228, 147, 180, 77, 160, 126, 1, 14, 39, 208, 6, 171, 85, 152, 11, 22, 140, 88, 54, 164, 244, 61, 59, 88, 142, 157, 86, 37, 143, 29, 203, 197, 239, 196, 120, 127, 3, 203, 48, 182, 11, 231, 83, 106, 49, 0, 148, 73, 235, 23, 107, 204, 241, 244, 128, 119, 157, 167, 132, 214, 215, 65, 219, 218, 241, 0, 215, 188, 116, 36, 232, 167, 221, 153, 233, 146, 34, 228, 204, 91, 95, 192 }, new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, null, 0, "31", true },
                    { 10, false, "hami@mert", "Hami Mert", new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, new byte[] { 61, 213, 21, 54, 160, 195, 189, 2, 176, 76, 30, 165, 65, 163, 165, 29, 244, 226, 246, 136, 236, 43, 57, 219, 212, 43, 209, 14, 102, 151, 145, 58, 27, 217, 85, 57, 105, 7, 81, 82, 122, 33, 139, 207, 164, 138, 228, 147, 180, 77, 160, 126, 1, 14, 39, 208, 6, 171, 85, 152, 11, 22, 140, 88, 54, 164, 244, 61, 59, 88, 142, 157, 86, 37, 143, 29, 203, 197, 239, 196, 120, 127, 3, 203, 48, 182, 11, 231, 83, 106, 49, 0, 148, 73, 235, 23, 107, 204, 241, 244, 128, 119, 157, 167, 132, 214, 215, 65, 219, 218, 241, 0, 215, 188, 116, 36, 232, 167, 221, 153, 233, 146, 34, 228, 204, 91, 95, 192 }, new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, null, 0, "31", true },
                    { 23, false, "altay@guvenir", "Altay Guvenir", new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, new byte[] { 61, 213, 21, 54, 160, 195, 189, 2, 176, 76, 30, 165, 65, 163, 165, 29, 244, 226, 246, 136, 236, 43, 57, 219, 212, 43, 209, 14, 102, 151, 145, 58, 27, 217, 85, 57, 105, 7, 81, 82, 122, 33, 139, 207, 164, 138, 228, 147, 180, 77, 160, 126, 1, 14, 39, 208, 6, 171, 85, 152, 11, 22, 140, 88, 54, 164, 244, 61, 59, 88, 142, 157, 86, 37, 143, 29, 203, 197, 239, 196, 120, 127, 3, 203, 48, 182, 11, 231, 83, 106, 49, 0, 148, 73, 235, 23, 107, 204, 241, 244, 128, 119, 157, 167, 132, 214, 215, 65, 219, 218, 241, 0, 215, 188, 116, 36, 232, 167, 221, 153, 233, 146, 34, 228, 204, 91, 95, 192 }, new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, null, 1, "31", true },
                    { 8, false, "berke@ceran", "HOCAM SIMDI BIZ SOYLE BI SISTEM DUSUNDUK DE", new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, new byte[] { 61, 213, 21, 54, 160, 195, 189, 2, 176, 76, 30, 165, 65, 163, 165, 29, 244, 226, 246, 136, 236, 43, 57, 219, 212, 43, 209, 14, 102, 151, 145, 58, 27, 217, 85, 57, 105, 7, 81, 82, 122, 33, 139, 207, 164, 138, 228, 147, 180, 77, 160, 126, 1, 14, 39, 208, 6, 171, 85, 152, 11, 22, 140, 88, 54, 164, 244, 61, 59, 88, 142, 157, 86, 37, 143, 29, 203, 197, 239, 196, 120, 127, 3, 203, 48, 182, 11, 231, 83, 106, 49, 0, 148, 73, 235, 23, 107, 204, 241, 244, 128, 119, 157, 167, 132, 214, 215, 65, 219, 218, 241, 0, 215, 188, 116, 36, 232, 167, 221, 153, 233, 146, 34, 228, 204, 91, 95, 192 }, new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, null, 0, "31", true },
                    { 7, false, "oguzhan@ozcelik", "oguzhan ozcelik", new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, new byte[] { 61, 213, 21, 54, 160, 195, 189, 2, 176, 76, 30, 165, 65, 163, 165, 29, 244, 226, 246, 136, 236, 43, 57, 219, 212, 43, 209, 14, 102, 151, 145, 58, 27, 217, 85, 57, 105, 7, 81, 82, 122, 33, 139, 207, 164, 138, 228, 147, 180, 77, 160, 126, 1, 14, 39, 208, 6, 171, 85, 152, 11, 22, 140, 88, 54, 164, 244, 61, 59, 88, 142, 157, 86, 37, 143, 29, 203, 197, 239, 196, 120, 127, 3, 203, 48, 182, 11, 231, 83, 106, 49, 0, 148, 73, 235, 23, 107, 204, 241, 244, 128, 119, 157, 167, 132, 214, 215, 65, 219, 218, 241, 0, 215, 188, 116, 36, 232, 167, 221, 153, 233, 146, 34, 228, 204, 91, 95, 192 }, new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, null, 0, "31", true },
                    { 6, false, "aybala@karakaya", "Aybala karakaya", new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, new byte[] { 61, 213, 21, 54, 160, 195, 189, 2, 176, 76, 30, 165, 65, 163, 165, 29, 244, 226, 246, 136, 236, 43, 57, 219, 212, 43, 209, 14, 102, 151, 145, 58, 27, 217, 85, 57, 105, 7, 81, 82, 122, 33, 139, 207, 164, 138, 228, 147, 180, 77, 160, 126, 1, 14, 39, 208, 6, 171, 85, 152, 11, 22, 140, 88, 54, 164, 244, 61, 59, 88, 142, 157, 86, 37, 143, 29, 203, 197, 239, 196, 120, 127, 3, 203, 48, 182, 11, 231, 83, 106, 49, 0, 148, 73, 235, 23, 107, 204, 241, 244, 128, 119, 157, 167, 132, 214, 215, 65, 219, 218, 241, 0, 215, 188, 116, 36, 232, 167, 221, 153, 233, 146, 34, 228, 204, 91, 95, 192 }, new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, null, 0, "31", true },
                    { 5, false, "yusuf@kawai", "Yusuf Uyar", new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, new byte[] { 61, 213, 21, 54, 160, 195, 189, 2, 176, 76, 30, 165, 65, 163, 165, 29, 244, 226, 246, 136, 236, 43, 57, 219, 212, 43, 209, 14, 102, 151, 145, 58, 27, 217, 85, 57, 105, 7, 81, 82, 122, 33, 139, 207, 164, 138, 228, 147, 180, 77, 160, 126, 1, 14, 39, 208, 6, 171, 85, 152, 11, 22, 140, 88, 54, 164, 244, 61, 59, 88, 142, 157, 86, 37, 143, 29, 203, 197, 239, 196, 120, 127, 3, 203, 48, 182, 11, 231, 83, 106, 49, 0, 148, 73, 235, 23, 107, 204, 241, 244, 128, 119, 157, 167, 132, 214, 215, 65, 219, 218, 241, 0, 215, 188, 116, 36, 232, 167, 221, 153, 233, 146, 34, 228, 204, 91, 95, 192 }, new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, null, 0, "31", true },
                    { 4, false, "ozgur@demir", "Ozgur Chadoglu", new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, new byte[] { 61, 213, 21, 54, 160, 195, 189, 2, 176, 76, 30, 165, 65, 163, 165, 29, 244, 226, 246, 136, 236, 43, 57, 219, 212, 43, 209, 14, 102, 151, 145, 58, 27, 217, 85, 57, 105, 7, 81, 82, 122, 33, 139, 207, 164, 138, 228, 147, 180, 77, 160, 126, 1, 14, 39, 208, 6, 171, 85, 152, 11, 22, 140, 88, 54, 164, 244, 61, 59, 88, 142, 157, 86, 37, 143, 29, 203, 197, 239, 196, 120, 127, 3, 203, 48, 182, 11, 231, 83, 106, 49, 0, 148, 73, 235, 23, 107, 204, 241, 244, 128, 119, 157, 167, 132, 214, 215, 65, 219, 218, 241, 0, 215, 188, 116, 36, 232, 167, 221, 153, 233, 146, 34, 228, 204, 91, 95, 192 }, new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, null, 0, "31", true },
                    { 3, false, "baris@ogun", "Baris Ogun", new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, new byte[] { 61, 213, 21, 54, 160, 195, 189, 2, 176, 76, 30, 165, 65, 163, 165, 29, 244, 226, 246, 136, 236, 43, 57, 219, 212, 43, 209, 14, 102, 151, 145, 58, 27, 217, 85, 57, 105, 7, 81, 82, 122, 33, 139, 207, 164, 138, 228, 147, 180, 77, 160, 126, 1, 14, 39, 208, 6, 171, 85, 152, 11, 22, 140, 88, 54, 164, 244, 61, 59, 88, 142, 157, 86, 37, 143, 29, 203, 197, 239, 196, 120, 127, 3, 203, 48, 182, 11, 231, 83, 106, 49, 0, 148, 73, 235, 23, 107, 204, 241, 244, 128, 119, 157, 167, 132, 214, 215, 65, 219, 218, 241, 0, 215, 188, 116, 36, 232, 167, 221, 153, 233, 146, 34, 228, 204, 91, 95, 192 }, new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, null, 0, "31", true },
                    { 2, false, "eray@tuzun", "Eray Tuzun", new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, new byte[] { 61, 213, 21, 54, 160, 195, 189, 2, 176, 76, 30, 165, 65, 163, 165, 29, 244, 226, 246, 136, 236, 43, 57, 219, 212, 43, 209, 14, 102, 151, 145, 58, 27, 217, 85, 57, 105, 7, 81, 82, 122, 33, 139, 207, 164, 138, 228, 147, 180, 77, 160, 126, 1, 14, 39, 208, 6, 171, 85, 152, 11, 22, 140, 88, 54, 164, 244, 61, 59, 88, 142, 157, 86, 37, 143, 29, 203, 197, 239, 196, 120, 127, 3, 203, 48, 182, 11, 231, 83, 106, 49, 0, 148, 73, 235, 23, 107, 204, 241, 244, 128, 119, 157, 167, 132, 214, 215, 65, 219, 218, 241, 0, 215, 188, 116, 36, 232, 167, 221, 153, 233, 146, 34, 228, 204, 91, 95, 192 }, new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, null, 0, "31", true },
                    { 1, false, "cagri@durgut", "Cagri Durgut", new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, new byte[] { 61, 213, 21, 54, 160, 195, 189, 2, 176, 76, 30, 165, 65, 163, 165, 29, 244, 226, 246, 136, 236, 43, 57, 219, 212, 43, 209, 14, 102, 151, 145, 58, 27, 217, 85, 57, 105, 7, 81, 82, 122, 33, 139, 207, 164, 138, 228, 147, 180, 77, 160, 126, 1, 14, 39, 208, 6, 171, 85, 152, 11, 22, 140, 88, 54, 164, 244, 61, 59, 88, 142, 157, 86, 37, 143, 29, 203, 197, 239, 196, 120, 127, 3, 203, 48, 182, 11, 231, 83, 106, 49, 0, 148, 73, 235, 23, 107, 204, 241, 244, 128, 119, 157, 167, 132, 214, 215, 65, 219, 218, 241, 0, 215, 188, 116, 36, 232, 167, 221, 153, 233, 146, 34, 228, 204, 91, 95, 192 }, new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, null, 0, "31", true },
                    { 11, false, "cagri@eren", "Cagri Eren", new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, new byte[] { 61, 213, 21, 54, 160, 195, 189, 2, 176, 76, 30, 165, 65, 163, 165, 29, 244, 226, 246, 136, 236, 43, 57, 219, 212, 43, 209, 14, 102, 151, 145, 58, 27, 217, 85, 57, 105, 7, 81, 82, 122, 33, 139, 207, 164, 138, 228, 147, 180, 77, 160, 126, 1, 14, 39, 208, 6, 171, 85, 152, 11, 22, 140, 88, 54, 164, 244, 61, 59, 88, 142, 157, 86, 37, 143, 29, 203, 197, 239, 196, 120, 127, 3, 203, 48, 182, 11, 231, 83, 106, 49, 0, 148, 73, 235, 23, 107, 204, 241, 244, 128, 119, 157, 167, 132, 214, 215, 65, 219, 218, 241, 0, 215, 188, 116, 36, 232, 167, 221, 153, 233, 146, 34, 228, 204, 91, 95, 192 }, new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, null, 0, "31", true },
                    { 24, false, "tuna@derbeder", "Tuna Derbeder", new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, new byte[] { 61, 213, 21, 54, 160, 195, 189, 2, 176, 76, 30, 165, 65, 163, 165, 29, 244, 226, 246, 136, 236, 43, 57, 219, 212, 43, 209, 14, 102, 151, 145, 58, 27, 217, 85, 57, 105, 7, 81, 82, 122, 33, 139, 207, 164, 138, 228, 147, 180, 77, 160, 126, 1, 14, 39, 208, 6, 171, 85, 152, 11, 22, 140, 88, 54, 164, 244, 61, 59, 88, 142, 157, 86, 37, 143, 29, 203, 197, 239, 196, 120, 127, 3, 203, 48, 182, 11, 231, 83, 106, 49, 0, 148, 73, 235, 23, 107, 204, 241, 244, 128, 119, 157, 167, 132, 214, 215, 65, 219, 218, 241, 0, 215, 188, 116, 36, 232, 167, 221, 153, 233, 146, 34, 228, 204, 91, 95, 192 }, new byte[] { 144, 105, 74, 54, 44, 37, 150, 89, 118, 252, 254, 120, 49, 44, 12, 196, 92, 250, 240, 149, 27, 228, 129, 85, 170, 21, 67, 252, 133, 42, 116, 112, 53, 66, 83, 28, 254, 253, 249, 61, 199, 123, 151, 186, 197, 195, 72, 116, 227, 7, 10, 0, 58, 255, 122, 48, 207, 35, 122, 247, 123, 144, 30, 78 }, null, 0, "31", true }
                });

            migrationBuilder.InsertData(
                table: "CourseUsers",
                columns: new[] { "CourseId", "UserId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 3, 15 },
                    { 5, 15 },
                    { 1, 16 },
                    { 1, 17 },
                    { 2, 18 },
                    { 4, 19 },
                    { 6, 20 },
                    { 3, 21 },
                    { 4, 22 },
                    { 2, 23 }
                });

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
