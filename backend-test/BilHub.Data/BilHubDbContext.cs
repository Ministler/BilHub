using Microsoft.EntityFrameworkCore;
using BilHub.Core.Models;
using BilHub.Data.Configurations;

namespace BilHub.Data
{
    public class BilHubDbContext : DbContext
    {
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<GroupSize> GroupSizes { get; set; }
        public DbSet<InstructorCourse> InstructorCourses { get; set; }
        public DbSet<JoinRequest> JoinRequests { get; set; }
        public DbSet<MergeRequest> MergeRequests { get; set; }
        public DbSet<PeerGrade> PeerGrades { get; set; }
        public DbSet<PeerGradeAssignment> PeerGradeAssignments { get; set; }
        public DbSet<ProjectGrade> ProjectGrades { get; set; }
        public DbSet<ProjectGroup> ProjectGroups { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<StudentJoinRequest> StudentJoinRequests { get; set; }
        public DbSet<StudentMergeRequest> StudentMergeRequests { get; set; }
        public DbSet<StudentProjectGroup> StudentProjectGroups { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<UnvotedJoinRequest> UnvotedJoinRequests { get; set; }

        public BilHubDbContext(DbContextOptions<BilHubDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .ApplyConfiguration(new StudentConfiguration());
            builder
                .ApplyConfiguration(new CourseConfiguration());
            builder
                .ApplyConfiguration(new InstructorConfiguration());
            builder
                .ApplyConfiguration(new MergeRequestConfiguration());
            builder
                .ApplyConfiguration(new ProjectGroupConfiguration());
            builder
                .ApplyConfiguration(new InstructorCourseConfiguration());
            builder
                .ApplyConfiguration(new StudentJoinRequestConfiguration());
            builder
                .ApplyConfiguration(new JoinRequestConfiguration());
            builder
                .ApplyConfiguration(new StudentCourseConfiguration());
            builder
                .ApplyConfiguration(new AssitantCourseConfiguration());
            builder
                .ApplyConfiguration(new StudentMergeRequestConfiguration());
            builder
                .ApplyConfiguration(new StudentProjectGroupConfiguration());
            builder
                .ApplyConfiguration(new UnvotedJoinRequestConfiguration());

        }
    }
}