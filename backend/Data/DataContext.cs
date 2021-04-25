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
        public DbSet<GroupSize> GroupSizes { get; set; }
        public DbSet<JoinRequest> JoinRequests { get; set; }
        public DbSet<MergeRequest> MergeRequests { get; set; }
        public DbSet<PeerGrade> PeerGrades { get; set; }
        public DbSet<PeerGradeAssignment> PeerGradeAssignments { get; set; }
        public DbSet<ProjectGrade> ProjectGrades { get; set; }
        public DbSet<ProjectGroup> ProjectGroups { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<MergeRequest>().HasOne(mr => mr.SenderGroup)
                .WithMany(g => g.OutgoingMergeRequest)
                .HasForeignKey(mr => mr.SenderGroupId);

            modelBuilder.Entity<MergeRequest>().HasOne(mr => mr.ReceiverGroup)
                .WithMany(g => g.IncomingMergeRequest)
                .HasForeignKey(mr => mr.ReceiverGroupId).OnDelete(DeleteBehavior.Restrict);

            // Utility.CreatePasswordHash("123456", out byte[] passwordHash, out byte[] passwordSalt);

            // modelBuilder.Entity<User>().HasData(
            //     new User { Id = 1, PasswordHash = passwordHash, SecondPasswordHash = passwordHash, PasswordSalt = passwordSalt, Email = "User1@gmail.com", UserType = 1 },
            //     new User { Id = 2, PasswordHash = passwordHash, SecondPasswordHash = passwordHash, PasswordSalt = passwordSalt, Email = "User2@gmail.com", UserType = 1 }
            // );

            // modelBuilder.Entity<InstructorCourse>().HasKey(ic => new { ic.InstructorId, ic.CourseId });
            // modelBuilder.Entity<StudentJoinRequest>().HasKey(sjr => new { sjr.StudentId, sjr.JoinRequestId });

            // modelBuilder.Entity<ProjectGroup>().HasMany(pg => pg.OutgoingMergeRequest)
            //     .WithOne(mr => mr.SenderGroup)
            //     .HasForeignKey(mr => mr.SenderGroupId);

            // modelBuilder.Entity<ProjectGroup>().HasMany(pg => pg.IncomingMergeRequest)
            //     .WithOne(mr => mr.ReceiverGroup)
            //     .HasForeignKey(mr => mr.ReceiverGroupId).OnDelete(DeleteBehavior.Restrict);

            // modelBuilder.Entity<Course>().HasMany(c => c.Instructors)
            //     .WithOne(i => i.Course).HasForeignKey(ic => ic.CourseId);

            // modelBuilder.Entity<User>().HasMany(i => i.InstructedCourses)
            //     .WithOne(ic => ic.Instructor).HasForeignKey(ic => ic.InstructorId);

            // modelBuilder.Entity<JoinRequest>().HasOne(jr => jr.RequestingStudent)
            //     .WithMany(s => s.OutgoingJoinRequests).HasForeignKey(s => s.UserId);

            // modelBuilder.Entity<User>().HasMany(i => i.UnvotedJoinRequests)
            //     .WithOne(sjr => sjr.Student).HasForeignKey(sjr => sjr.StudentId);

            // modelBuilder.Entity<Section>().HasMany(s => s.MergeRequests)
            //     .WithOne(mr => mr.AffiliatedSection).HasForeignKey(mr => mr.SectionId).OnDelete(false);

        }
    }
}