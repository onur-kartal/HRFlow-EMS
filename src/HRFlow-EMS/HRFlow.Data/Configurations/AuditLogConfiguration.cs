using HRFlow.Entities.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRFlow.Data.Configurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.Property(x => x.UserId).HasMaxLength(450);
            builder.Property(x => x.UserName).HasMaxLength(256);
            builder.Property(x => x.Role).HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(1000).IsRequired();
            builder.Property(x => x.IpAddress).HasMaxLength(45);
        }
    }
}
