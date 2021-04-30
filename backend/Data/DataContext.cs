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
                .HasForeignKey(mr => mr.SenderGroupId);

            modelBuilder.Entity<MergeRequest>().HasOne(mr => mr.ReceiverGroup)
                .WithMany(g => g.IncomingMergeRequest)
                .HasForeignKey(mr => mr.ReceiverGroupId).OnDelete(DeleteBehavior.Restrict);

            byte[] hash, salt;
            Utility.CreatePasswordHash("31", out hash, out salt);

            modelBuilder.Entity<Assignment>().HasData(
                new Assignment
                {
                    Id = 1,
                    AfilliatedCourseId = 1,
                    AssignmentDescription = "Odev",
                    DueDate = new DateTime(2021, 5, 15, 7, 0, 0),
                    CreatedAt = DateTime.Now,
                    AcceptedTypes = "pdf,doc,docx",
                    MaxFileSizeInBytes = 1024,
                    VisibilityOfSubmission = true,
                    CanBeGradedByStudents = true,
                    IsItGraded = true,
                    Title = "Design Report iteration 1"
                }, new Assignment
                {
                    Id = 2,
                    AfilliatedCourseId = 2,
                    AssignmentDescription = "315 proje",
                    DueDate = new DateTime(2021, 5, 15, 7, 0, 0),
                    CreatedAt = DateTime.Now,
                    AcceptedTypes = "pdf,doc,docx",
                    MaxFileSizeInBytes = 1024,
                    VisibilityOfSubmission = false,
                    CanBeGradedByStudents = false,
                    IsItGraded = false,
                    Title = "Language design"
                }, new Assignment
                {
                    Id = 3,
                    AfilliatedCourseId = 1,
                    AssignmentDescription = "Odev iteration 2",
                    DueDate = new DateTime(2021, 4, 15, 7, 0, 0),
                    CreatedAt = DateTime.Now,
                    AcceptedTypes = "pdf-doc-docx",
                    MaxFileSizeInBytes = 4096,
                    VisibilityOfSubmission = true,
                    CanBeGradedByStudents = true,
                    IsItGraded = true,
                    Title = "Design Report iteration 2"
                }, new Assignment
                {
                    Id = 4,
                    AfilliatedCourseId = 1,
                    AssignmentDescription = "Odev iteration 3",
                    DueDate = new DateTime(2021, 4, 15, 7, 0, 0),
                    CreatedAt = DateTime.Now,
                    AcceptedTypes = "pdf-doc-docx",
                    MaxFileSizeInBytes = 4096,
                    VisibilityOfSubmission = true,
                    CanBeGradedByStudents = true,
                    IsItGraded = true,
                    Title = "Design Report 3"
                }
            );
            modelBuilder.Entity<Submission>().HasData(
                new Submission
                {
                    Id = 1,
                    AffiliatedAssignmentId = 1,
                    AffiliatedGroupId = 1,
                    UpdatedAt = DateTime.Now,
                    HasSubmission = false,
                    CourseId = 1,
                    SectionId = 1
                }
            );
            modelBuilder.Entity<Comment>().HasData(
                new Comment
                {
                    Id = 1,
                    CommentedUserId = 2,
                    CommentedSubmissionId = 1,
                    CommentText = "nays",
                    MaxGrade = 10.0M,
                    Grade = 9.4M,
                    CreatedAt = DateTime.Now,
                    FileAttachmentAvailability = true
                }
            );
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
                    VerificationCode = "31"
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
                    UserType = UserTypeClass.Student,
                    Email = "eray@tuzun",
                    VerificationCode = "31"
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
                    VerificationCode = "31"
                }, new User
                {
                    Id = 4,
                    Name = "Ozgur Chadoglu",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "ozgur@demir",
                    VerificationCode = "31"
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
                    Email = "yusuf@kawai",
                    VerificationCode = "31"
                }, new User
                {
                    Id = 6,
                    Name = "Aybala karakaya",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "aybala@karakaya",
                    VerificationCode = "31"
                }, new User
                {
                    Id = 7,
                    Name = "oguzhan ozcelik",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "oguzhan@ozcelik",
                    VerificationCode = "31"
                }, new User
                {
                    Id = 8,
                    Name = "HOCAM SIMDI BIZ SOYLE BI SISTEM DUSUNDUK DE",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "berke@ceran",
                    VerificationCode = "31"
                }, new User
                {
                    Id = 9,
                    Name = "Funda Tan",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "funda@tan",
                    VerificationCode = "31"
                }, new User
                {
                    Id = 10,
                    Name = "Hami Mert",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "hami@mert",
                    VerificationCode = "31"
                }, new User
                {
                    Id = 11,
                    Name = "Cagri Eren",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "cagri@eren",
                    VerificationCode = "31"
                }, new User
                {
                    Id = 12,
                    Name = "Guven Gerger",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "guven@gerger",
                    VerificationCode = "31"
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
                    VerificationCode = "31"
                }, new User
                {
                    Id = 14,
                    Name = "Fuat Schwarzengger",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "fuat@schwarzenegger",
                    VerificationCode = "31"
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
                    VerificationCode = "31"
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
                    Email = "erdem@Tuna",
                    VerificationCode = "31"
                }, new User
                {
                    Id = 17,
                    Name = "Elgun TA",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "elgun@ta",
                    VerificationCode = "31"
                }, new User
                {
                    Id = 18,
                    Name = "Irem Reis",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "irem@reis",
                    VerificationCode = "31"
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
                    VerificationCode = "31"
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
                    VerificationCode = "31"
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
                    VerificationCode = "31"
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
                    Email = "alper@karel",
                    VerificationCode = "31"
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
                    VerificationCode = "31"
                }, new User
                {
                    Id = 24,
                    Name = "Tuna Derbeder",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "tuna@derbeder",
                    VerificationCode = "31"
                }, new User
                {
                    Id = 25,
                    Name = "Abdul Razak",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "abdul@razak",
                    VerificationCode = "31"
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
                }/*, new Course
                {
                    Id = 7,
                    Name = "Cs 476",
                    CourseInformation = "Automata",
                    LockDate = DateTime.Today,
                    StartDate = new DateTime(2020, 10, 15, 7, 0, 0),
                    EndDate = new DateTime(2021, 1, 15, 7, 0, 0),
                    CourseSemester = "fall",
                    MinGroupSize = 1,
                    MaxGroupSize = 2
                }, new Course
                {
                    Id = 8,
                    Name = "Cs 491",
                    CourseInformation = "bitirme",
                    LockDate = DateTime.Today,
                    StartDate = new DateTime(2020, 10, 15, 7, 0, 0),
                    EndDate = new DateTime(2021, 1, 15, 7, 0, 0),
                    CourseSemester = "fall",
                    MinGroupSize = 3,
                    MaxGroupSize = 5
                }, new Course
                {
                    Id = 9,
                    Name = "Gra 201",
                    CourseInformation = "Grafik dersi",
                    LockDate = DateTime.Today,
                    StartDate = new DateTime(2020, 10, 15, 7, 0, 0),
                    EndDate = new DateTime(2021, 1, 15, 7, 0, 0),
                    CourseSemester = "fall",
                    MinGroupSize = 2,
                    MaxGroupSize = 4
                }, new Course
                {
                    Id = 10,
                    Name = "Ge 301",
                    CourseInformation = "emine or withdraw",
                    LockDate = DateTime.Today,
                    StartDate = new DateTime(2020, 10, 15, 7, 0, 0),
                    EndDate = new DateTime(2021, 1, 15, 7, 0, 0),
                    CourseSemester = "fall",
                    MinGroupSize = 2,
                    MaxGroupSize = 5
                }*/
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
                }/*,
                new Section
                {
                    Id = 10,
                    SectionlessState = true,
                    AffiliatedCourseId = 7,
                    SectionNo = 1
                },
                new Section
                {
                    Id = 11,
                    SectionlessState = false,
                    AffiliatedCourseId = 8,
                    SectionNo = 1
                },
                new Section
                {
                    Id = 12,
                    SectionlessState = false,
                    AffiliatedCourseId = 8,
                    SectionNo = 2
                },
                new Section
                {
                    Id = 13,
                    SectionlessState = false,
                    AffiliatedCourseId = 8,
                    SectionNo = 3
                },
                new Section
                {
                    Id = 14,
                    SectionlessState = false,
                    AffiliatedCourseId = 9,
                    SectionNo = 1
                },
                new Section
                {
                    Id = 15,
                    SectionlessState = false,
                    AffiliatedCourseId = 10,
                    SectionNo = 1
                },
                new Section
                {
                    Id = 16,
                    SectionlessState = false,
                    AffiliatedCourseId = 10,
                    SectionNo = 2
                }*/
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
                    ProjectInformation = "Abduls Class Helper",
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
                    AffiliatedSectionId = 9,
                    AffiliatedCourseId = 6,
                    ConfirmationState = true,
                    ConfirmedUserNumber = 1,
                    ProjectInformation = "Hamdinin melekleri",
                    ConfirmedGroupMembers = "5"
                }, new ProjectGroup
                {
                    Id = 7,
                    AffiliatedSectionId = 9,
                    AffiliatedCourseId = 6,
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "Butun kizlar toplandik",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 8,
                    AffiliatedSectionId = 8,
                    AffiliatedCourseId = 5,
                    ConfirmationState = true,
                    ConfirmedUserNumber = 5,
                    ProjectInformation = "Baris erasmustayken bitirme aliyoruz",
                    ConfirmedGroupMembers = "1 4 5 6 7"
                }, new ProjectGroup
                {
                    Id = 9,
                    AffiliatedSectionId = 6,
                    AffiliatedCourseId = 4,
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "Donemin en ikonikleri uwu",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 10,
                    AffiliatedSectionId = 1,
                    AffiliatedCourseId = 1,
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "rakip grup",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 11,
                    AffiliatedSectionId = 9,
                    AffiliatedCourseId = 6,
                    ConfirmationState = true,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "o sen olsan bari",
                    ConfirmedGroupMembers = "5"
                }, new ProjectGroup
                {
                    Id = 12,
                    AffiliatedSectionId = 8,
                    AffiliatedCourseId = 5,
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "She abunai hanasu cancu",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 13,
                    AffiliatedSectionId = 8,
                    AffiliatedCourseId = 5,
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "cotomate cotomate",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 14,
                    AffiliatedSectionId = 6,
                    AffiliatedCourseId = 4,
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "Duz bireyler",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 15,
                    AffiliatedSectionId = 6,
                    AffiliatedCourseId = 4,
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "Hazin gelmiyor ama gelebilirdi",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 16,
                    AffiliatedSectionId = 1,
                    AffiliatedCourseId = 1,
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "bulamadim isim",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 17,
                    AffiliatedSectionId = 1,
                    AffiliatedCourseId = 1,
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "grup ariyorum",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 18,
                    AffiliatedSectionId = 1,
                    AffiliatedCourseId = 1,
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "frontend guzel renk bul",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 19,
                    AffiliatedSectionId = 1,
                    AffiliatedCourseId = 1,
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "yusuf klavye kullanmayi ogren",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 20,
                    AffiliatedSectionId = 1,
                    AffiliatedCourseId = 1,
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "nasrettin hocanin askerleriyiz",
                    ConfirmedGroupMembers = ""
                }
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
                    ProjectGroupId = 2,
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
                }
            );

        }
    }
}