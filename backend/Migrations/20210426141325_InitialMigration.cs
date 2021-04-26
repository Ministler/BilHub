﻿using System;
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
                    { 1, "Object Oriented Software Engineering", "spring", new DateTime(2021, 5, 15, 7, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 4, 26, 0, 0, 0, 0, DateTimeKind.Local), 6, 5, "CS 319", new DateTime(2021, 2, 15, 7, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Programming Languages", "spring", new DateTime(2021, 5, 15, 7, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 4, 26, 0, 0, 0, 0, DateTimeKind.Local), 3, 2, "CS 315", new DateTime(2021, 2, 15, 7, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Data Structures and Algorithms", "spring", new DateTime(2021, 5, 15, 7, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 4, 26, 0, 0, 0, 0, DateTimeKind.Local), 1, 1, "CS 202", new DateTime(2021, 2, 15, 7, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Computer Organization and Architecture", "spring", new DateTime(2021, 5, 15, 7, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 4, 26, 0, 0, 0, 0, DateTimeKind.Local), 3, 2, "CS 224", new DateTime(2021, 2, 15, 7, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "History", "spring", new DateTime(2021, 5, 15, 7, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 4, 26, 0, 0, 0, 0, DateTimeKind.Local), 5, 4, "Hist 200", new DateTime(2021, 2, 15, 7, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "Calculus", "spring", new DateTime(2021, 5, 15, 7, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 4, 26, 0, 0, 0, 0, DateTimeKind.Local), 1, 1, "Math 101", new DateTime(2021, 2, 15, 7, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DarkModeStatus", "Email", "Name", "PasswordHash", "PasswordSalt", "SecondPasswordHash", "UserType", "VerificationCode", "VerifiedStatus" },
                values: new object[,]
                {
                    { 15, false, "aynur@dayanik", "Aynur Dayanik", new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, new byte[] { 174, 171, 123, 132, 187, 3, 75, 252, 192, 124, 104, 89, 51, 182, 200, 89, 156, 24, 97, 224, 177, 177, 231, 50, 46, 67, 11, 36, 26, 17, 76, 240, 110, 192, 166, 2, 25, 186, 57, 53, 242, 187, 49, 183, 249, 59, 114, 11, 2, 108, 24, 189, 189, 36, 47, 137, 136, 31, 153, 47, 54, 222, 85, 255, 202, 70, 111, 163, 110, 86, 23, 8, 173, 74, 207, 133, 41, 93, 148, 175, 46, 213, 182, 107, 89, 122, 28, 2, 56, 137, 193, 117, 191, 135, 122, 159, 180, 242, 28, 59, 69, 131, 84, 70, 158, 240, 220, 93, 216, 225, 246, 8, 187, 139, 86, 154, 70, 102, 104, 118, 250, 92, 195, 96, 123, 151, 63, 178 }, new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, 1, "31", true },
                    { 16, false, "erdem@Tuna", "Erdem Tuna", new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, new byte[] { 174, 171, 123, 132, 187, 3, 75, 252, 192, 124, 104, 89, 51, 182, 200, 89, 156, 24, 97, 224, 177, 177, 231, 50, 46, 67, 11, 36, 26, 17, 76, 240, 110, 192, 166, 2, 25, 186, 57, 53, 242, 187, 49, 183, 249, 59, 114, 11, 2, 108, 24, 189, 189, 36, 47, 137, 136, 31, 153, 47, 54, 222, 85, 255, 202, 70, 111, 163, 110, 86, 23, 8, 173, 74, 207, 133, 41, 93, 148, 175, 46, 213, 182, 107, 89, 122, 28, 2, 56, 137, 193, 117, 191, 135, 122, 159, 180, 242, 28, 59, 69, 131, 84, 70, 158, 240, 220, 93, 216, 225, 246, 8, 187, 139, 86, 154, 70, 102, 104, 118, 250, 92, 195, 96, 123, 151, 63, 178 }, new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, 0, "31", true },
                    { 17, false, "elgun@ta", "Elgun TA", new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, new byte[] { 174, 171, 123, 132, 187, 3, 75, 252, 192, 124, 104, 89, 51, 182, 200, 89, 156, 24, 97, 224, 177, 177, 231, 50, 46, 67, 11, 36, 26, 17, 76, 240, 110, 192, 166, 2, 25, 186, 57, 53, 242, 187, 49, 183, 249, 59, 114, 11, 2, 108, 24, 189, 189, 36, 47, 137, 136, 31, 153, 47, 54, 222, 85, 255, 202, 70, 111, 163, 110, 86, 23, 8, 173, 74, 207, 133, 41, 93, 148, 175, 46, 213, 182, 107, 89, 122, 28, 2, 56, 137, 193, 117, 191, 135, 122, 159, 180, 242, 28, 59, 69, 131, 84, 70, 158, 240, 220, 93, 216, 225, 246, 8, 187, 139, 86, 154, 70, 102, 104, 118, 250, 92, 195, 96, 123, 151, 63, 178 }, new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, 0, "31", true },
                    { 18, false, "irem@reis", "Irem Reis", new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, new byte[] { 174, 171, 123, 132, 187, 3, 75, 252, 192, 124, 104, 89, 51, 182, 200, 89, 156, 24, 97, 224, 177, 177, 231, 50, 46, 67, 11, 36, 26, 17, 76, 240, 110, 192, 166, 2, 25, 186, 57, 53, 242, 187, 49, 183, 249, 59, 114, 11, 2, 108, 24, 189, 189, 36, 47, 137, 136, 31, 153, 47, 54, 222, 85, 255, 202, 70, 111, 163, 110, 86, 23, 8, 173, 74, 207, 133, 41, 93, 148, 175, 46, 213, 182, 107, 89, 122, 28, 2, 56, 137, 193, 117, 191, 135, 122, 159, 180, 242, 28, 59, 69, 131, 84, 70, 158, 240, 220, 93, 216, 225, 246, 8, 187, 139, 86, 154, 70, 102, 104, 118, 250, 92, 195, 96, 123, 151, 63, 178 }, new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, 0, "31", true },
                    { 21, false, "ercument@cicek", "Ercument Cicek", new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, new byte[] { 174, 171, 123, 132, 187, 3, 75, 252, 192, 124, 104, 89, 51, 182, 200, 89, 156, 24, 97, 224, 177, 177, 231, 50, 46, 67, 11, 36, 26, 17, 76, 240, 110, 192, 166, 2, 25, 186, 57, 53, 242, 187, 49, 183, 249, 59, 114, 11, 2, 108, 24, 189, 189, 36, 47, 137, 136, 31, 153, 47, 54, 222, 85, 255, 202, 70, 111, 163, 110, 86, 23, 8, 173, 74, 207, 133, 41, 93, 148, 175, 46, 213, 182, 107, 89, 122, 28, 2, 56, 137, 193, 117, 191, 135, 122, 159, 180, 242, 28, 59, 69, 131, 84, 70, 158, 240, 220, 93, 216, 225, 246, 8, 187, 139, 86, 154, 70, 102, 104, 118, 250, 92, 195, 96, 123, 151, 63, 178 }, new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, 1, "31", true },
                    { 20, false, "can@alkan", "Can Alkan", new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, new byte[] { 174, 171, 123, 132, 187, 3, 75, 252, 192, 124, 104, 89, 51, 182, 200, 89, 156, 24, 97, 224, 177, 177, 231, 50, 46, 67, 11, 36, 26, 17, 76, 240, 110, 192, 166, 2, 25, 186, 57, 53, 242, 187, 49, 183, 249, 59, 114, 11, 2, 108, 24, 189, 189, 36, 47, 137, 136, 31, 153, 47, 54, 222, 85, 255, 202, 70, 111, 163, 110, 86, 23, 8, 173, 74, 207, 133, 41, 93, 148, 175, 46, 213, 182, 107, 89, 122, 28, 2, 56, 137, 193, 117, 191, 135, 122, 159, 180, 242, 28, 59, 69, 131, 84, 70, 158, 240, 220, 93, 216, 225, 246, 8, 187, 139, 86, 154, 70, 102, 104, 118, 250, 92, 195, 96, 123, 151, 63, 178 }, new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, 1, "31", true },
                    { 14, false, "fuat@schwarzenegger", "Fuat Schwarzengger", new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, new byte[] { 174, 171, 123, 132, 187, 3, 75, 252, 192, 124, 104, 89, 51, 182, 200, 89, 156, 24, 97, 224, 177, 177, 231, 50, 46, 67, 11, 36, 26, 17, 76, 240, 110, 192, 166, 2, 25, 186, 57, 53, 242, 187, 49, 183, 249, 59, 114, 11, 2, 108, 24, 189, 189, 36, 47, 137, 136, 31, 153, 47, 54, 222, 85, 255, 202, 70, 111, 163, 110, 86, 23, 8, 173, 74, 207, 133, 41, 93, 148, 175, 46, 213, 182, 107, 89, 122, 28, 2, 56, 137, 193, 117, 191, 135, 122, 159, 180, 242, 28, 59, 69, 131, 84, 70, 158, 240, 220, 93, 216, 225, 246, 8, 187, 139, 86, 154, 70, 102, 104, 118, 250, 92, 195, 96, 123, 151, 63, 178 }, new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, 0, "31", true },
                    { 22, false, "alper@karel", "Alper Sarikan", new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, new byte[] { 174, 171, 123, 132, 187, 3, 75, 252, 192, 124, 104, 89, 51, 182, 200, 89, 156, 24, 97, 224, 177, 177, 231, 50, 46, 67, 11, 36, 26, 17, 76, 240, 110, 192, 166, 2, 25, 186, 57, 53, 242, 187, 49, 183, 249, 59, 114, 11, 2, 108, 24, 189, 189, 36, 47, 137, 136, 31, 153, 47, 54, 222, 85, 255, 202, 70, 111, 163, 110, 86, 23, 8, 173, 74, 207, 133, 41, 93, 148, 175, 46, 213, 182, 107, 89, 122, 28, 2, 56, 137, 193, 117, 191, 135, 122, 159, 180, 242, 28, 59, 69, 131, 84, 70, 158, 240, 220, 93, 216, 225, 246, 8, 187, 139, 86, 154, 70, 102, 104, 118, 250, 92, 195, 96, 123, 151, 63, 178 }, new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, 1, "31", true },
                    { 23, false, "altay@guvenir", "Altay Guvenir", new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, new byte[] { 174, 171, 123, 132, 187, 3, 75, 252, 192, 124, 104, 89, 51, 182, 200, 89, 156, 24, 97, 224, 177, 177, 231, 50, 46, 67, 11, 36, 26, 17, 76, 240, 110, 192, 166, 2, 25, 186, 57, 53, 242, 187, 49, 183, 249, 59, 114, 11, 2, 108, 24, 189, 189, 36, 47, 137, 136, 31, 153, 47, 54, 222, 85, 255, 202, 70, 111, 163, 110, 86, 23, 8, 173, 74, 207, 133, 41, 93, 148, 175, 46, 213, 182, 107, 89, 122, 28, 2, 56, 137, 193, 117, 191, 135, 122, 159, 180, 242, 28, 59, 69, 131, 84, 70, 158, 240, 220, 93, 216, 225, 246, 8, 187, 139, 86, 154, 70, 102, 104, 118, 250, 92, 195, 96, 123, 151, 63, 178 }, new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, 1, "31", true },
                    { 19, false, "fazli@can", "Fazli Can", new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, new byte[] { 174, 171, 123, 132, 187, 3, 75, 252, 192, 124, 104, 89, 51, 182, 200, 89, 156, 24, 97, 224, 177, 177, 231, 50, 46, 67, 11, 36, 26, 17, 76, 240, 110, 192, 166, 2, 25, 186, 57, 53, 242, 187, 49, 183, 249, 59, 114, 11, 2, 108, 24, 189, 189, 36, 47, 137, 136, 31, 153, 47, 54, 222, 85, 255, 202, 70, 111, 163, 110, 86, 23, 8, 173, 74, 207, 133, 41, 93, 148, 175, 46, 213, 182, 107, 89, 122, 28, 2, 56, 137, 193, 117, 191, 135, 122, 159, 180, 242, 28, 59, 69, 131, 84, 70, 158, 240, 220, 93, 216, 225, 246, 8, 187, 139, 86, 154, 70, 102, 104, 118, 250, 92, 195, 96, 123, 151, 63, 178 }, new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, 1, "31", true },
                    { 13, false, "onur@korkmaz", "Onur Korkmaz", new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, new byte[] { 174, 171, 123, 132, 187, 3, 75, 252, 192, 124, 104, 89, 51, 182, 200, 89, 156, 24, 97, 224, 177, 177, 231, 50, 46, 67, 11, 36, 26, 17, 76, 240, 110, 192, 166, 2, 25, 186, 57, 53, 242, 187, 49, 183, 249, 59, 114, 11, 2, 108, 24, 189, 189, 36, 47, 137, 136, 31, 153, 47, 54, 222, 85, 255, 202, 70, 111, 163, 110, 86, 23, 8, 173, 74, 207, 133, 41, 93, 148, 175, 46, 213, 182, 107, 89, 122, 28, 2, 56, 137, 193, 117, 191, 135, 122, 159, 180, 242, 28, 59, 69, 131, 84, 70, 158, 240, 220, 93, 216, 225, 246, 8, 187, 139, 86, 154, 70, 102, 104, 118, 250, 92, 195, 96, 123, 151, 63, 178 }, new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, 0, "31", true },
                    { 10, false, "hami@mert", "Hami Mert", new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, new byte[] { 174, 171, 123, 132, 187, 3, 75, 252, 192, 124, 104, 89, 51, 182, 200, 89, 156, 24, 97, 224, 177, 177, 231, 50, 46, 67, 11, 36, 26, 17, 76, 240, 110, 192, 166, 2, 25, 186, 57, 53, 242, 187, 49, 183, 249, 59, 114, 11, 2, 108, 24, 189, 189, 36, 47, 137, 136, 31, 153, 47, 54, 222, 85, 255, 202, 70, 111, 163, 110, 86, 23, 8, 173, 74, 207, 133, 41, 93, 148, 175, 46, 213, 182, 107, 89, 122, 28, 2, 56, 137, 193, 117, 191, 135, 122, 159, 180, 242, 28, 59, 69, 131, 84, 70, 158, 240, 220, 93, 216, 225, 246, 8, 187, 139, 86, 154, 70, 102, 104, 118, 250, 92, 195, 96, 123, 151, 63, 178 }, new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, 0, "31", true },
                    { 11, false, "cagri@eren", "Cagri Eren", new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, new byte[] { 174, 171, 123, 132, 187, 3, 75, 252, 192, 124, 104, 89, 51, 182, 200, 89, 156, 24, 97, 224, 177, 177, 231, 50, 46, 67, 11, 36, 26, 17, 76, 240, 110, 192, 166, 2, 25, 186, 57, 53, 242, 187, 49, 183, 249, 59, 114, 11, 2, 108, 24, 189, 189, 36, 47, 137, 136, 31, 153, 47, 54, 222, 85, 255, 202, 70, 111, 163, 110, 86, 23, 8, 173, 74, 207, 133, 41, 93, 148, 175, 46, 213, 182, 107, 89, 122, 28, 2, 56, 137, 193, 117, 191, 135, 122, 159, 180, 242, 28, 59, 69, 131, 84, 70, 158, 240, 220, 93, 216, 225, 246, 8, 187, 139, 86, 154, 70, 102, 104, 118, 250, 92, 195, 96, 123, 151, 63, 178 }, new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, 0, "31", true },
                    { 24, false, "tuna@derbeder", "Tuna Derbeder", new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, new byte[] { 174, 171, 123, 132, 187, 3, 75, 252, 192, 124, 104, 89, 51, 182, 200, 89, 156, 24, 97, 224, 177, 177, 231, 50, 46, 67, 11, 36, 26, 17, 76, 240, 110, 192, 166, 2, 25, 186, 57, 53, 242, 187, 49, 183, 249, 59, 114, 11, 2, 108, 24, 189, 189, 36, 47, 137, 136, 31, 153, 47, 54, 222, 85, 255, 202, 70, 111, 163, 110, 86, 23, 8, 173, 74, 207, 133, 41, 93, 148, 175, 46, 213, 182, 107, 89, 122, 28, 2, 56, 137, 193, 117, 191, 135, 122, 159, 180, 242, 28, 59, 69, 131, 84, 70, 158, 240, 220, 93, 216, 225, 246, 8, 187, 139, 86, 154, 70, 102, 104, 118, 250, 92, 195, 96, 123, 151, 63, 178 }, new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, 0, "31", true },
                    { 9, false, "funda@tan", "Funda Tan", new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, new byte[] { 174, 171, 123, 132, 187, 3, 75, 252, 192, 124, 104, 89, 51, 182, 200, 89, 156, 24, 97, 224, 177, 177, 231, 50, 46, 67, 11, 36, 26, 17, 76, 240, 110, 192, 166, 2, 25, 186, 57, 53, 242, 187, 49, 183, 249, 59, 114, 11, 2, 108, 24, 189, 189, 36, 47, 137, 136, 31, 153, 47, 54, 222, 85, 255, 202, 70, 111, 163, 110, 86, 23, 8, 173, 74, 207, 133, 41, 93, 148, 175, 46, 213, 182, 107, 89, 122, 28, 2, 56, 137, 193, 117, 191, 135, 122, 159, 180, 242, 28, 59, 69, 131, 84, 70, 158, 240, 220, 93, 216, 225, 246, 8, 187, 139, 86, 154, 70, 102, 104, 118, 250, 92, 195, 96, 123, 151, 63, 178 }, new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, 0, "31", true },
                    { 8, false, "berke@ceran", "HOCAM SIMDI BIZ SOYLE BI SISTEM DUSUNDUK DE", new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, new byte[] { 174, 171, 123, 132, 187, 3, 75, 252, 192, 124, 104, 89, 51, 182, 200, 89, 156, 24, 97, 224, 177, 177, 231, 50, 46, 67, 11, 36, 26, 17, 76, 240, 110, 192, 166, 2, 25, 186, 57, 53, 242, 187, 49, 183, 249, 59, 114, 11, 2, 108, 24, 189, 189, 36, 47, 137, 136, 31, 153, 47, 54, 222, 85, 255, 202, 70, 111, 163, 110, 86, 23, 8, 173, 74, 207, 133, 41, 93, 148, 175, 46, 213, 182, 107, 89, 122, 28, 2, 56, 137, 193, 117, 191, 135, 122, 159, 180, 242, 28, 59, 69, 131, 84, 70, 158, 240, 220, 93, 216, 225, 246, 8, 187, 139, 86, 154, 70, 102, 104, 118, 250, 92, 195, 96, 123, 151, 63, 178 }, new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, 0, "31", true },
                    { 7, false, "oguzhan@ozcelik", "oguzhan ozcelik", new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, new byte[] { 174, 171, 123, 132, 187, 3, 75, 252, 192, 124, 104, 89, 51, 182, 200, 89, 156, 24, 97, 224, 177, 177, 231, 50, 46, 67, 11, 36, 26, 17, 76, 240, 110, 192, 166, 2, 25, 186, 57, 53, 242, 187, 49, 183, 249, 59, 114, 11, 2, 108, 24, 189, 189, 36, 47, 137, 136, 31, 153, 47, 54, 222, 85, 255, 202, 70, 111, 163, 110, 86, 23, 8, 173, 74, 207, 133, 41, 93, 148, 175, 46, 213, 182, 107, 89, 122, 28, 2, 56, 137, 193, 117, 191, 135, 122, 159, 180, 242, 28, 59, 69, 131, 84, 70, 158, 240, 220, 93, 216, 225, 246, 8, 187, 139, 86, 154, 70, 102, 104, 118, 250, 92, 195, 96, 123, 151, 63, 178 }, new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, 0, "31", true },
                    { 6, false, "aybala@karakaya", "Aybala karakaya", new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, new byte[] { 174, 171, 123, 132, 187, 3, 75, 252, 192, 124, 104, 89, 51, 182, 200, 89, 156, 24, 97, 224, 177, 177, 231, 50, 46, 67, 11, 36, 26, 17, 76, 240, 110, 192, 166, 2, 25, 186, 57, 53, 242, 187, 49, 183, 249, 59, 114, 11, 2, 108, 24, 189, 189, 36, 47, 137, 136, 31, 153, 47, 54, 222, 85, 255, 202, 70, 111, 163, 110, 86, 23, 8, 173, 74, 207, 133, 41, 93, 148, 175, 46, 213, 182, 107, 89, 122, 28, 2, 56, 137, 193, 117, 191, 135, 122, 159, 180, 242, 28, 59, 69, 131, 84, 70, 158, 240, 220, 93, 216, 225, 246, 8, 187, 139, 86, 154, 70, 102, 104, 118, 250, 92, 195, 96, 123, 151, 63, 178 }, new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, 0, "31", true },
                    { 5, false, "yusuf@kawai", "Yusuf Uyar", new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, new byte[] { 174, 171, 123, 132, 187, 3, 75, 252, 192, 124, 104, 89, 51, 182, 200, 89, 156, 24, 97, 224, 177, 177, 231, 50, 46, 67, 11, 36, 26, 17, 76, 240, 110, 192, 166, 2, 25, 186, 57, 53, 242, 187, 49, 183, 249, 59, 114, 11, 2, 108, 24, 189, 189, 36, 47, 137, 136, 31, 153, 47, 54, 222, 85, 255, 202, 70, 111, 163, 110, 86, 23, 8, 173, 74, 207, 133, 41, 93, 148, 175, 46, 213, 182, 107, 89, 122, 28, 2, 56, 137, 193, 117, 191, 135, 122, 159, 180, 242, 28, 59, 69, 131, 84, 70, 158, 240, 220, 93, 216, 225, 246, 8, 187, 139, 86, 154, 70, 102, 104, 118, 250, 92, 195, 96, 123, 151, 63, 178 }, new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, 0, "31", true },
                    { 4, false, "ozgur@demir", "Ozgur Chadoglu", new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, new byte[] { 174, 171, 123, 132, 187, 3, 75, 252, 192, 124, 104, 89, 51, 182, 200, 89, 156, 24, 97, 224, 177, 177, 231, 50, 46, 67, 11, 36, 26, 17, 76, 240, 110, 192, 166, 2, 25, 186, 57, 53, 242, 187, 49, 183, 249, 59, 114, 11, 2, 108, 24, 189, 189, 36, 47, 137, 136, 31, 153, 47, 54, 222, 85, 255, 202, 70, 111, 163, 110, 86, 23, 8, 173, 74, 207, 133, 41, 93, 148, 175, 46, 213, 182, 107, 89, 122, 28, 2, 56, 137, 193, 117, 191, 135, 122, 159, 180, 242, 28, 59, 69, 131, 84, 70, 158, 240, 220, 93, 216, 225, 246, 8, 187, 139, 86, 154, 70, 102, 104, 118, 250, 92, 195, 96, 123, 151, 63, 178 }, new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, 0, "31", true },
                    { 3, false, "baris@ogun", "Baris Ogun", new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, new byte[] { 174, 171, 123, 132, 187, 3, 75, 252, 192, 124, 104, 89, 51, 182, 200, 89, 156, 24, 97, 224, 177, 177, 231, 50, 46, 67, 11, 36, 26, 17, 76, 240, 110, 192, 166, 2, 25, 186, 57, 53, 242, 187, 49, 183, 249, 59, 114, 11, 2, 108, 24, 189, 189, 36, 47, 137, 136, 31, 153, 47, 54, 222, 85, 255, 202, 70, 111, 163, 110, 86, 23, 8, 173, 74, 207, 133, 41, 93, 148, 175, 46, 213, 182, 107, 89, 122, 28, 2, 56, 137, 193, 117, 191, 135, 122, 159, 180, 242, 28, 59, 69, 131, 84, 70, 158, 240, 220, 93, 216, 225, 246, 8, 187, 139, 86, 154, 70, 102, 104, 118, 250, 92, 195, 96, 123, 151, 63, 178 }, new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, 0, "31", true },
                    { 2, false, "eray@tuzun", "Eray Tuzun", new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, new byte[] { 174, 171, 123, 132, 187, 3, 75, 252, 192, 124, 104, 89, 51, 182, 200, 89, 156, 24, 97, 224, 177, 177, 231, 50, 46, 67, 11, 36, 26, 17, 76, 240, 110, 192, 166, 2, 25, 186, 57, 53, 242, 187, 49, 183, 249, 59, 114, 11, 2, 108, 24, 189, 189, 36, 47, 137, 136, 31, 153, 47, 54, 222, 85, 255, 202, 70, 111, 163, 110, 86, 23, 8, 173, 74, 207, 133, 41, 93, 148, 175, 46, 213, 182, 107, 89, 122, 28, 2, 56, 137, 193, 117, 191, 135, 122, 159, 180, 242, 28, 59, 69, 131, 84, 70, 158, 240, 220, 93, 216, 225, 246, 8, 187, 139, 86, 154, 70, 102, 104, 118, 250, 92, 195, 96, 123, 151, 63, 178 }, new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, 0, "31", true },
                    { 1, false, "cagri@durgut", "Cagri Durgut", new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, new byte[] { 174, 171, 123, 132, 187, 3, 75, 252, 192, 124, 104, 89, 51, 182, 200, 89, 156, 24, 97, 224, 177, 177, 231, 50, 46, 67, 11, 36, 26, 17, 76, 240, 110, 192, 166, 2, 25, 186, 57, 53, 242, 187, 49, 183, 249, 59, 114, 11, 2, 108, 24, 189, 189, 36, 47, 137, 136, 31, 153, 47, 54, 222, 85, 255, 202, 70, 111, 163, 110, 86, 23, 8, 173, 74, 207, 133, 41, 93, 148, 175, 46, 213, 182, 107, 89, 122, 28, 2, 56, 137, 193, 117, 191, 135, 122, 159, 180, 242, 28, 59, 69, 131, 84, 70, 158, 240, 220, 93, 216, 225, 246, 8, 187, 139, 86, 154, 70, 102, 104, 118, 250, 92, 195, 96, 123, 151, 63, 178 }, new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, 0, "31", true },
                    { 12, false, "guven@gerger", "Guven Gerger", new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, new byte[] { 174, 171, 123, 132, 187, 3, 75, 252, 192, 124, 104, 89, 51, 182, 200, 89, 156, 24, 97, 224, 177, 177, 231, 50, 46, 67, 11, 36, 26, 17, 76, 240, 110, 192, 166, 2, 25, 186, 57, 53, 242, 187, 49, 183, 249, 59, 114, 11, 2, 108, 24, 189, 189, 36, 47, 137, 136, 31, 153, 47, 54, 222, 85, 255, 202, 70, 111, 163, 110, 86, 23, 8, 173, 74, 207, 133, 41, 93, 148, 175, 46, 213, 182, 107, 89, 122, 28, 2, 56, 137, 193, 117, 191, 135, 122, 159, 180, 242, 28, 59, 69, 131, 84, 70, 158, 240, 220, 93, 216, 225, 246, 8, 187, 139, 86, 154, 70, 102, 104, 118, 250, 92, 195, 96, 123, 151, 63, 178 }, new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, 0, "31", true },
                    { 25, false, "abdul@razak", "Abdul Razak", new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, new byte[] { 174, 171, 123, 132, 187, 3, 75, 252, 192, 124, 104, 89, 51, 182, 200, 89, 156, 24, 97, 224, 177, 177, 231, 50, 46, 67, 11, 36, 26, 17, 76, 240, 110, 192, 166, 2, 25, 186, 57, 53, 242, 187, 49, 183, 249, 59, 114, 11, 2, 108, 24, 189, 189, 36, 47, 137, 136, 31, 153, 47, 54, 222, 85, 255, 202, 70, 111, 163, 110, 86, 23, 8, 173, 74, 207, 133, 41, 93, 148, 175, 46, 213, 182, 107, 89, 122, 28, 2, 56, 137, 193, 117, 191, 135, 122, 159, 180, 242, 28, 59, 69, 131, 84, 70, 158, 240, 220, 93, 216, 225, 246, 8, 187, 139, 86, 154, 70, 102, 104, 118, 250, 92, 195, 96, 123, 151, 63, 178 }, new byte[] { 88, 171, 70, 24, 138, 166, 189, 130, 198, 49, 254, 149, 215, 223, 122, 30, 44, 137, 93, 214, 100, 143, 172, 131, 117, 93, 135, 255, 215, 2, 192, 174, 213, 2, 214, 129, 121, 211, 33, 102, 245, 183, 14, 5, 175, 29, 17, 130, 216, 179, 90, 89, 183, 79, 164, 158, 122, 76, 228, 70, 36, 22, 87, 204 }, 0, "31", true }
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
                    { 3, "pdf-doc-docx", 1, "Odev iteration 1", true, 1, new DateTime(2021, 4, 26, 17, 13, 24, 881, DateTimeKind.Local).AddTicks(9761), new DateTime(2021, 4, 15, 7, 0, 0, 0, DateTimeKind.Unspecified), true, 4096, true },
                    { 4, "pdf-doc-docx", 1, "Odev iteration 2", true, 1, new DateTime(2021, 4, 26, 17, 13, 24, 883, DateTimeKind.Local).AddTicks(3603), new DateTime(2021, 4, 15, 7, 0, 0, 0, DateTimeKind.Unspecified), true, 4096, true },
                    { 1, "pdf,doc,docx", 1, "Odev", true, 1, new DateTime(2021, 4, 26, 17, 13, 24, 888, DateTimeKind.Local).AddTicks(6689), new DateTime(2021, 5, 15, 7, 0, 0, 0, DateTimeKind.Unspecified), true, 1024, true },
                    { 2, "pdf,doc,docx", 1, "315 proje", false, 2, new DateTime(2021, 4, 26, 17, 13, 24, 888, DateTimeKind.Local).AddTicks(6712), new DateTime(2021, 5, 15, 7, 0, 0, 0, DateTimeKind.Unspecified), false, 1024, false }
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
                values: new object[] { 1, 1, 1, 1, null, false, 1, new DateTime(2021, 4, 26, 17, 13, 24, 884, DateTimeKind.Local).AddTicks(8318) });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "CommentText", "CommentedSubmissionId", "CommentedUserId", "CreatedAt", "FileAttachmentAvailability", "FilePath", "Grade", "MaxGrade" },
                values: new object[] { 1, "nays", 1, 2, new DateTime(2021, 4, 26, 17, 13, 24, 885, DateTimeKind.Local).AddTicks(3744), true, null, 9.4m, 10.0m });

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
