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

            var crs10221 = 7;
            var as10221 = 4;
            var sec10221 = 10;
            var idseak = 30;
            var gr10221 = 14;
            var sub10221 = 22;

            var gr319 = 35;
            var sub319 = 40;

            var crs315 = 10;
            var us315 = 65;
            var sec315 = 15;
            var as315 = 10;
            var sub315 = 80;

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
                }, new User
                {
                    Id = idseak,
                    Name = "Selim Aksoy",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Instructor,
                    Email = "selim@aksoy",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = idseak + 1,
                    Name = "Hasan Kaya",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "selim@aksoy",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = idseak + 2,
                    Name = "Ayse Kaya",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "ayse@kaya",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = idseak + 3,
                    Name = "Demir Kaya",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "demir@kaya",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = idseak + 4,
                    Name = "Tuna Dagli",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "tuna@dagli",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = idseak + 5,
                    Name = "Ahmet Mumtaz",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "ahmet@mumtaz",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = idseak + 6,
                    Name = "Hakan Sivik",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,

                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "hakan@sivik",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = idseak + 7,
                    Name = "Semih Karay",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,

                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "semih@karay",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = idseak + 8,
                    Name = "Yagmur Topcu",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,

                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "yagmur@topcu",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = idseak + 9,
                    Name = "Engin Uygur",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,

                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "engin@uygur",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = idseak + 10,
                    Name = "Ece Zengin",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,

                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "ece@zengin",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = idseak + 11,
                    Name = "Erim Eraydin",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,

                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "erim@eraydin",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = idseak + 12,
                    Name = "Yasar Dinc",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,

                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "yasar@dinc",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = idseak + 13,
                    Name = "Devrim Toker",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,

                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "devrim@toker",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = idseak + 14,
                    Name = "Onur Gollu",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,

                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "onur@gollu",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = idseak + 15,
                    Name = "Banu Ceren",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,

                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "banu@ceren",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = idseak + 16,
                    Name = "Ela Nazif",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,

                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "ela@nazif",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = idseak + 17,
                    Name = "Doga Genc",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,

                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "doga@genc",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = idseak + 18,
                    Name = "Iskender Koc",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,

                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "iskender@koc",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = idseak + 19,
                    Name = "Bulut Kucuk",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,

                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "bulut@kucuk",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = idseak + 20,
                    Name = "Nehir Aksoy",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,

                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "nehir@aksoy",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = idseak + 21,
                    Name = "Ilkin Aslan",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,

                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "ilkin@aslan",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = idseak + 22,
                    Name = "Kadir Demirci",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,

                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "kadir@demirci",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = idseak + 23,
                    Name = "Ipek Koc",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,

                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "ipek@koc",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = idseak + 24,
                    Name = "Murat Aksoy",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,

                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "murat@aksoy",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = idseak + 25,
                    Name = "Muge Uzun",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,

                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "muge@uzun",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = us315,
                    Name = "Irmak Turkoz",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "irmak@turkoz",
                    VerificationCode = "cs"
                }, new User
                {
                    Id = us315 + 1,
                    Name = "Alper Sahistan",
                    PasswordHash = hash,
                    SecondPasswordHash = hash,
                    PasswordSalt = salt,
                    VerifiedStatus = true,
                    DarkModeStatus = false,
                    UserType = UserTypeClass.Student,
                    Email = "alper@sahistan",
                    VerificationCode = "cs"
                }
            );
            modelBuilder.Entity<Course>().HasData(
                new Course
                {
                    Id = 1,
                    Name = "CS 319",
                    CourseInformation = "Object Oriented Software Engineering",
                    CourseDescription = "Object Oriented Software Engineering",
                    CourseSemester = SemesterType.Fall,
                    NumberOfSections = 2,
                    Year = 2021,
                    LockDate = DateTime.Today,
                    StartDate = new DateTime(2021, 2, 15, 7, 0, 0),
                    MinGroupSize = 3,
                    MaxGroupSize = 5,
                    IsActive = true,
                }
                /*
                , new Course
                {
                    Id = 2,
                    Name = "CS 315",
                    CourseInformation = "Programming Languages",
                    CourseDescription = "Programming Languages",
                    LockDate = DateTime.Today,
                    StartDate = new DateTime(2021, 2, 15, 7, 0, 0),
                    CourseSemester = SemesterType.Spring,
                    NumberOfSections = 2,
                    Year = 2021,
                    IsActive = true,
                    MinGroupSize = 2,
                    MaxGroupSize = 3
                }
                */
                , new Course
                {
                    Id = 3,
                    Name = "CS 202",
                    CourseInformation = "Data Structures and Algorithms",
                    CourseDescription = "Data Structures and Algorithms",
                    LockDate = DateTime.Today,
                    StartDate = new DateTime(2021, 2, 15, 7, 0, 0),
                    CourseSemester = SemesterType.Fall,
                    Year = 2021,
                    MinGroupSize = 1,
                    IsActive = true,
                    MaxGroupSize = 1
                }, new Course
                {
                    Id = 4,
                    Name = "CS 224",
                    CourseInformation = "Computer Organization and Architecture",
                    CourseDescription = "Computer Organization and Architecture",
                    LockDate = DateTime.Today,
                    StartDate = new DateTime(2021, 2, 15, 7, 0, 0),
                    CourseSemester = SemesterType.Fall,
                    Year = 2019,
                    MinGroupSize = 2,
                    IsActive = true,
                    MaxGroupSize = 3
                }, new Course
                {
                    Id = 5,
                    Name = "Hist 200",
                    CourseInformation = "History",
                    LockDate = DateTime.Today,
                    StartDate = new DateTime(2021, 2, 15, 7, 0, 0),
                    CourseDescription = "Object Oriented Software Engineering",
                    CourseSemester = SemesterType.Fall,
                    Year = 2020,
                    IsActive = true,
                    MinGroupSize = 4,
                    MaxGroupSize = 5
                }, new Course
                {
                    Id = 6,
                    Name = "Math 101",
                    CourseInformation = "Calculus",
                    CourseDescription = "Basic Calculus",
                    LockDate = DateTime.Today,
                    StartDate = new DateTime(2021, 2, 15, 7, 0, 0),
                    CourseSemester = SemesterType.Spring,
                    Year = 2021,
                    IsActive = true,
                    MinGroupSize = 1,
                    MaxGroupSize = 1
                }, new Course
                {
                    Id = crs10221,
                    Name = "CS102",
                    CourseDescription = "CS102 gives you an opportunity to put the basic computer literacy, design and programming skills you learnt in CS101 into practice. The course has two components. The first is simply a continuation of CS101 aimed at expanding the range of techniques you have available to solve problems. These new techniques will be presented in formal lectures and, as in C"
                                            + "S101, you will be given lab. assignments designed to let you practice them. Material in this section includes recursion, files and some basic data structures, plus a little about object-oriented programming, event-driven architectures, searching and sorting. ",
                    StartDate = new DateTime(2021, 1, 27, 6, 0, 0),
                    CourseSemester = SemesterType.Spring,
                    CourseInformation = "Algorithms and Programming",
                    LockDate = DateTime.Today,//bunlar mi   
                    IsActive = true,
                    Year = 2021,
                    MinGroupSize = 1,
                    MaxGroupSize = 3,
                    NumberOfSections = 2
                }, new Course
                {
                    Id = crs10221 + 1,
                    Name = "CS102",
                    CourseInformation = "Algorithms and Programming",
                    CourseDescription = "Algorithms and Programming",
                    LockDate = new DateTime(2020, 1, 17),
                    StartDate = new DateTime(2019, 9, 27),
                    CourseSemester = SemesterType.Spring,
                    IsActive = false,
                    NumberOfSections = 1,
                    Year = 2021,
                    MinGroupSize = 4,
                    MaxGroupSize = 6
                }, new Course
                {
                    Id = crs315,
                    Name = "CS315",
                    CourseInformation = "Programming Languages",
                    CourseDescription = "Language evaluation criteria. Describing syntax and semantics and more...",
                    LockDate = DateTime.Today,
                    StartDate = new DateTime(2020, 1, 27, 1, 1, 7),
                    CourseSemester = SemesterType.Spring,
                    IsActive = true,
                    NumberOfSections = 1,
                    Year = 2021,
                    MinGroupSize = 2,
                    MaxGroupSize = 3
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
                }
                /*
                , new CourseUser
                {
                    UserId = 18,
                    CourseId = 2
                }
                */
                , new CourseUser
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
                },
                 /*new CourseUser
                 {
                     UserId = 23,
                     CourseId = 2
                 },
                 */
                 new CourseUser
                 {
                     UserId = 20,
                     CourseId = 6
                 }, new CourseUser
                 {
                     UserId = 15,
                     CourseId = 5
                 }, new CourseUser
                 {
                     UserId = 15,
                     CourseId = crs10221
                 }, new CourseUser
                 {
                     UserId = 5,
                     CourseId = crs10221
                 }, new CourseUser
                 {
                     UserId = 6,
                     CourseId = crs10221
                 }, new CourseUser
                 {
                     UserId = idseak,
                     CourseId = crs10221
                 }, new CourseUser
                 {
                     UserId = 23,
                     CourseId = crs315
                 }, new CourseUser
                 {
                     UserId = us315,
                     CourseId = crs315
                 }, new CourseUser
                 {
                     UserId = us315 + 1,
                     CourseId = crs315
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
                /*
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
                */
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
                },
                new Section
                {
                    Id = sec10221,
                    AffiliatedCourseId = crs10221,
                    SectionNo = 1
                },
                new Section
                {
                    Id = sec10221 + 1,
                    AffiliatedCourseId = crs10221,
                    SectionNo = 2
                },
                new Section
                {
                    Id = sec315,
                    AffiliatedCourseId = crs315,
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
                    Name = "BilHub",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 5,
                    AffiliatedSectionId = 1,
                    AffiliatedCourseId = 1,
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "Yet Another Class Helper",
                    Name = "YACH",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 2,
                    AffiliatedSectionId = 2,
                    AffiliatedCourseId = 1,
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "Classrom Helper",
                    Name = "ClassHelp",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 3,
                    AffiliatedSectionId = sec315,
                    AffiliatedCourseId = crs315,
                    ConfirmationState = true,
                    Name = "AGA inc",
                    ConfirmedUserNumber = 3,
                    ProjectInformation = "AGA Language",
                    ConfirmedGroupMembers = "1 3 4"
                }, new ProjectGroup
                {
                    Id = 4,
                    AffiliatedSectionId = sec315,
                    AffiliatedCourseId = crs315,
                    ConfirmationState = true,
                    Name = "STS lng",
                    ConfirmedUserNumber = 3,
                    ProjectInformation = "Satis Language",
                    ConfirmedGroupMembers = "5 6 7"
                }, new ProjectGroup
                {
                    Id = 6,
                    AffiliatedSectionId = 2,
                    Name = "SMS company",
                    AffiliatedCourseId = 1,
                    ConfirmationState = false,
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
                    Name = "ProCheck",
                    ProjectInformation = "procheck",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 8,
                    AffiliatedSectionId = 1,
                    AffiliatedCourseId = 1,
                    ConfirmationState = false,
                    Name = "Runtime Errors",
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "CS319 group",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 9,
                    AffiliatedSectionId = 1,
                    AffiliatedCourseId = 1,
                    Name = "BCMS",
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "Best Class Management System",
                    ConfirmedGroupMembers = ""
                },
                new ProjectGroup
                {
                    Id = 10,
                    AffiliatedSectionId = sec315,
                    AffiliatedCourseId = crs315,
                    Name = "C language",
                    ConfirmationState = true,
                    ConfirmedUserNumber = 2,
                    ProjectInformation = "Making C Better",
                    ConfirmedGroupMembers = "26 27"
                },
                new ProjectGroup
                {
                    Id = 11,
                    AffiliatedSectionId = sec315,
                    AffiliatedCourseId = crs315,
                    ConfirmationState = true,
                    ConfirmedUserNumber = 2,
                    Name = "Pyhton lovers",
                    ProjectInformation = "PYhLov",
                    ConfirmedGroupMembers = "28 29"
                },
                /*new ProjectGroup

                {
                    Id = 12,
                    AffiliatedSectionId = 3,
                    AffiliatedCourseId = 2,
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    Name = "Group languge",
                    ProjectInformation = "Assembly-ish",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = 13,
                    AffiliatedSectionId = 3,
                    AffiliatedCourseId = 2,
                    Name = "new lang",
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "NewLanguage()",
                    ConfirmedGroupMembers = ""
                }, 
                */
                new ProjectGroup
                {
                    Id = gr10221,
                    AffiliatedSectionId = sec10221,
                    AffiliatedCourseId = crs10221,
                    Name = "Course Helper",
                    ConfirmationState = true,
                    ConfirmedUserNumber = 3,
                    ProjectInformation = "Designing a course helper",
                    ConfirmedGroupMembers = "31 32 33"
                }, new ProjectGroup
                {
                    Id = gr10221 + 1,
                    AffiliatedSectionId = sec10221,
                    AffiliatedCourseId = crs10221,
                    Name = "GPS Utilizer",
                    ConfirmationState = true,
                    ConfirmedUserNumber = 3,
                    ProjectInformation = "GPS's are fun",
                    ConfirmedGroupMembers = "3 4 7"
                }, new ProjectGroup
                {
                    Id = gr10221 + 2,
                    AffiliatedSectionId = sec10221,
                    AffiliatedCourseId = crs10221,
                    Name = "UNSCO",
                    ConfirmationState = true,
                    ConfirmedUserNumber = 3,
                    ProjectInformation = "UNESCO",
                    ConfirmedGroupMembers = "34 35 36"
                }, new ProjectGroup
                {
                    Id = gr10221 + 3,
                    AffiliatedSectionId = sec10221,
                    AffiliatedCourseId = crs10221,
                    Name = "Group1",
                    ConfirmationState = true,
                    ConfirmedUserNumber = 2,
                    ProjectInformation = "g1",
                    ConfirmedGroupMembers = "37 38"
                }, new ProjectGroup
                {
                    Id = gr10221 + 4,
                    AffiliatedSectionId = sec10221,
                    AffiliatedCourseId = crs10221,
                    Name = "Group2",
                    ConfirmationState = true,
                    ConfirmedUserNumber = 2,
                    ProjectInformation = "g2",
                    ConfirmedGroupMembers = "39 40"
                }, new ProjectGroup
                {
                    Id = gr10221 + 5,
                    AffiliatedSectionId = sec10221,
                    AffiliatedCourseId = crs10221,
                    Name = "Group3",
                    ConfirmationState = true,
                    ConfirmedUserNumber = 2,
                    ProjectInformation = "g3",
                    ConfirmedGroupMembers = "41 42"
                }, new ProjectGroup
                {
                    Id = gr10221 + 6,
                    AffiliatedSectionId = sec10221,
                    AffiliatedCourseId = crs10221,
                    Name = "Group4",
                    ConfirmationState = true,
                    ConfirmedUserNumber = 1,
                    ProjectInformation = "g4",
                    ConfirmedGroupMembers = "43"
                }, new ProjectGroup
                {
                    Id = gr10221 + 7,
                    AffiliatedSectionId = sec10221,
                    AffiliatedCourseId = crs10221,
                    Name = "Group5",
                    ConfirmationState = true,
                    ConfirmedUserNumber = 1,
                    ProjectInformation = "g5",
                    ConfirmedGroupMembers = "44"
                }, new ProjectGroup
                {
                    Id = gr10221 + 8,
                    AffiliatedSectionId = sec10221,
                    AffiliatedCourseId = crs10221,
                    Name = "Group6",
                    ConfirmationState = true,
                    ConfirmedUserNumber = 1,
                    ProjectInformation = "g6",
                    ConfirmedGroupMembers = "45"
                }, new ProjectGroup
                {
                    Id = gr10221 + 9,
                    AffiliatedSectionId = sec10221,
                    AffiliatedCourseId = crs10221,
                    Name = "Group7",
                    ConfirmationState = true,
                    ConfirmedUserNumber = 1,
                    ProjectInformation = "g7",
                    ConfirmedGroupMembers = "46"
                }, new ProjectGroup
                {
                    Id = gr10221 + 10,
                    AffiliatedSectionId = sec10221,
                    AffiliatedCourseId = crs10221,
                    Name = "Group8",
                    ConfirmationState = true,
                    ConfirmedUserNumber = 1,
                    ProjectInformation = "g8",
                    ConfirmedGroupMembers = "47"
                }, new ProjectGroup
                {
                    Id = gr10221 + 11,
                    AffiliatedSectionId = sec10221,
                    AffiliatedCourseId = crs10221,
                    Name = "Group9",
                    ConfirmationState = true,
                    ConfirmedUserNumber = 1,
                    ProjectInformation = "g9",
                    ConfirmedGroupMembers = "48"
                }, new ProjectGroup
                {
                    Id = gr10221 + 12,
                    AffiliatedSectionId = sec10221 + 1,
                    AffiliatedCourseId = crs10221,
                    Name = "Weather Application",
                    ConfirmationState = true,
                    ConfirmedUserNumber = 3,
                    ProjectInformation = "we are implementing a weather mobile app",
                    ConfirmedGroupMembers = "49 50 51"
                }, new ProjectGroup
                {
                    Id = gr10221 + 13,
                    AffiliatedSectionId = sec10221 + 1,
                    AffiliatedCourseId = crs10221,
                    Name = "Twitter Bot",
                    ConfirmationState = true,
                    ConfirmedUserNumber = 3,
                    ProjectInformation = "Twitter Bot to do cool things",
                    ConfirmedGroupMembers = "52 53 54"
                }, new ProjectGroup
                {
                    Id = gr319,
                    AffiliatedSectionId = 1,
                    AffiliatedCourseId = 1,
                    Name = "Enhanced Helper",
                    ConfirmationState = false,
                    ConfirmedUserNumber = 0,
                    ProjectInformation = "We will implement a classroom helper",
                    ConfirmedGroupMembers = ""
                }, new ProjectGroup
                {
                    Id = gr319 + 1,
                    AffiliatedSectionId = 1,
                    AffiliatedCourseId = 1,
                    Name = "Impressive Helper",
                    ConfirmationState = false,
                    ConfirmedUserNumber = 2,
                    ProjectInformation = "The best project ever",
                    ConfirmedGroupMembers = "6 7"
                }
            );
            modelBuilder.Entity<ProjectGroupUser>().HasData(
                new ProjectGroupUser
                {
                    UserId = 5,
                    ProjectGroupId = 1, // 
                }, new ProjectGroupUser
                {
                    UserId = 3,
                    ProjectGroupId = 1, //
                }, new ProjectGroupUser
                {
                    UserId = 4,
                    ProjectGroupId = gr319, //
                }, new ProjectGroupUser
                {
                    UserId = 1,
                    ProjectGroupId = gr319, // bu 4'u yusufta ayni sectionda gorunuyo ama degiller seed ayni yusufa gitmiyo
                }, new ProjectGroupUser
                {
                    UserId = 6,
                    ProjectGroupId = gr319 + 1, //
                }, new ProjectGroupUser
                {
                    UserId = 7,
                    ProjectGroupId = gr319 + 1,
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
                },
                /*new ProjectGroupUser
                {
                    UserId = 9,
                    ProjectGroupId = 12,
                },
                */
                new ProjectGroupUser
                {
                    UserId = idseak + 1,
                    ProjectGroupId = gr10221,
                }, new ProjectGroupUser
                {
                    UserId = idseak + 2,
                    ProjectGroupId = gr10221,
                }, new ProjectGroupUser
                {
                    UserId = idseak + 3,
                    ProjectGroupId = gr10221,
                }, new ProjectGroupUser
                {
                    UserId = idseak + 4,
                    ProjectGroupId = gr10221 + 2,
                }, new ProjectGroupUser
                {
                    UserId = idseak + 5,
                    ProjectGroupId = gr10221 + 2,
                }, new ProjectGroupUser
                {
                    UserId = idseak + 6,
                    ProjectGroupId = gr10221 + 2,
                }, new ProjectGroupUser
                {
                    UserId = 3,
                    ProjectGroupId = gr10221 + 1,
                }, new ProjectGroupUser
                {
                    UserId = 4,
                    ProjectGroupId = gr10221 + 1,
                }, new ProjectGroupUser
                {
                    UserId = 7,
                    ProjectGroupId = gr10221 + 1,
                }, new ProjectGroupUser
                {
                    UserId = idseak + 7,
                    ProjectGroupId = gr10221 + 3,
                }, new ProjectGroupUser
                {
                    UserId = idseak + 8,
                    ProjectGroupId = gr10221 + 3,
                }, new ProjectGroupUser
                {
                    UserId = idseak + 9,
                    ProjectGroupId = gr10221 + 4,
                }, new ProjectGroupUser
                {
                    UserId = idseak + 10,
                    ProjectGroupId = gr10221 + 4,
                }, new ProjectGroupUser
                {
                    UserId = idseak + 11,
                    ProjectGroupId = gr10221 + 5,
                }, new ProjectGroupUser
                {
                    UserId = idseak + 12,
                    ProjectGroupId = gr10221 + 5,
                }, new ProjectGroupUser
                {
                    UserId = idseak + 13,
                    ProjectGroupId = gr10221 + 6,
                }, new ProjectGroupUser
                {
                    UserId = idseak + 14,
                    ProjectGroupId = gr10221 + 7,
                }, new ProjectGroupUser
                {
                    UserId = idseak + 15,
                    ProjectGroupId = gr10221 + 8,
                }, new ProjectGroupUser
                {
                    UserId = idseak + 16,
                    ProjectGroupId = gr10221 + 9,
                }, new ProjectGroupUser
                {
                    UserId = idseak + 17,
                    ProjectGroupId = gr10221 + 10,
                }, new ProjectGroupUser
                {
                    UserId = idseak + 18,
                    ProjectGroupId = gr10221 + 11,
                }, new ProjectGroupUser
                {
                    UserId = idseak + 19,
                    ProjectGroupId = gr10221 + 12,
                }, new ProjectGroupUser
                {
                    UserId = idseak + 20,
                    ProjectGroupId = gr10221 + 12,
                }, new ProjectGroupUser
                {
                    UserId = idseak + 21,
                    ProjectGroupId = gr10221 + 12,
                }, new ProjectGroupUser
                {
                    UserId = idseak + 22,
                    ProjectGroupId = gr10221 + 13,
                }, new ProjectGroupUser
                {
                    UserId = idseak + 23,
                    ProjectGroupId = gr10221 + 13,
                }, new ProjectGroupUser
                {
                    UserId = idseak + 24,
                    ProjectGroupId = gr10221 + 13,
                }
            );
            modelBuilder.Entity<Assignment>().HasData(
                /*
                new Assignment
                {
                    Id = 1,
                    Title = "Requirements report final",
                    AfilliatedCourseId = 1,
                    AssignmentDescription = "analysis and requirements report final",
                    DueDate = new DateTime(2021, 3, 31, 23, 59, 59),
                    CreatedAt = new DateTime(2021, 3, 1, 14, 2, 23),
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
                    DueDate = new DateTime(2021, 5, 5, 23, 59, 59),
                    CreatedAt = new DateTime(2021, 4, 20, 4, 50, 23),
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
                    DueDate = new DateTime(2021, 3, 15, 23, 59, 59),
                    CreatedAt = new DateTime(2021, 3, 10, 4, 50, 23),
                    AcceptedTypes = "pdf, txt, doc, docx",
                    MaxFileSizeInBytes = 4096,
                    VisibilityOfSubmission = true,
                    CanBeGradedByStudents = true,
                    IsItGraded = false,
                    HasFile = false
                },
                */
                new Assignment
                {
                    Id = as10221,
                    Title = "Analysis report",
                    AfilliatedCourseId = crs10221,
                    AssignmentDescription = "",
                    DueDate = new DateTime(2021, 6, 15, 23, 59, 59),
                    CreatedAt = new DateTime(2021, 3, 10, 4, 50, 23),
                    AcceptedTypes = "pdf, txt, doc, docx",
                    MaxFileSizeInBytes = 4096,
                    VisibilityOfSubmission = true,
                    CanBeGradedByStudents = true,
                    IsItGraded = true,
                    HasFile = false
                },
                new Assignment
                {
                    Id = as315,
                    Title = "Project 1",
                    AfilliatedCourseId = crs315,
                    AssignmentDescription = "A Programming Language for Creating Adventure Games and its Lexical Analyzer",
                    DueDate = new DateTime(2021, 5, 20, 23, 59, 59),
                    CreatedAt = new DateTime(2021, 5, 1, 14, 2, 23),
                    AcceptedTypes = "pdf",
                    MaxFileSizeInBytes = 4096,
                    VisibilityOfSubmission = true,
                    CanBeGradedByStudents = true,
                    IsItGraded = false,
                    HasFile = false
                },
                new Assignment
                {
                    Id = as315 + 1,
                    Title = "Homework 2",
                    AfilliatedCourseId = crs315,
                    AssignmentDescription = "Data Structures in C, Go, Javascript",
                    DueDate = new DateTime(2021, 5, 2, 23, 59, 59),
                    CreatedAt = new DateTime(2021, 4, 10, 14, 2, 23),
                    AcceptedTypes = "pdf",
                    MaxFileSizeInBytes = 4096,
                    VisibilityOfSubmission = true,
                    CanBeGradedByStudents = true,
                    IsItGraded = true,
                    HasFile = false
                }
            );
            modelBuilder.Entity<Submission>().HasData(
                /*
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
                    Id = sub319,
                    Description = "We have submitted",
                    IsGraded = true,
                    SrsGrade = (decimal)8.2,
                    AffiliatedAssignmentId = 1,
                    AffiliatedGroupId = gr319,
                    UpdatedAt = new DateTime(2021, 3, 31, 22, 54, 2),
                    FilePath = "", // check this
                    HasSubmission = true,
                    HasFile = false,
                    CourseId = 1,
                    SectionId = 1
                }, new Submission
                {
                    Id = sub319 + 1,
                    Description = "report is here",
                    IsGraded = true,
                    SrsGrade = (decimal)9.1,
                    AffiliatedAssignmentId = 1,
                    AffiliatedGroupId = gr319 + 1,
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
                    HasSubmission = false,
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
                    Id = sub319 + 2,
                    Description = "",
                    IsGraded = false,
                    AffiliatedAssignmentId = 2,
                    AffiliatedGroupId = gr319,
                    FilePath = "", // check this
                    HasSubmission = false,
                    HasFile = false,
                    CourseId = 1,
                    SectionId = 1
                }, new Submission
                {
                    Id = sub319 + 3,
                    Description = "",
                    IsGraded = false,
                    AffiliatedAssignmentId = 2,
                    AffiliatedGroupId = gr319 + 1,
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
                    HasSubmission = false,
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
                    HasSubmission = false,
                    HasFile = false,
                    CourseId = 1,
                    SectionId = 1
                }, new Submission
                {
                    Id = sub319 + 4,
                    Description = "",
                    IsGraded = false,
                    AffiliatedAssignmentId = 3,
                    AffiliatedGroupId = gr319,
                    UpdatedAt = new DateTime(2021, 3, 13, 1, 44, 20),
                    FilePath = "", // check this
                    HasSubmission = false,
                    HasFile = false,
                    CourseId = 1,
                    SectionId = 1
                }, new Submission
                {
                    Id = sub319 + 5,
                    Description = "",
                    IsGraded = false,
                    AffiliatedAssignmentId = 3,
                    AffiliatedGroupId = gr319 + 1,
                    UpdatedAt = new DateTime(2021, 3, 13, 1, 44, 20),
                    FilePath = "", // check this
                    HasSubmission = false,
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
                    HasSubmission = false,
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
                    HasSubmission = false,
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
                },
                */
                new Submission
                {
                    Id = sub10221,
                    Description = "hey, this is our submission",
                    IsGraded = false,
                    AffiliatedAssignmentId = as10221,
                    AffiliatedGroupId = gr10221,
                    UpdatedAt = new DateTime(2021, 5, 15, 15, 44, 20),
                    FilePath = "",
                    HasSubmission = true,
                    HasFile = false,
                    CourseId = crs10221,
                    SectionId = sec10221
                }, new Submission
                {
                    Id = sub10221 + 1,
                    Description = "",
                    IsGraded = false,
                    AffiliatedAssignmentId = as10221,
                    AffiliatedGroupId = gr10221 + 1,
                    FilePath = "",
                    HasSubmission = false,
                    HasFile = false,
                    CourseId = crs10221,
                    SectionId = sec10221
                }, new Submission
                {
                    Id = sub10221 + 2,
                    Description = "please grade good",
                    IsGraded = false,
                    AffiliatedAssignmentId = as10221,
                    AffiliatedGroupId = gr10221 + 2,
                    UpdatedAt = new DateTime(2021, 4, 9, 15, 44, 20),
                    FilePath = "",
                    HasSubmission = true,
                    HasFile = false,
                    CourseId = crs10221,
                    SectionId = sec10221
                }, new Submission
                {
                    Id = sub10221 + 3,
                    Description = "analysis report is done",
                    IsGraded = false,
                    AffiliatedAssignmentId = as10221,
                    AffiliatedGroupId = gr10221 + 3,
                    UpdatedAt = new DateTime(2021, 4, 9, 15, 44, 20),
                    FilePath = "",
                    HasSubmission = true,
                    HasFile = false,
                    CourseId = crs10221,
                    SectionId = sec10221
                }, new Submission
                {
                    Id = sub10221 + 4,
                    Description = "",
                    IsGraded = false,
                    AffiliatedAssignmentId = as10221,
                    AffiliatedGroupId = gr10221 + 4,
                    FilePath = "",
                    HasSubmission = false,
                    HasFile = false,
                    CourseId = crs10221,
                    SectionId = sec10221
                }, new Submission
                {
                    Id = sub10221 + 5,
                    Description = "analysis report submission",
                    IsGraded = false,
                    AffiliatedAssignmentId = as10221,
                    AffiliatedGroupId = gr10221 + 5,
                    UpdatedAt = new DateTime(2021, 4, 9, 15, 44, 20),
                    FilePath = "",
                    HasSubmission = true,
                    HasFile = false,
                    CourseId = crs10221,
                    SectionId = sec10221
                }, new Submission
                {
                    Id = sub10221 + 6,
                    Description = "we submitted",
                    IsGraded = false,
                    AffiliatedAssignmentId = as10221,
                    AffiliatedGroupId = gr10221 + 6,
                    UpdatedAt = new DateTime(2021, 4, 9, 15, 44, 20),
                    FilePath = "",
                    HasSubmission = true,
                    HasFile = false,
                    CourseId = crs10221,
                    SectionId = sec10221
                }, new Submission
                {
                    Id = sub10221 + 7,
                    Description = "",
                    IsGraded = false,
                    AffiliatedAssignmentId = as10221,
                    AffiliatedGroupId = gr10221 + 7,
                    FilePath = "",
                    HasSubmission = false,
                    HasFile = false,
                    CourseId = crs10221,
                    SectionId = sec10221
                }, new Submission
                {
                    Id = sub10221 + 8,
                    Description = "hi hocam, we worked hard for this assignment",
                    IsGraded = false,
                    AffiliatedAssignmentId = as10221,
                    AffiliatedGroupId = gr10221 + 8,
                    UpdatedAt = new DateTime(2021, 4, 9, 15, 44, 20),
                    FilePath = "",
                    HasSubmission = true,
                    HasFile = false,
                    CourseId = crs10221,
                    SectionId = sec10221
                }, new Submission
                {
                    Id = sub10221 + 9,
                    Description = "",
                    IsGraded = false,
                    AffiliatedAssignmentId = as10221,
                    AffiliatedGroupId = gr10221 + 9,
                    FilePath = "",
                    HasSubmission = false,
                    HasFile = false,
                    CourseId = crs10221,
                    SectionId = sec10221
                }, new Submission
                {
                    Id = sub10221 + 10,
                    Description = "can you please give extensive feedback>",
                    IsGraded = false,
                    AffiliatedAssignmentId = as10221,
                    AffiliatedGroupId = gr10221 + 10,
                    UpdatedAt = new DateTime(2021, 4, 9, 15, 44, 20),
                    FilePath = "",
                    HasSubmission = true,
                    HasFile = false,
                    CourseId = crs10221,
                    SectionId = sec10221
                }, new Submission
                {
                    Id = sub10221 + 11,
                    Description = "Hey, this is our report",
                    IsGraded = false,
                    AffiliatedAssignmentId = as10221,
                    AffiliatedGroupId = gr10221 + 11,
                    UpdatedAt = new DateTime(2021, 4, 9, 15, 44, 20),
                    FilePath = "",
                    HasSubmission = true,
                    HasFile = false,
                    CourseId = crs10221,
                    SectionId = sec10221
                }, new Submission
                {
                    Id = sub10221 + 12,
                    Description = "Hello, we submitted our report",
                    IsGraded = false,
                    AffiliatedAssignmentId = as10221,
                    AffiliatedGroupId = gr10221 + 12,
                    UpdatedAt = new DateTime(2021, 4, 9, 15, 44, 20),
                    FilePath = "",
                    HasSubmission = true,
                    HasFile = false,
                    CourseId = crs10221,
                    SectionId = sec10221 + 1
                }, new Submission
                {
                    Id = sub10221 + 13,
                    Description = "Hey, this is our report",
                    IsGraded = false,
                    AffiliatedAssignmentId = as10221,
                    AffiliatedGroupId = gr10221 + 13,
                    UpdatedAt = new DateTime(2021, 4, 9, 15, 44, 20),
                    FilePath = "",
                    HasSubmission = true,
                    HasFile = false,
                    CourseId = crs10221,
                    SectionId = sec10221 + 1
                }, new Submission
                {
                    Id = sub315,
                    Description = "",
                    IsGraded = false,
                    AffiliatedAssignmentId = as315,
                    AffiliatedGroupId = 3,
                    //UpdatedAt = new DateTime(2021, 4, 9, 15, 44, 20),
                    FilePath = "",
                    HasSubmission = false,
                    HasFile = false,
                    CourseId = crs315,
                    SectionId = sec315
                }, new Submission
                {
                    Id = sub315 + 1,
                    Description = "",
                    IsGraded = false,
                    AffiliatedAssignmentId = as315,
                    AffiliatedGroupId = 4,
                    //UpdatedAt = new DateTime(2021, 4, 9, 15, 44, 20),
                    FilePath = "",
                    HasSubmission = false,
                    HasFile = false,
                    CourseId = crs315,
                    SectionId = sec315
                }, new Submission
                {
                    Id = sub315 + 2,
                    Description = "",
                    IsGraded = false,
                    AffiliatedAssignmentId = as315,
                    AffiliatedGroupId = 10,
                    //UpdatedAt = new DateTime(2021, 4, 9, 15, 44, 20),
                    FilePath = "",
                    HasSubmission = false,
                    HasFile = false,
                    CourseId = crs315,
                    SectionId = sec315
                }, new Submission
                {
                    Id = sub315 + 3,
                    Description = "",
                    IsGraded = false,
                    AffiliatedAssignmentId = as315,
                    AffiliatedGroupId = 11,
                    //UpdatedAt = new DateTime(2021, 4, 9, 15, 44, 20),
                    FilePath = "",
                    HasSubmission = false,
                    HasFile = false,
                    CourseId = crs315,
                    SectionId = sec315
                }, new Submission
                {
                    Id = sub315 + 4,
                    Description = "",
                    SrsGrade = (decimal)9.5,
                    IsGraded = true,
                    AffiliatedAssignmentId = as315 + 1,
                    AffiliatedGroupId = 3,
                    //UpdatedAt = new DateTime(2021, 4, 9, 15, 44, 20),
                    FilePath = "",
                    HasSubmission = false,
                    HasFile = false,
                    CourseId = crs315,
                    SectionId = sec315
                }, new Submission
                {
                    Id = sub315 + 5,
                    Description = "",
                    IsGraded = true,
                    SrsGrade = (decimal)9,
                    AffiliatedAssignmentId = as315 + 1,
                    AffiliatedGroupId = 4,
                    //UpdatedAt = new DateTime(2021, 4, 9, 15, 44, 20),
                    FilePath = "",
                    HasSubmission = false,
                    HasFile = false,
                    CourseId = crs315,
                    SectionId = sec315
                }, new Submission
                {
                    Id = sub315 + 6,
                    Description = "",
                    IsGraded = true,
                    SrsGrade = (decimal)8,
                    AffiliatedAssignmentId = as315 +  1,
                    AffiliatedGroupId = 10,
                    //UpdatedAt = new DateTime(2021, 4, 9, 15, 44, 20),
                    FilePath = "",
                    HasSubmission = false,
                    HasFile = false,
                    CourseId = crs315,
                    SectionId = sec315
                }, new Submission
                {
                    Id = sub315 + 7,
                    Description = "",
                    IsGraded = true,
                    SrsGrade = (decimal)7,
                    AffiliatedAssignmentId = as315 + 1, 
                    AffiliatedGroupId = 11,
                    //UpdatedAt = new DateTime(2021, 4, 9, 15, 44, 20),
                    FilePath = "",
                    HasSubmission = true,
                    HasFile = false,
                    CourseId = crs315,
                    SectionId = sec315
                }

                // 3'ten 11'e kadar
            );

            /*
            modelBuilder.Entity<Comment>().HasData(
            
                new Comment
                {
                    Id = 1,
                    CommentedUserId = 24,
                    CommentedSubmissionId = 1,
                    CommentText = "You could make these improvements: ....",
                    MaxGrade = 10,
                    Grade = 8,
                    CreatedAt = new DateTime(2021, 4, 14, 12, 2, 3),
                    FileAttachmentAvailability = false,
                    FilePath = ""
                }, new Comment
                {
                    Id = 2,
                    CommentedUserId = 25,
                    CommentedSubmissionId = 1,
                    CommentText = "It's a nice report but I still have some suggestions",
                    MaxGrade = 10,
                    Grade = (decimal)9.5,
                    CreatedAt = new DateTime(2021, 4, 1, 11, 2, 3),
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
                    CreatedAt = new DateTime(2021, 4, 2, 11, 2, 3),
                    FileAttachmentAvailability = false,
                    FilePath = ""
                }, new Comment
                {
                    Id = 4,
                    CommentedUserId = 16,
                    CommentedSubmissionId = 1,
                    CommentText = "You could use more diagrams",
                    MaxGrade = 10,
                    Grade = (decimal)9.4,
                    CreatedAt = new DateTime(2021, 4, 2, 11, 2, 3),
                    FileAttachmentAvailability = false,
                    FilePath = ""
                }, new Comment
                {
                    Id = 5,
                    CommentedUserId = 17,
                    CommentedSubmissionId = 1,
                    CommentText = "Wonderful",
                    MaxGrade = 10,
                    Grade = (decimal)10,
                    CreatedAt = new DateTime(2021, 4, 2, 11, 2, 3),
                    FileAttachmentAvailability = false,
                    FilePath = ""
                }, new Comment
                {
                    Id = 6,
                    CommentedUserId = 2,
                    CommentedSubmissionId = 1,
                    CommentText = "You can consider using facade design pattern",
                    MaxGrade = 10,
                    Grade = (decimal)9,
                    CreatedAt = new DateTime(2021, 4, 2, 11, 2, 3),
                    FileAttachmentAvailability = false,
                    FilePath = ""
                }

            );*/

            modelBuilder.Entity<JoinRequest>().HasData(
                 /*
                 new JoinRequest
                 {
                     Id = 1,
                     RequestingStudentId = 14,
                     Description = "Hi, I want to join your group. I know .net",
                     RequestedGroupId = 5,
                     CreatedAt = new DateTime(2021, 4, 29, 10, 23, 2),
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
                     CreatedAt = new DateTime(2021, 4, 29, 10, 23, 2),
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
                     CreatedAt = new DateTime(2021, 4, 29, 10, 23, 2),
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
                     CreatedAt = new DateTime(2021, 4, 29, 10, 29, 2),
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
                     CreatedAt = new DateTime(2021, 4, 29, 10, 30, 2),
                     AcceptedNumber = 1,
                     Accepted = false,
                     Resolved = false,
                     VotedStudents = "26"
                 }
                 /*, new JoinRequest
                 {
                     Id = 6,
                     RequestingStudentId = 9,
                     Description = "",
                     RequestedGroupId = 13,
                     CreatedAt = new DateTime(2021, 4, 29, 14, 30, 2),
                     AcceptedNumber = 0,
                     Accepted = false,
                     Resolved = false,
                     VotedStudents = ""
                 }*//*, new JoinRequest
                 {
                     Id = 7,
                     RequestingStudentId = 7,
                     Description = "take me",
                     RequestedGroupId = 1,
                     CreatedAt = new DateTime(2021, 4, 15, 14, 30, 2),
                     AcceptedNumber = 5,
                     Accepted = true,
                     Resolved = false,
                     VotedStudents = "1 3 4 5 6"
                 },*/

                 new JoinRequest
                 {
                     Id = 8,
                     RequestingStudentId = 25,
                     Description = "take me",
                     RequestedGroupId = 1,
                     CreatedAt = new DateTime(2021, 4, 15, 10, 30, 2),
                     AcceptedNumber = 2,
                     Accepted = false,
                     Resolved = true,
                     VotedStudents = "1 3 4"
                 },
                 new JoinRequest
                 {
                     Id = 9,
                     RequestingStudentId = 5,
                     Description = "I know deep learning",
                     RequestedGroupId = 5,
                     CreatedAt = new DateTime(2021, 4, 15, 10, 30, 2),
                     AcceptedNumber = 0,
                     Accepted = false,
                     Resolved = false,
                     VotedStudents = ""
                 }, new JoinRequest
                 {
                     Id = 10,
                     RequestingStudentId = 5,
                     Description = "We'd make a good team together",
                     RequestedGroupId = 9,
                     CreatedAt = new DateTime(2021, 4, 15, 10, 30, 2),
                     AcceptedNumber = 2,
                     Accepted = false,
                     Resolved = false,
                     VotedStudents = "27 28"
                 }, new JoinRequest
                 {
                     Id = 11,
                     RequestingStudentId = 24,
                     Description = "I have experience with web development",
                     RequestedGroupId = 1,
                     CreatedAt = new DateTime(2021, 4, 15, 10, 30, 2),
                     AcceptedNumber = 1,
                     Accepted = false,
                     Resolved = false,
                     VotedStudents = "3"
                 }, new JoinRequest
                 {
                     Id = 12,
                     RequestingStudentId = 14,
                     Description = "I'm good UX designer guys",
                     RequestedGroupId = 1,
                     CreatedAt = new DateTime(2021, 4, 15, 10, 30, 2),
                     AcceptedNumber = 0,
                     Accepted = false,
                     Resolved = false,
                     VotedStudents = ""
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
                    CreatedAt = new DateTime(2021, 4, 13, 10, 23, 2),
                    Accepted = false,
                    Resolved = false
                }, new MergeRequest
                {
                    Id = 2,
                    SenderGroupId = 9,
                    Description = "You can see our past projects from our pages. We'd be good team if we merged",
                    ReceiverGroupId = 2,
                    VotedStudents = "27",
                    CreatedAt = new DateTime(2021, 4, 13, 14, 23, 2),
                    Accepted = false,
                    Resolved = false
                }, new MergeRequest
                {
                    Id = 3,
                    SenderGroupId = 2,
                    Description = "",
                    ReceiverGroupId = 6,
                    VotedStudents = "13",
                    CreatedAt = new DateTime(2021, 4, 13, 16, 23, 2),
                    Accepted = false,
                    Resolved = false
                }
                /*, new MergeRequest
                {
                    Id = 4,
                    SenderGroupId = 12,
                    Description = "let's merge",
                    ReceiverGroupId = 11,
                    VotedStudents = "9",
                    CreatedAt = new DateTime(2021, 4, 13, 17, 23, 2),
                    Accepted = false,
                    Resolved = false
                }, new MergeRequest
                {
                    Id = 5,
                    SenderGroupId = 10,
                    Description = "",
                    ReceiverGroupId = 13,
                    VotedStudents = "",
                    CreatedAt = new DateTime(2021, 4, 13, 17, 23, 2),
                    Accepted = false,
                    Resolved = false
                }, new MergeRequest
                {
                    Id = 6,
                    SenderGroupId = 13,
                    Description = "",
                    ReceiverGroupId = 11,
                    VotedStudents = "10 28",
                    CreatedAt = new DateTime(2021, 4, 13, 19, 23, 2),
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
                },*/
                 , new MergeRequest
                 {
                     Id = 8,
                     SenderGroupId = 5,
                     Description = "hey, merge",
                     ReceiverGroupId = 1,
                     VotedStudents = "1 3 24 4",
                     CreatedAt = new DateTime(2021, 4, 10, 19, 23, 2),
                     Accepted = false,
                     Resolved = true
                 }, new MergeRequest
                 {
                     Id = 9,
                     SenderGroupId = gr319,
                     Description = "we are good team players",
                     ReceiverGroupId = 1,
                     VotedStudents = "4 3 1",
                     CreatedAt = new DateTime(2021, 4, 10, 19, 23, 2),
                     Accepted = false,
                     Resolved = false
                 }, new MergeRequest
                 {
                     Id = 10,
                     SenderGroupId = 5,
                     Description = "hey, we are good with design patterns",
                     ReceiverGroupId = 1,
                     VotedStudents = "24 25",
                     CreatedAt = new DateTime(2021, 4, 10, 19, 23, 2),
                     Accepted = false,
                     Resolved = false
                 }, new MergeRequest
                 {
                     Id = 11,
                     SenderGroupId = 1,
                     Description = "We're good at algorithms",
                     ReceiverGroupId = 8,
                     VotedStudents = "3",
                     CreatedAt = new DateTime(2021, 4, 10, 19, 23, 2),
                     Accepted = false,
                     Resolved = false
                 }, new MergeRequest
                 {
                     Id = 12,
                     SenderGroupId = 1,
                     Description = "We'd make a strong team since we know front-end and you know back-end",
                     ReceiverGroupId = 9,
                     VotedStudents = "5 28 3",
                     CreatedAt = new DateTime(2021, 4, 10, 19, 23, 2),
                     Accepted = false,
                     Resolved = false
                 }
            );
            modelBuilder.Entity<PeerGradeAssignment>().HasData(
                /*new PeerGradeAssignment
                {
                    Id = 1,
                    CourseId = 1,
                    DueDate = new DateTime(2021, 6, 1, 1, 0, 0),
                    LastEdited = new DateTime(2021, 4, 1, 1, 1, 0, 0),
                    MaxGrade = 10
                }, */
                new PeerGradeAssignment
                {
                    Id = 2,
                    CourseId = crs315,
                    DueDate = new DateTime(2021, 8, 1, 1, 0, 0),
                    LastEdited = new DateTime(2021, 4, 1, 1, 1, 0, 0),
                    MaxGrade = 10
                }
            );
            //modelBuilder.Entity<ProjectGrade>().HasData(
            /*
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
                LastEdited = new DateTime(2021, 4, 1, 2, 1, 0, 0)
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
                LastEdited = new DateTime(2021, 4, 1, 2, 1, 1, 0)
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
                LastEdited = new DateTime(2021, 4, 1, 2, 1, 2, 0)
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
                LastEdited = new DateTime(2021, 4, 1, 2, 1, 3, 0)
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
                LastEdited = new DateTime(2021, 4, 1, 2, 1, 4, 0)
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
                LastEdited = new DateTime(2021, 4, 1, 2, 1, 5, 0)
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
                LastEdited = new DateTime(2021, 4, 1, 2, 1, 6, 0)
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
                LastEdited = new DateTime(2021, 4, 1, 2, 1, 7, 0)
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
                LastEdited = new DateTime(2021, 4, 1, 2, 1, 8, 0)
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
                LastEdited = new DateTime(2021, 4, 1, 2, 1, 9, 0)
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
                LastEdited = new DateTime(2021, 4, 1, 2, 1, 10, 0)
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
                LastEdited = new DateTime(2021, 4, 1, 2, 1, 11, 0)
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
                LastEdited = new DateTime(2021, 4, 1, 2, 1, 11, 0)
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
                LastEdited = new DateTime(2021, 4, 1, 2, 1, 11, 0)
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
                LastEdited = new DateTime(2021, 4, 1, 2, 1, 11, 0)
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
                LastEdited = new DateTime(2021, 4, 1, 2, 1, 11, 0)
            }*/
            //);
            modelBuilder.Entity<PeerGrade>().HasData(

                new PeerGrade
                {
                    Id = 1,
                    ProjectGroupId = 4,
                    ReviewerId = 5,
                    RevieweeId = 6,
                    MaxGrade = 10,
                    Grade = 10,
                    Comment = "best teammate ever",
                    PeerGradeAssignmentId = 2,
                    LastEdited = new DateTime(2021, 4, 3, 1, 1, 1)
                }

                , new PeerGrade
                {
                    Id = 2,
                    ProjectGroupId = 1,
                    ReviewerId = 5,
                    RevieweeId = 7,
                    MaxGrade = 10,
                    Grade = 4,
                    Comment = "He didn't do anything",
                    PeerGradeAssignmentId = 2,
                    LastEdited = new DateTime(2021, 4, 1, 3, 1, 1, 1)
                }
                /*, new PeerGrade
                {
                    Id = 3,
                    ProjectGroupId = 1,
                    ReviewerId = 1,
                    RevieweeId = 5,
                    MaxGrade = 10,
                    Grade = 8,
                    Comment = "",
                    PeerGradeAssignmentId = 1,
                    LastEdited = new DateTime(2021, 4, 1, 2, 1, 2, 0)
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
                    LastEdited = new DateTime(2021, 4, 1, 2, 1, 3, 0)
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
                    LastEdited = new DateTime(2021, 4, 1, 2, 1, 4, 0)
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
                    LastEdited = new DateTime(2021, 4, 1, 2, 1, 5, 0)
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
                    LastEdited = new DateTime(2021, 4, 1, 2, 1, 6, 0)
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
                    LastEdited = new DateTime(2021, 4, 1, 2, 1, 7, 0)
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
                    LastEdited = new DateTime(2021, 4, 1, 2, 1, 8, 0)
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
                    LastEdited = new DateTime(2021, 4, 1, 2, 1, 9, 0)
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
                    LastEdited = new DateTime(2021, 4, 1, 2, 1, 10, 0)
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
                    LastEdited = new DateTime(2021, 4, 1, 2, 1, 11, 0)
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
                    LastEdited = new DateTime(2021, 4, 1, 2, 1, 12, 0)
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
                    LastEdited = new DateTime(2021, 4, 1, 2, 1, 13, 0)
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
                    LastEdited = new DateTime(2021, 4, 1, 2, 1, 14, 0)
                }
                */
            );

        }
    }
}