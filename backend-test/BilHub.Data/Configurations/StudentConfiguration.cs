using BilHub.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BilHub.Data.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
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
                .HasMany(s => s.TakenCourses)
                .WithOne(sc => sc.Student)
                .HasForeignKey(m => m.StudentId);

            builder
                .HasMany(s => s.AssistedCourses)
                .WithOne(ac => ac.Student)
                .HasForeignKey(m => m.StudentId);

            builder
                .HasMany(s => s.OutgoingJoinRequests)
                .WithOne(sjr => sjr.Student)
                .HasForeignKey(s => s.StudentId);

            builder
                .HasMany(s => s.UnvotedJoinRequests)
                .WithOne(ujr => ujr.Student)
                .HasForeignKey(s => s.StudentId);

            builder
                .ToTable("Students");
        }
    }
}