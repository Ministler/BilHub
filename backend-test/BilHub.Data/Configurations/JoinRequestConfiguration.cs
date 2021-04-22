using BilHub.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BilHub.Data.Configurations
{
    public class JoinRequestConfiguration : IEntityTypeConfiguration<JoinRequest>
    {
        public void Configure(EntityTypeBuilder<JoinRequest> builder)
        {
            builder
            .HasMany(jr => jr.VotedStudents)
            .WithOne(sjr => sjr.JoinRequest)
            .HasForeignKey(jr => jr.JoinRequestId);
        }
    }
}