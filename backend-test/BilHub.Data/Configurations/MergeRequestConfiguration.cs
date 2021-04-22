using BilHub.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BilHub.Data.Configurations
{
    public class MergeRequestConfiguration : IEntityTypeConfiguration<MergeRequest>
    {
        public void Configure(EntityTypeBuilder<MergeRequest> builder)
        {
            builder.HasKey(mr => mr.Id);

            builder
                .HasOne(mr => mr.SenderGroup)
                .WithMany(pg => pg.OutgoingMergeRequest)
                .HasForeignKey(mr => mr.Id);

            builder
                .HasOne(mr => mr.ReceiverGroup)
                .WithMany(pg => pg.IncomingMergeRequest)
                .HasForeignKey(mr => mr.Id);
        }
    }
}