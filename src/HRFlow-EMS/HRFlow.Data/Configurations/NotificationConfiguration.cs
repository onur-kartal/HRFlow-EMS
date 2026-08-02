using HRFlow.Entities.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRFlow.Data.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.Property(x => x.UserId).HasMaxLength(450).IsRequired();
            builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Message).HasMaxLength(1000).IsRequired();
            builder.Property(x => x.Url).HasMaxLength(500);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.UserId, x.IsRead, x.CreatedDate });

            builder.HasIndex(x => new { x.UserId, x.SourceModule, x.SourceEntityId, x.EventKey })
                .IsUnique()
                .HasFilter("[SourceModule] IS NOT NULL AND [SourceEntityId] IS NOT NULL AND [EventKey] IS NOT NULL");
        }
    }
}
