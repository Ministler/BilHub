using BilHub.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BilHub.Data.Configurations
{
    public class AssitantCourseConfiguration : IEntityTypeConfiguration<AssistantCourse>
    {
        public void Configure(EntityTypeBuilder<AssistantCourse> builder)
        {

            builder
                .HasKey(ac => new { ac.StudentId, ac.CourseId });

            builder
                .HasOne(sc => sc.Student)
                .WithMany(s => s.AssistedCourses)
                .HasForeignKey(c => c.StudentId);
            builder
                .HasOne(sc => sc.Course)
                .WithMany(s => s.Assistants)
                .HasForeignKey(c => c.CourseId);
        }

    }
}