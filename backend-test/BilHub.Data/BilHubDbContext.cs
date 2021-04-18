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

        public BilHubDbContext(DbContextOptions<BilHubDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .ApplyConfiguration(new StudentConfiguration());

            builder
                .ApplyConfiguration(new CourseConfiguration());

            builder
                .ApplyConfiguration(new InstructorConfiguration());
        }
    }
}