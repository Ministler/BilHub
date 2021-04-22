using BilHub.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BilHub.Data.Configurations
{
    public class InstructorCourseConfiguration : IEntityTypeConfiguration<InstructorCourse>
    {
        public void Configure(EntityTypeBuilder<InstructorCourse> builder)
        {
            builder
                .HasKey(ic => new { ic.CourseId, ic.InstructorId });

            builder
                .HasOne(ic => ic.Course)
                .WithMany(c => c.Instructors)
                .HasForeignKey(ic => ic.CourseId);

            builder
                .HasOne(ic => ic.Instructor)
                .WithMany(i => i.InstructedCourses)
                .HasForeignKey(ic => ic.InstructorId);
        }
    }
}