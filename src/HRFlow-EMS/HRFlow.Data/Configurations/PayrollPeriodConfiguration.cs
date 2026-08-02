using HRFlow.Entities.HumanResources;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRFlow.Data.Configurations
{
    public class PayrollPeriodConfiguration : IEntityTypeConfiguration<PayrollPeriod>
    {
        public void Configure(EntityTypeBuilder<PayrollPeriod> builder)
        {
            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(x => new { x.Year, x.Month })
                .IsUnique();
        }
    }
}
