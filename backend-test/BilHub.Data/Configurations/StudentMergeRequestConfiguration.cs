using BilHub.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BilHub.Data.Configurations
{
    public class StudentMergeRequestConfiguration : IEntityTypeConfiguration<StudentMergeRequest>
    {
        public void Configure(EntityTypeBuilder<StudentMergeRequest> builder)
        {
            builder
                .HasKey(smr => new { smr.StudentId, smr.MergeRequestId });

            builder
                .HasOne(smr => smr.Student)
                .WithMany(s => s.UnvotedMergeRequests)
                .HasForeignKey(mr => mr.StudentId);

            builder
                .HasOne(smr => smr.MergeRequest)
                .WithMany(mr => mr.VotedStudents)
                .HasForeignKey(s => s.MergeRequestId);
        }
    }
}