using BilHub.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BilHub.Data.Configurations
{
    public class StudentJoinRequestConfiguration : IEntityTypeConfiguration<StudentJoinRequest>
    {
        public void Configure(EntityTypeBuilder<StudentJoinRequest> builder)
        {
            builder
                .HasKey(sjr => new { sjr.StudentId, sjr.JoinRequestId });

            // builder
            //     .HasOne(sjr => sjr.Student)
            //     .WithMany(s => s.UnvotedJoinRequests)
            //     .HasForeignKey(jr => jr.StudentId);

            builder
                .HasOne(sjr => sjr.Student)
                .WithMany(s => s.OutgoingJoinRequests)
                .HasForeignKey(jr => jr.StudentId);

            builder
                .HasOne(sjr => sjr.JoinRequest)
                .WithMany(jr => jr.VotedStudents)
                .HasForeignKey(s => s.JoinRequestId);
        }
    }
}