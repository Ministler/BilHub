using BilHub.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BilHub.Data.Configurations
{
    public class UnvotedJoinRequestConfiguration : IEntityTypeConfiguration<UnvotedJoinRequest>
    {
        public void Configure(EntityTypeBuilder<UnvotedJoinRequest> builder)
        {
            builder
                .HasKey(ujr => new { ujr.StudentId, ujr.JoinRequestId });

            builder
                .HasOne(ujr => ujr.Student)
                .WithMany(s => s.UnvotedJoinRequests)
                .HasForeignKey(jr => jr.StudentId);

        }
    }
}