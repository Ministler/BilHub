using BilHub.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BilHub.Data.Configurations
{
    public class StudentProjectGroupConfiguration : IEntityTypeConfiguration<StudentProjectGroup>
    {
        public void Configure(EntityTypeBuilder<StudentProjectGroup> builder)
        {
            builder
                .HasKey(spg => new { spg.StudentId, spg.ProjectGroupId });

            builder
                .HasOne(spg => spg.Student)
                .WithMany(s => s.ProjectGroups)
                .HasForeignKey(pg => pg.StudentId);
            builder
                .HasOne(spg => spg.ProjectGroup)
                .WithMany(pg => pg.GroupMembers)
                .HasForeignKey(s => s.ProjectGroupId);
        }
    }
}