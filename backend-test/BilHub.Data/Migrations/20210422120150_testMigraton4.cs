using Microsoft.EntityFrameworkCore.Migrations;

namespace BilHub.Data.Migrations
{
    public partial class testMigraton4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignment_Courses_CourseId",
                table: "Assignment");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignment_User_PublishedUsedId",
                table: "Assignment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Submission_CommentedSubmissionId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_User_CommentedUserId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_PeerGradeAssignment_PeerGradeAssignmentId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupSize_Courses_CourseId",
                table: "GroupSize");

            migrationBuilder.DropForeignKey(
                name: "FK_InstructorCourse_Courses_CourseId",
                table: "InstructorCourse");

            migrationBuilder.DropForeignKey(
                name: "FK_InstructorCourse_Instructors_InstructorId",
                table: "InstructorCourse");

            migrationBuilder.DropForeignKey(
                name: "FK_JoinRequest_ProjectGroup_RequestedGroupId",
                table: "JoinRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_JoinRequest_Section_SectionId",
                table: "JoinRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_JoinRequest_Students_RequestingStudentId",
                table: "JoinRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_MergeRequest_ProjectGroup_Id",
                table: "MergeRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_MergeRequest_ProjectGroup_SenderGroupId",
                table: "MergeRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_MergeRequest_Section_SectionId",
                table: "MergeRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_PeerGradeAssignment_User_PublishedUserId",
                table: "PeerGradeAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectGrade_Courses_CourseId",
                table: "ProjectGrade");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectGrade_ProjectGroup_GradedProjectGroupId",
                table: "ProjectGrade");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectGrade_Students_GradingStudentId",
                table: "ProjectGrade");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectGroup_Courses_AffiliatedCourseId",
                table: "ProjectGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectGroup_Section_SectionId",
                table: "ProjectGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_Section_Courses_CourseId",
                table: "Section");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourse_Courses_CourseId",
                table: "StudentCourse");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourse_Students_StudentId",
                table: "StudentCourse");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentJoinRequest_JoinRequest_JoinRequestId",
                table: "StudentJoinRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentJoinRequest_Students_StudentId",
                table: "StudentJoinRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentMergeRequest_MergeRequest_MergeRequestId",
                table: "StudentMergeRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentMergeRequest_Students_StudentId",
                table: "StudentMergeRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentProjectGroup_ProjectGroup_ProjectGroupId",
                table: "StudentProjectGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentProjectGroup_Students_StudentId",
                table: "StudentProjectGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Section_SectionId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Submission_Assignment_AffiliatedAssignmentId",
                table: "Submission");

            migrationBuilder.DropForeignKey(
                name: "FK_Submission_Courses_CourseId",
                table: "Submission");

            migrationBuilder.DropForeignKey(
                name: "FK_Submission_ProjectGroup_AffiliatedGroupId",
                table: "Submission");

            migrationBuilder.DropForeignKey(
                name: "FK_UnvotedJoinRequest_JoinRequest_JoinRequestId",
                table: "UnvotedJoinRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_UnvotedJoinRequest_Students_StudentId",
                table: "UnvotedJoinRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UnvotedJoinRequest",
                table: "UnvotedJoinRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Submission",
                table: "Submission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentProjectGroup",
                table: "StudentProjectGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentMergeRequest",
                table: "StudentMergeRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentJoinRequest",
                table: "StudentJoinRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentCourse",
                table: "StudentCourse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Section",
                table: "Section");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectGroup",
                table: "ProjectGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectGrade",
                table: "ProjectGrade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PeerGradeAssignment",
                table: "PeerGradeAssignment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MergeRequest",
                table: "MergeRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JoinRequest",
                table: "JoinRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InstructorCourse",
                table: "InstructorCourse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupSize",
                table: "GroupSize");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assignment",
                table: "Assignment");

            migrationBuilder.RenameTable(
                name: "UnvotedJoinRequest",
                newName: "UnvotedJoinRequests");

            migrationBuilder.RenameTable(
                name: "Submission",
                newName: "Submissions");

            migrationBuilder.RenameTable(
                name: "StudentProjectGroup",
                newName: "StudentProjectGroups");

            migrationBuilder.RenameTable(
                name: "StudentMergeRequest",
                newName: "StudentMergeRequests");

            migrationBuilder.RenameTable(
                name: "StudentJoinRequest",
                newName: "StudentJoinRequests");

            migrationBuilder.RenameTable(
                name: "StudentCourse",
                newName: "StudentCourses");

            migrationBuilder.RenameTable(
                name: "Section",
                newName: "Sections");

            migrationBuilder.RenameTable(
                name: "ProjectGroup",
                newName: "ProjectGroups");

            migrationBuilder.RenameTable(
                name: "ProjectGrade",
                newName: "ProjectGrades");

            migrationBuilder.RenameTable(
                name: "PeerGradeAssignment",
                newName: "PeerGradeAssignments");

            migrationBuilder.RenameTable(
                name: "MergeRequest",
                newName: "MergeRequests");

            migrationBuilder.RenameTable(
                name: "JoinRequest",
                newName: "JoinRequests");

            migrationBuilder.RenameTable(
                name: "InstructorCourse",
                newName: "InstructorCourses");

            migrationBuilder.RenameTable(
                name: "GroupSize",
                newName: "GroupSizes");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "Comments");

            migrationBuilder.RenameTable(
                name: "Assignment",
                newName: "Assignments");

            migrationBuilder.RenameIndex(
                name: "IX_UnvotedJoinRequest_JoinRequestId",
                table: "UnvotedJoinRequests",
                newName: "IX_UnvotedJoinRequests_JoinRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_Submission_CourseId",
                table: "Submissions",
                newName: "IX_Submissions_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Submission_AffiliatedGroupId",
                table: "Submissions",
                newName: "IX_Submissions_AffiliatedGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Submission_AffiliatedAssignmentId",
                table: "Submissions",
                newName: "IX_Submissions_AffiliatedAssignmentId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentProjectGroup_ProjectGroupId",
                table: "StudentProjectGroups",
                newName: "IX_StudentProjectGroups_ProjectGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentMergeRequest_MergeRequestId",
                table: "StudentMergeRequests",
                newName: "IX_StudentMergeRequests_MergeRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentJoinRequest_JoinRequestId",
                table: "StudentJoinRequests",
                newName: "IX_StudentJoinRequests_JoinRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentCourse_CourseId",
                table: "StudentCourses",
                newName: "IX_StudentCourses_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Section_CourseId",
                table: "Sections",
                newName: "IX_Sections_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectGroup_SectionId",
                table: "ProjectGroups",
                newName: "IX_ProjectGroups_SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectGroup_AffiliatedCourseId",
                table: "ProjectGroups",
                newName: "IX_ProjectGroups_AffiliatedCourseId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectGrade_GradingStudentId",
                table: "ProjectGrades",
                newName: "IX_ProjectGrades_GradingStudentId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectGrade_GradedProjectGroupId",
                table: "ProjectGrades",
                newName: "IX_ProjectGrades_GradedProjectGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectGrade_CourseId",
                table: "ProjectGrades",
                newName: "IX_ProjectGrades_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_PeerGradeAssignment_PublishedUserId",
                table: "PeerGradeAssignments",
                newName: "IX_PeerGradeAssignments_PublishedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_MergeRequest_SenderGroupId",
                table: "MergeRequests",
                newName: "IX_MergeRequests_SenderGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_MergeRequest_SectionId",
                table: "MergeRequests",
                newName: "IX_MergeRequests_SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_JoinRequest_SectionId",
                table: "JoinRequests",
                newName: "IX_JoinRequests_SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_JoinRequest_RequestingStudentId",
                table: "JoinRequests",
                newName: "IX_JoinRequests_RequestingStudentId");

            migrationBuilder.RenameIndex(
                name: "IX_JoinRequest_RequestedGroupId",
                table: "JoinRequests",
                newName: "IX_JoinRequests_RequestedGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_InstructorCourse_InstructorId",
                table: "InstructorCourses",
                newName: "IX_InstructorCourses_InstructorId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupSize_CourseId",
                table: "GroupSizes",
                newName: "IX_GroupSizes_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_CommentedUserId",
                table: "Comments",
                newName: "IX_Comments_CommentedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_CommentedSubmissionId",
                table: "Comments",
                newName: "IX_Comments_CommentedSubmissionId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignment_PublishedUsedId",
                table: "Assignments",
                newName: "IX_Assignments_PublishedUsedId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignment_CourseId",
                table: "Assignments",
                newName: "IX_Assignments_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UnvotedJoinRequests",
                table: "UnvotedJoinRequests",
                columns: new[] { "StudentId", "JoinRequestId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Submissions",
                table: "Submissions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentProjectGroups",
                table: "StudentProjectGroups",
                columns: new[] { "StudentId", "ProjectGroupId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentMergeRequests",
                table: "StudentMergeRequests",
                columns: new[] { "StudentId", "MergeRequestId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentJoinRequests",
                table: "StudentJoinRequests",
                columns: new[] { "StudentId", "JoinRequestId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentCourses",
                table: "StudentCourses",
                columns: new[] { "StudentId", "CourseId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sections",
                table: "Sections",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectGroups",
                table: "ProjectGroups",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectGrades",
                table: "ProjectGrades",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PeerGradeAssignments",
                table: "PeerGradeAssignments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MergeRequests",
                table: "MergeRequests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JoinRequests",
                table: "JoinRequests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InstructorCourses",
                table: "InstructorCourses",
                columns: new[] { "CourseId", "InstructorId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupSizes",
                table: "GroupSizes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assignments",
                table: "Assignments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PeerGrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReviewerId = table.Column<int>(type: "int", nullable: true),
                    RevieweeId = table.Column<int>(type: "int", nullable: true),
                    MaxGrade = table.Column<double>(type: "float", nullable: false),
                    Grade = table.Column<double>(type: "float", nullable: false),
                    AdditionalComment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeerGrades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PeerGrades_Students_RevieweeId",
                        column: x => x.RevieweeId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PeerGrades_Students_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PeerGrades_RevieweeId",
                table: "PeerGrades",
                column: "RevieweeId");

            migrationBuilder.CreateIndex(
                name: "IX_PeerGrades_ReviewerId",
                table: "PeerGrades",
                column: "ReviewerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Courses_CourseId",
                table: "Assignments",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_User_PublishedUsedId",
                table: "Assignments",
                column: "PublishedUsedId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Submissions_CommentedSubmissionId",
                table: "Comments",
                column: "CommentedSubmissionId",
                principalTable: "Submissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_User_CommentedUserId",
                table: "Comments",
                column: "CommentedUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_PeerGradeAssignments_PeerGradeAssignmentId",
                table: "Courses",
                column: "PeerGradeAssignmentId",
                principalTable: "PeerGradeAssignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSizes_Courses_CourseId",
                table: "GroupSizes",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorCourses_Courses_CourseId",
                table: "InstructorCourses",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorCourses_Instructors_InstructorId",
                table: "InstructorCourses",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JoinRequests_ProjectGroups_RequestedGroupId",
                table: "JoinRequests",
                column: "RequestedGroupId",
                principalTable: "ProjectGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JoinRequests_Sections_SectionId",
                table: "JoinRequests",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JoinRequests_Students_RequestingStudentId",
                table: "JoinRequests",
                column: "RequestingStudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MergeRequests_ProjectGroups_Id",
                table: "MergeRequests",
                column: "Id",
                principalTable: "ProjectGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MergeRequests_ProjectGroups_SenderGroupId",
                table: "MergeRequests",
                column: "SenderGroupId",
                principalTable: "ProjectGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MergeRequests_Sections_SectionId",
                table: "MergeRequests",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PeerGradeAssignments_User_PublishedUserId",
                table: "PeerGradeAssignments",
                column: "PublishedUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectGrades_Courses_CourseId",
                table: "ProjectGrades",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectGrades_ProjectGroups_GradedProjectGroupId",
                table: "ProjectGrades",
                column: "GradedProjectGroupId",
                principalTable: "ProjectGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectGrades_Students_GradingStudentId",
                table: "ProjectGrades",
                column: "GradingStudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectGroups_Courses_AffiliatedCourseId",
                table: "ProjectGroups",
                column: "AffiliatedCourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectGroups_Sections_SectionId",
                table: "ProjectGroups",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Courses_CourseId",
                table: "Sections",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourses_Courses_CourseId",
                table: "StudentCourses",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourses_Students_StudentId",
                table: "StudentCourses",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentJoinRequests_JoinRequests_JoinRequestId",
                table: "StudentJoinRequests",
                column: "JoinRequestId",
                principalTable: "JoinRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentJoinRequests_Students_StudentId",
                table: "StudentJoinRequests",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentMergeRequests_MergeRequests_MergeRequestId",
                table: "StudentMergeRequests",
                column: "MergeRequestId",
                principalTable: "MergeRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentMergeRequests_Students_StudentId",
                table: "StudentMergeRequests",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentProjectGroups_ProjectGroups_ProjectGroupId",
                table: "StudentProjectGroups",
                column: "ProjectGroupId",
                principalTable: "ProjectGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentProjectGroups_Students_StudentId",
                table: "StudentProjectGroups",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Sections_SectionId",
                table: "Students",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Assignments_AffiliatedAssignmentId",
                table: "Submissions",
                column: "AffiliatedAssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Courses_CourseId",
                table: "Submissions",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_ProjectGroups_AffiliatedGroupId",
                table: "Submissions",
                column: "AffiliatedGroupId",
                principalTable: "ProjectGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UnvotedJoinRequests_JoinRequests_JoinRequestId",
                table: "UnvotedJoinRequests",
                column: "JoinRequestId",
                principalTable: "JoinRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UnvotedJoinRequests_Students_StudentId",
                table: "UnvotedJoinRequests",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Courses_CourseId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_User_PublishedUsedId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Submissions_CommentedSubmissionId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_User_CommentedUserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_PeerGradeAssignments_PeerGradeAssignmentId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupSizes_Courses_CourseId",
                table: "GroupSizes");

            migrationBuilder.DropForeignKey(
                name: "FK_InstructorCourses_Courses_CourseId",
                table: "InstructorCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_InstructorCourses_Instructors_InstructorId",
                table: "InstructorCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_JoinRequests_ProjectGroups_RequestedGroupId",
                table: "JoinRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_JoinRequests_Sections_SectionId",
                table: "JoinRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_JoinRequests_Students_RequestingStudentId",
                table: "JoinRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_MergeRequests_ProjectGroups_Id",
                table: "MergeRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_MergeRequests_ProjectGroups_SenderGroupId",
                table: "MergeRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_MergeRequests_Sections_SectionId",
                table: "MergeRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PeerGradeAssignments_User_PublishedUserId",
                table: "PeerGradeAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectGrades_Courses_CourseId",
                table: "ProjectGrades");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectGrades_ProjectGroups_GradedProjectGroupId",
                table: "ProjectGrades");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectGrades_Students_GradingStudentId",
                table: "ProjectGrades");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectGroups_Courses_AffiliatedCourseId",
                table: "ProjectGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectGroups_Sections_SectionId",
                table: "ProjectGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Courses_CourseId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourses_Courses_CourseId",
                table: "StudentCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourses_Students_StudentId",
                table: "StudentCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentJoinRequests_JoinRequests_JoinRequestId",
                table: "StudentJoinRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentJoinRequests_Students_StudentId",
                table: "StudentJoinRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentMergeRequests_MergeRequests_MergeRequestId",
                table: "StudentMergeRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentMergeRequests_Students_StudentId",
                table: "StudentMergeRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentProjectGroups_ProjectGroups_ProjectGroupId",
                table: "StudentProjectGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentProjectGroups_Students_StudentId",
                table: "StudentProjectGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Sections_SectionId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Assignments_AffiliatedAssignmentId",
                table: "Submissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Courses_CourseId",
                table: "Submissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_ProjectGroups_AffiliatedGroupId",
                table: "Submissions");

            migrationBuilder.DropForeignKey(
                name: "FK_UnvotedJoinRequests_JoinRequests_JoinRequestId",
                table: "UnvotedJoinRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_UnvotedJoinRequests_Students_StudentId",
                table: "UnvotedJoinRequests");

            migrationBuilder.DropTable(
                name: "PeerGrades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UnvotedJoinRequests",
                table: "UnvotedJoinRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Submissions",
                table: "Submissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentProjectGroups",
                table: "StudentProjectGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentMergeRequests",
                table: "StudentMergeRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentJoinRequests",
                table: "StudentJoinRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentCourses",
                table: "StudentCourses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sections",
                table: "Sections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectGroups",
                table: "ProjectGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectGrades",
                table: "ProjectGrades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PeerGradeAssignments",
                table: "PeerGradeAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MergeRequests",
                table: "MergeRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JoinRequests",
                table: "JoinRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InstructorCourses",
                table: "InstructorCourses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupSizes",
                table: "GroupSizes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assignments",
                table: "Assignments");

            migrationBuilder.RenameTable(
                name: "UnvotedJoinRequests",
                newName: "UnvotedJoinRequest");

            migrationBuilder.RenameTable(
                name: "Submissions",
                newName: "Submission");

            migrationBuilder.RenameTable(
                name: "StudentProjectGroups",
                newName: "StudentProjectGroup");

            migrationBuilder.RenameTable(
                name: "StudentMergeRequests",
                newName: "StudentMergeRequest");

            migrationBuilder.RenameTable(
                name: "StudentJoinRequests",
                newName: "StudentJoinRequest");

            migrationBuilder.RenameTable(
                name: "StudentCourses",
                newName: "StudentCourse");

            migrationBuilder.RenameTable(
                name: "Sections",
                newName: "Section");

            migrationBuilder.RenameTable(
                name: "ProjectGroups",
                newName: "ProjectGroup");

            migrationBuilder.RenameTable(
                name: "ProjectGrades",
                newName: "ProjectGrade");

            migrationBuilder.RenameTable(
                name: "PeerGradeAssignments",
                newName: "PeerGradeAssignment");

            migrationBuilder.RenameTable(
                name: "MergeRequests",
                newName: "MergeRequest");

            migrationBuilder.RenameTable(
                name: "JoinRequests",
                newName: "JoinRequest");

            migrationBuilder.RenameTable(
                name: "InstructorCourses",
                newName: "InstructorCourse");

            migrationBuilder.RenameTable(
                name: "GroupSizes",
                newName: "GroupSize");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comment");

            migrationBuilder.RenameTable(
                name: "Assignments",
                newName: "Assignment");

            migrationBuilder.RenameIndex(
                name: "IX_UnvotedJoinRequests_JoinRequestId",
                table: "UnvotedJoinRequest",
                newName: "IX_UnvotedJoinRequest_JoinRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_Submissions_CourseId",
                table: "Submission",
                newName: "IX_Submission_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Submissions_AffiliatedGroupId",
                table: "Submission",
                newName: "IX_Submission_AffiliatedGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Submissions_AffiliatedAssignmentId",
                table: "Submission",
                newName: "IX_Submission_AffiliatedAssignmentId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentProjectGroups_ProjectGroupId",
                table: "StudentProjectGroup",
                newName: "IX_StudentProjectGroup_ProjectGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentMergeRequests_MergeRequestId",
                table: "StudentMergeRequest",
                newName: "IX_StudentMergeRequest_MergeRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentJoinRequests_JoinRequestId",
                table: "StudentJoinRequest",
                newName: "IX_StudentJoinRequest_JoinRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentCourses_CourseId",
                table: "StudentCourse",
                newName: "IX_StudentCourse_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Sections_CourseId",
                table: "Section",
                newName: "IX_Section_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectGroups_SectionId",
                table: "ProjectGroup",
                newName: "IX_ProjectGroup_SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectGroups_AffiliatedCourseId",
                table: "ProjectGroup",
                newName: "IX_ProjectGroup_AffiliatedCourseId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectGrades_GradingStudentId",
                table: "ProjectGrade",
                newName: "IX_ProjectGrade_GradingStudentId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectGrades_GradedProjectGroupId",
                table: "ProjectGrade",
                newName: "IX_ProjectGrade_GradedProjectGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectGrades_CourseId",
                table: "ProjectGrade",
                newName: "IX_ProjectGrade_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_PeerGradeAssignments_PublishedUserId",
                table: "PeerGradeAssignment",
                newName: "IX_PeerGradeAssignment_PublishedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_MergeRequests_SenderGroupId",
                table: "MergeRequest",
                newName: "IX_MergeRequest_SenderGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_MergeRequests_SectionId",
                table: "MergeRequest",
                newName: "IX_MergeRequest_SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_JoinRequests_SectionId",
                table: "JoinRequest",
                newName: "IX_JoinRequest_SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_JoinRequests_RequestingStudentId",
                table: "JoinRequest",
                newName: "IX_JoinRequest_RequestingStudentId");

            migrationBuilder.RenameIndex(
                name: "IX_JoinRequests_RequestedGroupId",
                table: "JoinRequest",
                newName: "IX_JoinRequest_RequestedGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_InstructorCourses_InstructorId",
                table: "InstructorCourse",
                newName: "IX_InstructorCourse_InstructorId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupSizes_CourseId",
                table: "GroupSize",
                newName: "IX_GroupSize_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_CommentedUserId",
                table: "Comment",
                newName: "IX_Comment_CommentedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_CommentedSubmissionId",
                table: "Comment",
                newName: "IX_Comment_CommentedSubmissionId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_PublishedUsedId",
                table: "Assignment",
                newName: "IX_Assignment_PublishedUsedId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_CourseId",
                table: "Assignment",
                newName: "IX_Assignment_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UnvotedJoinRequest",
                table: "UnvotedJoinRequest",
                columns: new[] { "StudentId", "JoinRequestId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Submission",
                table: "Submission",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentProjectGroup",
                table: "StudentProjectGroup",
                columns: new[] { "StudentId", "ProjectGroupId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentMergeRequest",
                table: "StudentMergeRequest",
                columns: new[] { "StudentId", "MergeRequestId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentJoinRequest",
                table: "StudentJoinRequest",
                columns: new[] { "StudentId", "JoinRequestId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentCourse",
                table: "StudentCourse",
                columns: new[] { "StudentId", "CourseId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Section",
                table: "Section",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectGroup",
                table: "ProjectGroup",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectGrade",
                table: "ProjectGrade",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PeerGradeAssignment",
                table: "PeerGradeAssignment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MergeRequest",
                table: "MergeRequest",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JoinRequest",
                table: "JoinRequest",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InstructorCourse",
                table: "InstructorCourse",
                columns: new[] { "CourseId", "InstructorId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupSize",
                table: "GroupSize",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assignment",
                table: "Assignment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignment_Courses_CourseId",
                table: "Assignment",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignment_User_PublishedUsedId",
                table: "Assignment",
                column: "PublishedUsedId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Submission_CommentedSubmissionId",
                table: "Comment",
                column: "CommentedSubmissionId",
                principalTable: "Submission",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_User_CommentedUserId",
                table: "Comment",
                column: "CommentedUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_PeerGradeAssignment_PeerGradeAssignmentId",
                table: "Courses",
                column: "PeerGradeAssignmentId",
                principalTable: "PeerGradeAssignment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSize_Courses_CourseId",
                table: "GroupSize",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorCourse_Courses_CourseId",
                table: "InstructorCourse",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorCourse_Instructors_InstructorId",
                table: "InstructorCourse",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JoinRequest_ProjectGroup_RequestedGroupId",
                table: "JoinRequest",
                column: "RequestedGroupId",
                principalTable: "ProjectGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JoinRequest_Section_SectionId",
                table: "JoinRequest",
                column: "SectionId",
                principalTable: "Section",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JoinRequest_Students_RequestingStudentId",
                table: "JoinRequest",
                column: "RequestingStudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MergeRequest_ProjectGroup_Id",
                table: "MergeRequest",
                column: "Id",
                principalTable: "ProjectGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MergeRequest_ProjectGroup_SenderGroupId",
                table: "MergeRequest",
                column: "SenderGroupId",
                principalTable: "ProjectGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MergeRequest_Section_SectionId",
                table: "MergeRequest",
                column: "SectionId",
                principalTable: "Section",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PeerGradeAssignment_User_PublishedUserId",
                table: "PeerGradeAssignment",
                column: "PublishedUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectGrade_Courses_CourseId",
                table: "ProjectGrade",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectGrade_ProjectGroup_GradedProjectGroupId",
                table: "ProjectGrade",
                column: "GradedProjectGroupId",
                principalTable: "ProjectGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectGrade_Students_GradingStudentId",
                table: "ProjectGrade",
                column: "GradingStudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectGroup_Courses_AffiliatedCourseId",
                table: "ProjectGroup",
                column: "AffiliatedCourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectGroup_Section_SectionId",
                table: "ProjectGroup",
                column: "SectionId",
                principalTable: "Section",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Section_Courses_CourseId",
                table: "Section",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourse_Courses_CourseId",
                table: "StudentCourse",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourse_Students_StudentId",
                table: "StudentCourse",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentJoinRequest_JoinRequest_JoinRequestId",
                table: "StudentJoinRequest",
                column: "JoinRequestId",
                principalTable: "JoinRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentJoinRequest_Students_StudentId",
                table: "StudentJoinRequest",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentMergeRequest_MergeRequest_MergeRequestId",
                table: "StudentMergeRequest",
                column: "MergeRequestId",
                principalTable: "MergeRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentMergeRequest_Students_StudentId",
                table: "StudentMergeRequest",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentProjectGroup_ProjectGroup_ProjectGroupId",
                table: "StudentProjectGroup",
                column: "ProjectGroupId",
                principalTable: "ProjectGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentProjectGroup_Students_StudentId",
                table: "StudentProjectGroup",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Section_SectionId",
                table: "Students",
                column: "SectionId",
                principalTable: "Section",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Submission_Assignment_AffiliatedAssignmentId",
                table: "Submission",
                column: "AffiliatedAssignmentId",
                principalTable: "Assignment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Submission_Courses_CourseId",
                table: "Submission",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Submission_ProjectGroup_AffiliatedGroupId",
                table: "Submission",
                column: "AffiliatedGroupId",
                principalTable: "ProjectGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UnvotedJoinRequest_JoinRequest_JoinRequestId",
                table: "UnvotedJoinRequest",
                column: "JoinRequestId",
                principalTable: "JoinRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UnvotedJoinRequest_Students_StudentId",
                table: "UnvotedJoinRequest",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
