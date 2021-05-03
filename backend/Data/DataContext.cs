using System;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<JoinRequest> JoinRequests { get; set; }
        public DbSet<MergeRequest> MergeRequests { get; set; }
        public DbSet<PeerGrade> PeerGrades { get; set; }
        public DbSet<PeerGradeAssignment> PeerGradeAssignments { get; set; }
        public DbSet<ProjectGrade> ProjectGrades { get; set; }
        public DbSet<ProjectGroup> ProjectGroups { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CourseUser> CourseUsers { get; set; }
        public DbSet<ProjectGroupUser> ProjectGroupUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Assignment>()
                .HasMany(a => a.Submissions)
                .WithOne(s => s.AffiliatedAssignment)
                .HasForeignKey(s => s.AffiliatedAssignmentId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Submission>()
                .HasOne(s => s.AffiliatedGroup)
                .WithMany(a => a.Submissions)
                .HasForeignKey(s => s.AffiliatedGroupId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProjectGroup>()
                .HasOne(pg => pg.AffiliatedSection)
                .WithMany(s => s.ProjectGroups)
                .HasForeignKey(pg => pg.AffiliatedSectionId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CourseUser>().HasKey(cu => new { cu.UserId, cu.CourseId });
            modelBuilder.Entity<CourseUser>()
            .HasOne(bc => bc.Course)
                .WithMany(b => b.Instructors)
                .HasForeignKey(bc => bc.CourseId);
            modelBuilder.Entity<CourseUser>()
                .HasOne(bc => bc.User)
                .WithMany(c => c.InstructedCourses)
                .HasForeignKey(bc => bc.UserId);

            modelBuilder.Entity<ProjectGroupUser>().HasKey(cu => new { cu.UserId, cu.ProjectGroupId });
            modelBuilder.Entity<ProjectGroupUser>()
            .HasOne(bc => bc.ProjectGroup)
                .WithMany(b => b.GroupMembers)
                .HasForeignKey(bc => bc.ProjectGroupId);
            modelBuilder.Entity<ProjectGroupUser>()
                .HasOne(bc => bc.User)
                .WithMany(c => c.ProjectGroups)
                .HasForeignKey(bc => bc.UserId);

            modelBuilder.Entity<MergeRequest>().HasOne(mr => mr.SenderGroup)
                .WithMany(g => g.OutgoingMergeRequest)
                .HasForeignKey(mr => mr.SenderGroupId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MergeRequest>().HasOne(mr => mr.ReceiverGroup)
                .WithMany(g => g.IncomingMergeRequest)
                .HasForeignKey(mr => mr.ReceiverGroupId).OnDelete(DeleteBehavior.NoAction);

            byte[] hash, salt;
            Utility.CreatePasswordHash("cs319", out hash, out salt);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Cagri Durgut",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = 0,
                    Email = "cagri@durgut",
                    VerificationCode = "cs"
                },
                new User
                {
                    Id = 2,
                    Name = "Eray Tuzun",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Instructor,
                    Email = "eray@tuzun",
                    VerificationCode = "cs"
                },
                new User
                {
                    Id = 3,
                    Name = "Baris Ogun",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "baris@ogun",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = 4,
                    Name = "Ozgur Demir",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "ozgur@demir",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = 5,
                    Name = "Yusuf Uyar",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "yusuf@uyar",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = 6,
                    Name = "Aybala Karakaya",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "aybala@karakaya",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = 7,
                    Name = "Oguzhan Ozcelik",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "oguzhan@ozcelik",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = 8,
                    Name = "Enise Feyza",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "enise@feyza",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = 9,
                    Name = "Zehra Erdem",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "zehra@erdem",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = 10,
                    Name = "Akin Kutlu",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "akin@kutlu",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = 11,
                    Name = "Ahmet Hakan",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "ahmet@hakan",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = 12,
                    Name = "Guven Gergerli",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "guven@gergerli",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = 13,
                    Name = "Onur Korkmaz",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "onur@korkmaz",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = 14,
                    Name = "Fuat Aslan",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "fuat@aslan",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = 15,
                    Name = "Aynur Dayanik",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Instructor,
                    Email = "aynur@dayanik",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = 16,
                    Name = "Erdem Tuna",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "erdem@tuna",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = 17,
                    Name = "Elgun Jabrayilzade",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "elgun@jabrayilzade",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = 18,
                    Name = "Serkan Demirci",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "serkan@demirci",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = 19,
                    Name = "Fazli Can",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Instructor,
                    Email = "fazli@can",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = 20,
                    Name = "Can Alkan",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Instructor,
                    Email = "can@alkan",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = 21,
                    Name = "Ercument Cicek",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Instructor,
                    Email = "ercument@cicek",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = 22,
                    Name = "Alper Sarikan",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Instructor,
                    Email = "alper@sarikan",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = 23,
                    Name = "Altay Guvenir",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Instructor,
                    Email = "altay@guvenir",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = 24,
                    Name = "Tuna Dalbeler",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "tuna@dalbeler",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = 25,
                    Name = "Ali Emre",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "ali@emre",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = 26,
                    Name = "Ali Yilmaz",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "ali@yilmaz",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = 27,
                    Name = "Ayse Aydin",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "ayse@aydin",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = 28,
                    Name = "Can Demir",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "can@demir",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = 29,
                    Name = "Elif Kaya",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "elif@kaya",
                    VerificationCode = "cs"
                }
            );
            modelBuilder.Entity<Course>().HasData(
                new Course
                {
                    Id = 1,
                    Name = "CS 319",
                    CourseInformation = "Object Oriented Software Engineering",
                    CourseSemester = SemesterType.Fall,
                    Year = 2021,
                    LockDate = DateTime.Today,
                    StartDate = new DateTime(2021, 2, 15, 7, 0, 0),
                    MinGroupSize = 5,
                    MaxGroupSize = 6
                }, new Course
                {
                    Id = 2,
                    Name = "CS 315",
                    CourseInformation = "Programming Languages",
                    LockDate = DateTime.Today,
                    StartDate = new DateTime(2021, 2, 15, 7, 0, 0),
                    CourseSemester = SemesterType.Fall,
                    Year = 2021,
                    MinGroupSize = 2,
                    MaxGroupSize = 3
                }, new Course
                {
                    Id = 3,
                    Name = "CS 202",
                    CourseInformation = "Data Structures and Algorithms",
                    LockDate = DateTime.Today,
                    StartDate = new DateTime(2021, 2, 15, 7, 0, 0),
                    CourseSemester = SemesterType.Fall,
                    Year = 2021,
                    MinGroupSize = 1,
                    MaxGroupSize = 1
                }, new Course
                {
                    Id = 4,
                    Name = "CS 224",
                    CourseInformation = "Computer Organization and Architecture",
                    LockDate = DateTime.Today,
                    StartDate = new DateTime(2021, 2, 15, 7, 0, 0),
                    CourseSemester = SemesterType.Fall,
                    Year = 2019,
                    MinGroupSize = 2,
                    MaxGroupSize = 3
                }, new Course
                {
                    Id = 5,
                    Name = "Hist 200",
                    CourseInformation = "History",
                    LockDate = DateTime.Today,
                    StartDate = new DateTime(2021, 2, 15, 7, 0, 0),
                    CourseSemester = SemesterType.Fall,
                    Year = 2020,
                    MinGroupSize = 4,
                    MaxGroupSize = 5
                }, new Course
                {
                    Id = 6,
                    Name = "Math 101",
                    CourseInformation = "Calculus",
                    LockDate = DateTime.Today,
                    StartDate = new DateTime(2021, 2, 15, 7, 0, 0),
                    CourseSemester = SemesterType.Spring,
                    Year = 2021,
                    MinGroupSize = 1,
                    MaxGroupSize = 1
                }
            );
            modelBuilder.Entity<CourseUser>().HasData(
                new CourseUser
                {
                    UserId = 2,
                    CourseId = 1
                }, new CourseUser
                {
                    UserId = 16,
                    CourseId = 1
                }, new CourseUser
                {
                    UserId = 17,
                    CourseId = 1
                }, new CourseUser
                {
                    UserId = 18,
                    CourseId = 2
                }, new CourseUser
                {
                    UserId = 15,
                    CourseId = 3
                }, new CourseUser
                {
                    UserId = 21,
                    CourseId = 3
                }, new CourseUser
                {
                    UserId = 19,
                    CourseId = 4
                }, new CourseUser
                {
                    UserId = 22,
                    CourseId = 4
                }, new CourseUser
                {
                    UserId = 23,
                    CourseId = 2
                }, new CourseUser
                {
                    UserId = 20,
                    CourseId = 6
                }, new CourseUser
                {
                    UserId = 15,
                    CourseId = 5
                }
            );
            modelBuilder.Entity<Section>().HasData(
                new Section
                {
                    Id = 1,
                    AffiliatedCourseId = 1,
                    SectionNo = 1
                },
                new Section
                {
                    Id = 2,
                    AffiliatedCourseId = 1,
                    SectionNo = 2
                },
                new Section
                {
                    Id = 3,
                    AffiliatedCourseId = 2,
                    SectionNo = 1
                },
                new Section
                {
                    Id = 4,
                    AffiliatedCourseId = 2,
                    SectionNo = 2
                },
                new Section
                {
                    Id = 5,
                    AffiliatedCourseId = 3,
                    SectionNo = 1
                },
                new Section
                {
                    Id = 6,
                    AffiliatedCourseId = 4,
                    SectionNo = 1
                },
                new Section
                {
                    Id = 7,
                    AffiliatedCourseId = 4,
                    SectionNo = 2
                },
                new Section
                {
                    Id = 8,
                    AffiliatedCourseId = 5,
                    SectionNo = 2
                },
                new Section
                {
                    Id = 9,
                    AffiliatedCourseId = 6,
                    SectionNo = 1
                }
            );
            modelBuilder.Entity<ProjectGroup>().HasData(
                new ProjectGroup
                {
                    Id = 1,
                    AffiliatedSectionId = 1,
                    AffiliatedCourseId = 1,
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "BilHub Class Helper",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 5,
                    AffiliatedSectionId = 1,
                    AffiliatedCourseId = 1,
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "Yet Another Class Helper",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 2,
                    AffiliatedSectionId = 2,
                    AffiliatedCourseId = 1,
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "Classrom Helper",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 3,
                    AffiliatedSectionId = 3,
                    AffiliatedCourseId = 2,
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "AGA Language",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 4,
                    AffiliatedSectionId = 3,
                    AffiliatedCourseId = 2,
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "Satis Language",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 6,
                    AffiliatedSectionId = 2,
                    AffiliatedCourseId = 1,
                    ConfirmationState = true,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "SMS - A Student Management System",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 7,
                    AffiliatedSectionId = 1,
                    AffiliatedCourseId = 1,
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "Group A",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 8,
                    AffiliatedSectionId = 1,
                    AffiliatedCourseId = 1,
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "Group B",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 9,
                    AffiliatedSectionId = 1,
                    AffiliatedCourseId = 1,
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "Best Class Management System",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 10,
                    AffiliatedSectionId = 3,
                    AffiliatedCourseId = 2,
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "++C",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 11,
                    AffiliatedSectionId = 3,
                    AffiliatedCourseId = 2,
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "Payton",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 12,
                    AffiliatedSectionId = 3,
                    AffiliatedCourseId = 2,
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "Assembly-ish",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 13,
                    AffiliatedSectionId = 3,
                    AffiliatedCourseId = 2,
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "NewLanguage()",
                    ConfirmedGroupMembers = ""
                } // 14 - 9'la mergelendi
            );
            modelBuilder.Entity<ProjectGroupUser>().HasData(
                new ProjectGroupUser
                {
                    UserId = 1,
                    ProjectGroupId = 1,
                }, new ProjectGroupUser
                {
                    UserId = 3,
                    ProjectGroupId = 1,
                }, new ProjectGroupUser
                {
                    UserId = 4,
                    ProjectGroupId = 1,
                }, new ProjectGroupUser
                {
                    UserId = 5,
                    ProjectGroupId = 1,
                }, new ProjectGroupUser
                {
                    UserId = 6,
                    ProjectGroupId = 1,
                }, new ProjectGroupUser
                {
                    UserId = 7,
                    ProjectGroupId = 1,
                }, new ProjectGroupUser
                {
                    UserId = 11,
                    ProjectGroupId = 2,
                }, new ProjectGroupUser
                {
                    UserId = 12,
                    ProjectGroupId = 2,
                }, new ProjectGroupUser
                {
                    UserId = 13,
                    ProjectGroupId = 2,
                }, new ProjectGroupUser
                {
                    UserId = 24,
                    ProjectGroupId = 5,
                }, new ProjectGroupUser
                {
                    UserId = 25,
                    ProjectGroupId = 5,
                }, new ProjectGroupUser
                {
                    UserId = 1,
                    ProjectGroupId = 3,
                }, new ProjectGroupUser
                {
                    UserId = 3,
                    ProjectGroupId = 3,
                }, new ProjectGroupUser
                {
                    UserId = 4,
                    ProjectGroupId = 3,
                }, new ProjectGroupUser
                {
                    UserId = 5,
                    ProjectGroupId = 4,
                }, new ProjectGroupUser
                {
                    UserId = 6,
                    ProjectGroupId = 4,
                }, new ProjectGroupUser
                {
                    UserId = 7,
                    ProjectGroupId = 4,
                }, new ProjectGroupUser
                {
                    UserId = 9,
                    ProjectGroupId = 6,
                }, new ProjectGroupUser
                {
                    UserId = 8,
                    ProjectGroupId = 6,
                }, new ProjectGroupUser
                {
                    UserId = 14,
                    ProjectGroupId = 7,
                }, new ProjectGroupUser
                {
                    UserId = 10,
                    ProjectGroupId = 8,
                }, new ProjectGroupUser
                {
                    UserId = 26,
                    ProjectGroupId = 6,
                }, new ProjectGroupUser
                {
                    UserId = 27,
                    ProjectGroupId = 9,
                }, new ProjectGroupUser
                {
                    UserId = 28,
                    ProjectGroupId = 9,
                }, new ProjectGroupUser
                {
                    UserId = 29,
                    ProjectGroupId = 9,
                }, new ProjectGroupUser
                {
                    UserId = 26,
                    ProjectGroupId = 10,
                }, new ProjectGroupUser
                {
                    UserId = 27,
                    ProjectGroupId = 10,
                }, new ProjectGroupUser
                {
                    UserId = 28,
                    ProjectGroupId = 11,
                }, new ProjectGroupUser
                {
                    UserId = 29,
                    ProjectGroupId = 11,
                }, new ProjectGroupUser
                {
                    UserId = 9,
                    ProjectGroupId = 12,
                }, new ProjectGroupUser
                {
                    UserId = 10,
                    ProjectGroupId = 13,
                }

            );
            modelBuilder.Entity<Assignment>().HasData(
                new Assignment
                {
                    Id = 1,
                    Title = "Requirements report final",
                    AfilliatedCourseId = 1,
                    AssignmentDescription = "analysis and requirements report final",
                    DueDate = new DateTime(2021,3,31,23,59,59),
                    CreatedAt = new DateTime(2021,3,1,14,2,23),
                    AcceptedTypes = "pdf",
                    MaxFileSizeInBytes = 4096,
                    VisibilityOfSubmission = true,
                    CanBeGradedByStudents = true,
                    IsItGraded = true,
                    HasFile = false
                },
                new Assignment
                {
                    Id = 2,
                    Title = "Final report",
                    AfilliatedCourseId = 1,
                    AssignmentDescription = "explain the final version of your project",
                    DueDate = new DateTime(2021,5,5,23,59,59),
                    CreatedAt = new DateTime(2021,4,20,4,50,23),
                    AcceptedTypes = "pdf",
                    MaxFileSizeInBytes = 4096,
                    VisibilityOfSubmission = true,
                    CanBeGradedByStudents = false,
                    IsItGraded = true,
                    HasFile = false
                },
                new Assignment
                {
                    Id = 3,
                    Title = "First iteration of design report",
                    AfilliatedCourseId = 1,
                    AssignmentDescription = "This is the first iteration so it won't be graded",
                    DueDate = new DateTime(2021,3,15,23,59,59),
                    CreatedAt = new DateTime(2021,3,10,4,50,23),
                    AcceptedTypes = "pdf, txt, doc, docx",
                    MaxFileSizeInBytes = 4096,
                    VisibilityOfSubmission = true,
                    CanBeGradedByStudents = true,
                    IsItGraded = false,
                    HasFile = false
                }
            );
            modelBuilder.Entity<Submission>().HasData(
                new Submission
                {
                    Id = 1,
                    Description = "Our submission is here",
                    IsGraded = true,
                    SrsGrade = (decimal)9.6,
                    AffiliatedAssignmentId = 1,
                    AffiliatedGroupId = 1,
                    UpdatedAt = new DateTime(2021, 3, 31, 22, 54, 2),
                    FilePath = "", // check this
                    HasSubmission = true,
                    HasFile = false,
                    CourseId = 1,
                    SectionId = 1
                }, new Submission
                {
                    Id = 2,
                    Description = "",
                    IsGraded = true,
                    SrsGrade = (decimal)8,
                    AffiliatedAssignmentId = 1,
                    AffiliatedGroupId = 5,
                    UpdatedAt = new DateTime(2021, 3, 31, 23, 51, 12),
                    FilePath = "", // check this
                    HasSubmission = true,
                    HasFile = false,
                    CourseId = 1,
                    SectionId = 1
                }, new Submission
                {
                    Id = 3,
                    Description = "",
                    IsGraded = true,
                    SrsGrade = (decimal)6.5,
                    AffiliatedAssignmentId = 1,
                    AffiliatedGroupId = 2,
                    UpdatedAt = new DateTime(2021, 3, 30, 15, 44, 20),
                    FilePath = "", // check this
                    HasSubmission = true,
                    HasFile = false,
                    CourseId = 1,
                    SectionId = 2
                }, new Submission
                {
                    Id = 4,
                    Description = "",
                    IsGraded = false,
                    AffiliatedAssignmentId = 2,
                    AffiliatedGroupId = 1,
                    FilePath = "", // check this
                    HasSubmission = false,
                    HasFile = false,
                    CourseId = 1,
                    SectionId = 1
                }, new Submission
                {
                    Id = 5,
                    Description = "",
                    IsGraded = false,
                    AffiliatedAssignmentId = 2,
                    AffiliatedGroupId = 5,
                    FilePath = "", // check this
                    HasSubmission = false,
                    HasFile = false,
                    CourseId = 1,
                    SectionId = 1
                }, new Submission
                {
                    Id = 6,
                    Description = "",
                    IsGraded = false,
                    AffiliatedAssignmentId = 2,
                    AffiliatedGroupId = 2,
                    UpdatedAt = new DateTime(2021, 5, 3, 1, 44, 20),
                    FilePath = "", // check this
                    HasSubmission = true,
                    HasFile = false,
                    CourseId = 1,
                    SectionId = 2
                }, new Submission
                {
                    Id = 7,
                    Description = "",
                    IsGraded = false,
                    AffiliatedAssignmentId = 3,
                    AffiliatedGroupId = 1,
                    UpdatedAt = new DateTime(2021, 3, 13, 1, 44, 20),
                    FilePath = "", // check this
                    HasSubmission = true,
                    HasFile = false,
                    CourseId = 1,
                    SectionId = 1
                }, new Submission
                {
                    Id = 8,
                    Description = "",
                    IsGraded = false,
                    AffiliatedAssignmentId = 3,
                    AffiliatedGroupId = 5,
                    UpdatedAt = new DateTime(2021, 3, 14, 1, 44, 20),
                    FilePath = "", // check this
                    HasSubmission = true,
                    HasFile = false,
                    CourseId = 1,
                    SectionId = 1, 
                }, new Submission
                {
                    Id = 9,
                    Description = "",
                    IsGraded = false,
                    AffiliatedAssignmentId = 3,
                    AffiliatedGroupId = 2,
                    FilePath = "", // check this
                    HasSubmission = false,
                    HasFile = false,
                    CourseId = 1,
                    SectionId = 2
                }, new Submission
                {
                    Id = 10,
                    Description = "we submitted our report",
                    IsGraded = true,
                    SrsGrade = (decimal)9.8,
                    AffiliatedAssignmentId = 1,
                    AffiliatedGroupId = 6,
                    UpdatedAt = new DateTime(2021, 3, 31, 22, 54, 2),
                    FilePath = "", // check this
                    HasSubmission = true,
                    HasFile = false,
                    CourseId = 1,
                    SectionId = 2
                }, new Submission
                {
                    Id = 11,
                    Description = "",
                    IsGraded = true,
                    SrsGrade = (decimal)7.5,
                    AffiliatedAssignmentId = 1,
                    AffiliatedGroupId = 7,
                    UpdatedAt = new DateTime(2021, 3, 27, 23, 51, 12),
                    FilePath = "", // check this
                    HasSubmission = true,
                    HasFile = false,
                    CourseId = 1,
                    SectionId = 1
                }, new Submission
                {
                    Id = 12,
                    Description = "",
                    IsGraded = true,
                    SrsGrade = (decimal)10,
                    AffiliatedAssignmentId = 1,
                    AffiliatedGroupId = 8,
                    UpdatedAt = new DateTime(2021, 3, 29, 15, 44, 20),
                    FilePath = "", // check this
                    HasSubmission = true,
                    HasFile = false,
                    CourseId = 1,
                    SectionId = 1
                }, new Submission
                {
                    Id = 13,
                    Description = "",
                    IsGraded = true,
                    SrsGrade = (decimal)0,
                    AffiliatedAssignmentId = 1,
                    AffiliatedGroupId = 9,
                    FilePath = "", // check this
                    HasSubmission = false,
                    HasFile = false,
                    CourseId = 1,
                    SectionId = 1
                }, new Submission
                {
                    Id = 14,
                    Description = "",
                    IsGraded = true,
                    SrsGrade = (decimal)7.4,
                    AffiliatedAssignmentId = 2,
                    AffiliatedGroupId = 6,
                    UpdatedAt = new DateTime(2021, 5, 22, 22, 54, 2),
                    FilePath = "", // check this
                    HasSubmission = true,
                    HasFile = false,
                    CourseId = 1,
                    SectionId = 2
                }, new Submission
                {
                    Id = 15,
                    Description = "",
                    IsGraded = true,
                    SrsGrade = (decimal)9,
                    AffiliatedAssignmentId = 2,
                    AffiliatedGroupId = 7,
                    UpdatedAt = new DateTime(2021, 5, 21, 23, 51, 12),
                    FilePath = "", // check this
                    HasSubmission = true,
                    HasFile = false,
                    CourseId = 1,
                    SectionId = 1
                }, new Submission
                {
                    Id = 16,
                    Description = "",
                    IsGraded = true,
                    SrsGrade = (decimal)9.2,
                    AffiliatedAssignmentId = 2,
                    AffiliatedGroupId = 8,
                    UpdatedAt = new DateTime(2021, 5, 28, 15, 44, 20),
                    FilePath = "", // check this
                    HasSubmission = true,
                    HasFile = false,
                    CourseId = 1,
                    SectionId = 1
                }, new Submission
                {
                    Id = 17,
                    Description = "",
                    IsGraded = true,
                    SrsGrade = (decimal)7,
                    AffiliatedAssignmentId = 2,
                    AffiliatedGroupId = 9,
                    UpdatedAt = new DateTime(2021, 5, 29, 15, 44, 20),
                    FilePath = "", // check this
                    HasSubmission = true,
                    HasFile = false,
                    CourseId = 1,
                    SectionId = 1
                }, new Submission
                {
                    Id = 18,
                    Description = "",
                    IsGraded = false,
                    AffiliatedAssignmentId = 3,
                    AffiliatedGroupId = 6,
                    UpdatedAt = new DateTime(2021, 3, 14, 22, 54, 2),
                    FilePath = "", // check this
                    HasSubmission = true,
                 
                    HasFile = false,
                    CourseId = 1,
                    SectionId = 2
                }, new Submission
                {
                    Id = 19,
                    Description = "",
                    IsGraded = false,
                    AffiliatedAssignmentId = 3,
                    AffiliatedGroupId = 7,
                    UpdatedAt = new DateTime(2021, 3, 15, 23, 51, 12),
                    FilePath = "", // check this
                    HasSubmission = true,
                    HasFile = false,
                    CourseId = 1,
                    SectionId = 1
                }, new Submission
                {
                    Id = 20,
                    Description = "",
                    IsGraded = false,
                    AffiliatedAssignmentId = 3,
                    AffiliatedGroupId = 8,
                    UpdatedAt = new DateTime(2021, 3, 14, 15, 44, 20),
                    FilePath = "", // check this
                    HasSubmission = true,
                    HasFile = false,
                    CourseId = 1,
                    SectionId = 1
                }, new Submission
                {
                    Id = 21,
                    Description = "",
                    IsGraded = false,
                    AffiliatedAssignmentId = 3,
                    AffiliatedGroupId = 9,
                    UpdatedAt = new DateTime(2021, 3, 15, 15, 44, 20),
                    FilePath = "", // check this
                    HasSubmission = true,
                    HasFile = false,
                    CourseId = 1,
                    SectionId = 1
                }
            );
            modelBuilder.Entity<Comment>().HasData(
                new Comment
                {
                    Id = 1,
                    CommentedUserId = 24,
                    CommentedSubmissionId = 1,
                    CommentText = "You could make these improvements: ....",
                    MaxGrade = 10, 
                    Grade = 8,
                    CreatedAt = new DateTime( 2021, 4, 14, 12, 2, 3),
                    FileAttachmentAvailability = false,
                    FilePath = ""
                }, new Comment
                {
                    Id = 2,
                    CommentedUserId = 25,
                    CommentedSubmissionId = 1,
                    CommentText = "It's a nice report but I'm still attaching some ideas in a file",
                    MaxGrade = 10, 
                    Grade = (decimal)9.5,
                    CreatedAt = new DateTime( 2021, 4, 1, 11, 2, 3),
                    FileAttachmentAvailability = false,
                    FilePath = ""
                }, new Comment
                {
                    Id = 3,
                    CommentedUserId = 13,
                    CommentedSubmissionId = 1,
                    CommentText = "Cool",
                    MaxGrade = 10, 
                    Grade = (decimal)10,
                    CreatedAt = new DateTime( 2021, 4, 2, 11, 2, 3),
                    FileAttachmentAvailability = false,
                    FilePath = ""
                }
            );
            modelBuilder.Entity<JoinRequest>().HasData(
                new JoinRequest
                {
                    Id = 1,
                    RequestingStudentId = 14,
                    Description = "Hi, I want to join your group. I know .net",
                    RequestedGroupId = 5,
                    CreatedAt = new DateTime( 2021, 4, 29, 10, 23, 2 ),
                    AcceptedNumber = 0,
                    Accepted = false,
                    Resolved = false,
                    VotedStudents = ""
                },
                new JoinRequest
                {
                    Id = 2,
                    RequestingStudentId = 24,
                    Description = "Hey, you can check out the projects I did on my github profile",
                    RequestedGroupId = 5,
                    CreatedAt = new DateTime( 2021, 4, 29, 10, 23, 2 ),
                    AcceptedNumber = 0,
                    Accepted = false,
                    Resolved = false,
                    VotedStudents = ""
                },
                new JoinRequest
                {
                    Id = 3,
                    RequestingStudentId = 24,
                    Description = "Hey, can I join you?",
                    RequestedGroupId = 7,
                    CreatedAt = new DateTime( 2021, 4, 29, 10, 23, 2 ),
                    AcceptedNumber = 0,
                    Accepted = false,
                    Resolved = false,
                    VotedStudents = ""
                }, new JoinRequest
                {
                    Id = 4,
                    RequestingStudentId = 13,
                    Description = "Hi, I am very good at Javascript",
                    RequestedGroupId = 6,
                    CreatedAt = new DateTime( 2021, 4, 29, 10, 29, 2 ),
                    AcceptedNumber = 0,
                    Accepted = false,
                    Resolved = false,
                    VotedStudents = ""
                }, new JoinRequest
                {
                    Id = 5,
                    RequestingStudentId = 9,
                    Description = "Hi, I am looking for a team.",
                    RequestedGroupId = 10,
                    CreatedAt = new DateTime( 2021, 4, 29, 10, 30, 2 ),
                    AcceptedNumber = 1,
                    Accepted = false,
                    Resolved = false,
                    VotedStudents = "26"
                }, new JoinRequest
                {
                    Id = 6,
                    RequestingStudentId = 9,
                    Description = "",
                    RequestedGroupId = 13,
                    CreatedAt = new DateTime( 2021, 4, 29, 14, 30, 2 ),
                    AcceptedNumber = 0,
                    Accepted = false,
                    Resolved = false,
                    VotedStudents = ""
                }, new JoinRequest
                {
                    Id = 7,
                    RequestingStudentId = 7,
                    Description = "take me",
                    RequestedGroupId = 1,
                    CreatedAt = new DateTime( 2021, 4, 15, 14, 30, 2 ),
                    AcceptedNumber = 5,
                    Accepted = true,
                    Resolved = false,
                    VotedStudents = "1 3 4 5 6"
                }, new JoinRequest
                {
                    Id = 8,
                    RequestingStudentId = 25,
                    Description = "take me",
                    RequestedGroupId = 1,
                    CreatedAt = new DateTime( 2021, 4, 15, 10, 30, 2 ),
                    AcceptedNumber = 2,
                    Accepted = false,
                    Resolved = true,
                    VotedStudents = "1 3 4"
                }
            );
            modelBuilder.Entity<MergeRequest>().HasData(
                new MergeRequest
                {
                    Id = 1,
                    SenderGroupId = 6,
                    Description = "Let's merge, we are strong in design",
                    ReceiverGroupId = 9,
                    VotedStudents = "26",
                    CreatedAt = new DateTime( 2021, 4, 13, 10, 23, 2 ),
                    Accepted = false,
                    Resolved = false
                }, new MergeRequest
                {
                    Id = 2,
                    SenderGroupId = 9,
                    Description = "You can see our past projects from our pages. We'd be good team if we merged",
                    ReceiverGroupId = 2,
                    VotedStudents = "27",
                    CreatedAt = new DateTime( 2021, 4, 13, 14, 23, 2 ),
                    Accepted = false,
                    Resolved = false
                }, new MergeRequest
                {
                    Id = 3,
                    SenderGroupId = 2,
                    Description = "",
                    ReceiverGroupId = 6,
                    VotedStudents = "13",
                    CreatedAt = new DateTime( 2021, 4, 13, 16, 23, 2 ),
                    Accepted = false,
                    Resolved = false
                }, new MergeRequest
                {
                    Id = 4,
                    SenderGroupId = 12,
                    Description = "let's merge",
                    ReceiverGroupId = 11,
                    VotedStudents = "9",
                    CreatedAt = new DateTime( 2021, 4, 13, 17, 23, 2 ),
                    Accepted = false,
                    Resolved = false
                }, new MergeRequest
                {
                    Id = 5,
                    SenderGroupId = 10,
                    Description = "",
                    ReceiverGroupId = 13,
                    VotedStudents = "",
                    CreatedAt = new DateTime( 2021, 4, 13, 17, 23, 2 ),
                    Accepted = false,
                    Resolved = false
                }, new MergeRequest
                {
                    Id = 6,
                    SenderGroupId = 13,
                    Description = "",
                    ReceiverGroupId = 11,
                    VotedStudents = "10 28",
                    CreatedAt = new DateTime( 2021, 4, 13, 19, 23, 2 ),
                    Accepted = false,
                    Resolved = false
                }, new MergeRequest
                {
                    Id = 7,
                    SenderGroupId = 14,
                    Description = "",
                    ReceiverGroupId = 1,
                    VotedStudents = "1 3 4 5 6",
                    CreatedAt = new DateTime( 2021, 4, 13, 19, 23, 2 ),
                    Accepted = true,
                    Resolved = false
                }, new MergeRequest
                {
                    Id = 8,
                    SenderGroupId = 5,
                    Description = "hey, merge",
                    ReceiverGroupId = 1,
                    VotedStudents = "1 3 24 4",
                    CreatedAt = new DateTime( 2021, 4, 10, 19, 23, 2 ),
                    Accepted = false,
                    Resolved = true
                }
            );
            modelBuilder.Entity<PeerGradeAssignment>().HasData(
                new PeerGradeAssignment
                {
                    Id = 1,
                    CourseId = 1,
                    DueDate = new DateTime( 2021, 6, 1, 1, 0, 0 ),
                    LastEdited = new DateTime( 2021, 4, 1, 1, 1, 0, 0 ),
                    MaxGrade = 10
                }, new PeerGradeAssignment
                {
                    Id = 2,
                    CourseId = 2,
                    DueDate = new DateTime( 2021, 8, 1, 1, 0, 0 ),
                    LastEdited = new DateTime( 2021, 4, 1, 1, 1, 0, 0 ),
                    MaxGrade = 5
                }
            );
            modelBuilder.Entity<ProjectGrade>().HasData(
                new ProjectGrade
                {
                    Id = 1,
                    GradingUserId = 8,
                    Description = "cool one",
                    GradedProjectGroupID = 1,
                    FilePath = "",
                    HasFile = false,
                    MaxGrade = 10,
                    Grade = 10,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 0, 0 )
                }, new ProjectGrade
                {
                    Id = 2,
                    GradingUserId = 9,
                    Description = " ",
                    GradedProjectGroupID = 1,
                    FilePath = "",
                    HasFile = false,
                    MaxGrade = 10,
                    Grade = 9,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 1, 0 )
                }, new ProjectGrade
                {
                    Id = 3,
                    GradingUserId = 10,
                    Description = " ",
                    GradedProjectGroupID = 1,
                    FilePath = "",
                    HasFile = false,
                    MaxGrade = 10,
                    Grade = 8,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 2, 0 )
                }, new ProjectGrade
                {
                    Id = 4,
                    GradingUserId = 2,
                    Description = " ",
                    GradedProjectGroupID = 1,
                    FilePath = "",
                    HasFile = false,
                    MaxGrade = 10,
                    Grade = 10,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 3, 0 )
                }, new ProjectGrade
                {
                    Id = 5,
                    GradingUserId = 1,
                    Description = " ",
                    GradedProjectGroupID = 2,
                    FilePath = "",
                    HasFile = false,
                    MaxGrade = 10,
                    Grade = 6,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 4, 0 )
                }, new ProjectGrade
                {
                    Id = 6,
                    GradingUserId = 2,
                    Description = " ",
                    GradedProjectGroupID = 2,
                    FilePath = "",
                    HasFile = false,
                    MaxGrade = 10,
                    Grade = 7,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 5, 0 )
                }, new ProjectGrade
                {
                    Id = 7,
                    GradingUserId = 3,
                    Description = " ",
                    GradedProjectGroupID = 1,
                    FilePath = "",
                    HasFile = false,
                    MaxGrade = 10,
                    Grade = 8,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 6, 0 )
                }, new ProjectGrade
                {
                    Id = 8,
                    GradingUserId = 9,
                    Description = " ",
                    GradedProjectGroupID = 1,
                    FilePath = "",
                    HasFile = false,
                    MaxGrade = 10,
                    Grade = 9,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 7, 0 )
                }, new ProjectGrade
                {
                    Id = 9,
                    GradingUserId = 6,
                    Description = " ",
                    GradedProjectGroupID = 5,
                    FilePath = "",
                    HasFile = false,
                    MaxGrade = 10,
                    Grade = 9,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 8, 0 )
                }, new ProjectGrade
                {
                    Id = 10,
                    GradingUserId = 2,
                    Description = " ",
                    GradedProjectGroupID = 5,
                    FilePath = "",
                    HasFile = false,
                    MaxGrade = 10,
                    Grade = 9,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 9, 0 )
                }, new ProjectGrade
                {
                    Id = 11,
                    GradingUserId = 3,
                    Description = " ",
                    GradedProjectGroupID = 5,
                    FilePath = "",
                    HasFile = false,
                    MaxGrade = 10,
                    Grade = 8,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 10, 0 )
                }, new ProjectGrade
                {
                    Id = 12,
                    GradingUserId = 7,
                    Description = " ",
                    GradedProjectGroupID = 5,
                    FilePath = "",
                    HasFile = false,
                    MaxGrade = 10,
                    Grade = 10,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 11, 0 )
                }, new ProjectGrade
                {
                    Id = 13,
                    GradingUserId = 17,
                    Description = "",
                    GradedProjectGroupID = 5,
                    FilePath = "",
                    HasFile = false,
                    MaxGrade = 10,
                    Grade = 8,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 11, 0 )
                }, new ProjectGrade
                {
                    Id = 14,
                    GradingUserId = 16,
                    Description = "good job",
                    GradedProjectGroupID = 1,
                    FilePath = "",
                    HasFile = false,
                    MaxGrade = 10,
                    Grade = 10,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 11, 0 )
                }, new ProjectGrade
                {
                    Id = 15,
                    GradingUserId = 17,
                    Description = "amazing",
                    GradedProjectGroupID = 1,
                    FilePath = "",
                    HasFile = false,
                    MaxGrade = 10,
                    Grade = 10,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 11, 0 )
                }, new ProjectGrade
                {
                    Id = 16,
                    GradingUserId = 16,
                    Description = "",
                    GradedProjectGroupID = 5,
                    FilePath = "",
                    HasFile = false,
                    MaxGrade = 10,
                    Grade = 9,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 11, 0 )
                }
            );
            modelBuilder.Entity<PeerGrade>().HasData(
                new PeerGrade
                {
                    Id = 1,
                    ProjectGroupId = 1,
                    ReviewerId = 1,
                    RevieweeId = 3,
                    MaxGrade = 10,
                    Grade = 10,
                    Comment = "nice term",
                    PeerGradeAssignmentId = 1,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 0, 0 )
                }, new PeerGrade
                {
                    Id = 2,
                    ProjectGroupId = 1,
                    ReviewerId = 1,
                    RevieweeId = 4,
                    MaxGrade = 10,
                    Grade = 9,
                    Comment = "",
                    PeerGradeAssignmentId = 1,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 1, 0 )
                }, new PeerGrade
                {
                    Id = 3,
                    ProjectGroupId = 1,
                    ReviewerId = 1,
                    RevieweeId = 5,
                    MaxGrade = 10,
                    Grade = 8,
                    Comment = "",
                    PeerGradeAssignmentId = 1,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 2, 0 )
                }, new PeerGrade
                {
                    Id = 4,
                    ProjectGroupId = 1,
                    ReviewerId = 1,
                    RevieweeId = 6,
                    MaxGrade = 10,
                    Grade = 8,
                    Comment = "",
                    PeerGradeAssignmentId = 1,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 3, 0 )
                }, new PeerGrade
                {
                    Id = 5,
                    ProjectGroupId = 1,
                    ReviewerId = 1,
                    RevieweeId = 7,
                    MaxGrade = 10,
                    Grade = 8,
                    Comment = "",
                    PeerGradeAssignmentId = 1,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 4, 0 )
                }, new PeerGrade
                {
                    Id = 6,
                    ProjectGroupId = 1,
                    ReviewerId = 3,
                    RevieweeId = 1,
                    MaxGrade = 10,
                    Grade = 8,
                    Comment = "",
                    PeerGradeAssignmentId = 1,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 5, 0 )
                }, new PeerGrade
                {
                    Id = 7,
                    ProjectGroupId = 1,
                    ReviewerId = 3,
                    RevieweeId = 4,
                    MaxGrade = 10,
                    Grade = 6,
                    Comment = "",
                    PeerGradeAssignmentId = 1,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 6, 0 )
                }, new PeerGrade
                {
                    Id = 8,
                    ProjectGroupId = 1,
                    ReviewerId = 3,
                    RevieweeId = 5,
                    MaxGrade = 10,
                    Grade = 8,
                    Comment = "",
                    PeerGradeAssignmentId = 1,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 7, 0 )
                }, new PeerGrade
                {
                    Id = 9,
                    ProjectGroupId = 1,
                    ReviewerId = 3,
                    RevieweeId = 6,
                    MaxGrade = 10,
                    Grade = 10,
                    Comment = "",
                    PeerGradeAssignmentId = 1,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 8, 0 )
                }, new PeerGrade
                {
                    Id = 10,
                    ProjectGroupId = 1,
                    ReviewerId = 3,
                    RevieweeId = 7,
                    MaxGrade = 10,
                    Grade = 10,
                    Comment = "",
                    PeerGradeAssignmentId = 1,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 9, 0 )
                }, new PeerGrade
                {
                    Id = 11,
                    ProjectGroupId = 1,
                    ReviewerId = 4,
                    RevieweeId = 1,
                    MaxGrade = 10,
                    Grade = 10,
                    Comment = "",
                    PeerGradeAssignmentId = 1,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 10, 0 )
                }, new PeerGrade
                {
                    Id = 12,
                    ProjectGroupId = 1,
                    ReviewerId = 4,
                    RevieweeId = 3,
                    MaxGrade = 10,
                    Grade = 4,
                    Comment = "",
                    PeerGradeAssignmentId = 1,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 11, 0 )
                }, new PeerGrade
                {
                    Id = 13,
                    ProjectGroupId = 1,
                    ReviewerId = 4,
                    RevieweeId = 5,
                    MaxGrade = 10,
                    Grade = 8,
                    Comment = "",
                    PeerGradeAssignmentId = 1,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 12, 0 )
                }, new PeerGrade
                {
                    Id = 14,
                    ProjectGroupId = 1,
                    ReviewerId = 4,
                    RevieweeId = 6,
                    MaxGrade = 10,
                    Grade = 7,
                    Comment = "",
                    PeerGradeAssignmentId = 1,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 13, 0 )
                }, new PeerGrade
                {
                    Id = 15,
                    ProjectGroupId = 1,
                    ReviewerId = 4,
                    RevieweeId = 7,
                    MaxGrade = 10,
                    Grade = 9,
                    Comment = "",
                    PeerGradeAssignmentId = 1,
                    LastEdited = new DateTime( 2021, 4, 1, 2, 1, 14, 0 )
                }
            );
        }
    }
}