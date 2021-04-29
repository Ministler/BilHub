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
            
        }
    }
}