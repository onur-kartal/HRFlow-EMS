using HRFlow.Entities.HumanResources;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRFlow.Data.Configurations
{
    public class OvertimeRequestConfiguration : IEntityTypeConfiguration<OvertimeRequest>
    {
        public void Configure(EntityTypeBuilder<OvertimeRequest> builder)
        {
            builder.Property(x => x.Description)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.TotalHours)
                .HasPrecision(5, 2);

            builder.Property(x => x.ApprovedBy)
                .HasMaxLength(450);

            builder.HasOne(x => x.Employee)
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
