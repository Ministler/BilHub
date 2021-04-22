using BilHub.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BilHub.Data.Configurations
{
    public class ProjectGroupConfiguration : IEntityTypeConfiguration<ProjectGroup>
    {
        public void Configure(EntityTypeBuilder<ProjectGroup> builder)
        {
            builder.HasKey(pg => pg.Id);

            builder
                .HasMany(pg => pg.OutgoingMergeRequest)
                .WithOne(mr => mr.SenderGroup)
                .HasForeignKey(pg => pg.Id);

            builder
                .HasMany(pg => pg.IncomingMergeRequest)
                .WithOne(mr => mr.ReceiverGroup)
                .HasForeignKey(pg => pg.Id);
        }
    }
}