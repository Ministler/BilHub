using BilHub.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BilHub.Data.Configurations
{
    public class StudentCourseConfiguration : IEntityTypeConfiguration<StudentCourse>
    {
        public void Configure(EntityTypeBuilder<StudentCourse> builder)
        {

            builder
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            builder
                .HasOne(sc => sc.Student)
                .WithMany(s => s.TakenCourses)
                .HasForeignKey(c => c.StudentId);
            builder
                .HasOne(sc => sc.Course)
                .WithMany(s => s.Students)
                .HasForeignKey(c => c.CourseId);
        }
    }
}