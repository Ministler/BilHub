using Microsoft.EntityFrameworkCore.Migrations;

namespace BilHub.Data.Migrations
{
    public partial class SeedTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder
                .Sql("INSERT INTO Instructors (Name) Values ('Can Alkan')");
            migrationBuilder
                .Sql("INSERT INTO Instructors (Name) Values ('Ercument Cicek')");
            migrationBuilder
                .Sql("INSERT INTO Instructors (Name) Values ('Selim Aksoy')");
            migrationBuilder
                .Sql("INSERT INTO Instructors (Name) Values ('Eray Tuzun')");

            migrationBuilder
                .Sql("INSERT INTO Courses (Name, InstructorId) Values ('CS 201', (SELECT Id FROM Instructors WHERE Name = 'Ercument Cicek'))");
            migrationBuilder
                .Sql("INSERT INTO Courses (Name, InstructorId) Values ('CS 202', (SELECT Id FROM Instructors WHERE Name = 'Selim Aksoy'))");
            migrationBuilder
                .Sql("INSERT INTO Courses (Name, InstructorId) Values ('CS 319', (SELECT Id FROM Instructors WHERE Name = 'Eray Tuzun'))");
            migrationBuilder
                .Sql("INSERT INTO Courses (Name, InstructorId) Values ('CS 478', (SELECT Id FROM Instructors WHERE Name = 'Can Alkan'))");

            migrationBuilder
                .Sql("INSERT INTO Students (Name, CourseId) Values ('Cagri', (SELECT Id FROM Courses WHERE Name = 'CS 201'))");
            migrationBuilder
                .Sql("INSERT INTO Students (Name, CourseId) Values ('Yusuf', (SELECT Id FROM Courses WHERE Name = 'CS 202'))");
            migrationBuilder
                .Sql("INSERT INTO Students (Name, CourseId) Values ('Aybala', (SELECT Id FROM Courses WHERE Name = 'CS 319'))");
            migrationBuilder
                .Sql("INSERT INTO Students (Name, CourseId) Values ('Ozco', (SELECT Id FROM Courses WHERE Name = 'CS 478'))");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder
                .Sql("DELETE FROM Courses");

            migrationBuilder
                .Sql("DELETE FROM Instructors");
        }
    }
}
