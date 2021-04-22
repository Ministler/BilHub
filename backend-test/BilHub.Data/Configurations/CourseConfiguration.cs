using BilHub.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BilHub.Data.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Id)
                .UseIdentityColumn();

            builder
                .Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(m => m.CourseInformation)
                .HasMaxLength(50);

            builder
                .HasMany(c => c.Instructors)
                .WithOne(ic => ic.Course)
                .HasForeignKey(c => c.CourseId);

            builder
                .HasMany(c => c.Assistants)
                .WithOne(ac => ac.Course)
                .HasForeignKey(c => c.CourseId);

            builder
                .HasMany(c => c.Students)
                .WithOne(sc => sc.Course)
                .HasForeignKey(s => s.CourseId);


            builder
                .ToTable("Courses");
        }
    }
}