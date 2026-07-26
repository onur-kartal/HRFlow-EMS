using HRFlow.Entities.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRFlow.Data.Configurations
{
    public class RequestLogConfiguration : IEntityTypeConfiguration<RequestLog>
    {
        public void Configure(EntityTypeBuilder<RequestLog> builder)
        {
            builder.Property(x => x.UserId).HasMaxLength(450);
            builder.Property(x => x.UserName).HasMaxLength(256);
            builder.Property(x => x.Role).HasMaxLength(100);
            builder.Property(x => x.IpAddress).HasMaxLength(45);
            builder.Property(x => x.RequestPath).HasMaxLength(1000).IsRequired();
            builder.Property(x => x.HttpMethod).HasMaxLength(10).IsRequired();
            builder.Property(x => x.UserAgent).HasMaxLength(1000);
            builder.Property(x => x.Browser).HasMaxLength(100);
            builder.Property(x => x.OperatingSystem).HasMaxLength(100);
        }
    }
}
