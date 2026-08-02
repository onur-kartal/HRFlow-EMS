using HRFlow.Entities.HumanResources;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRFlow.Data.Configurations
{
    public class EmployeePayrollConfiguration : IEntityTypeConfiguration<EmployeePayroll>
    {
        public void Configure(EntityTypeBuilder<EmployeePayroll> builder)
        {
            builder.HasIndex(x => new { x.PayrollPeriodId, x.EmployeeId })
                .IsUnique();

            builder.Property(x => x.BaseSalary)
                .HasPrecision(18, 2);

            builder.Property(x => x.OvertimeHours)
                .HasPrecision(18, 2);

            builder.Property(x => x.OvertimeAmount)
                .HasPrecision(18, 2);

            builder.Property(x => x.Bonus)
                .HasPrecision(18, 2);

            builder.Property(x => x.Deduction)
                .HasPrecision(18, 2);

            builder.Property(x => x.NetSalary)
                .HasPrecision(18, 2);

            builder.HasOne(x => x.PayrollPeriod)
                .WithMany(x => x.EmployeePayrolls)
                .HasForeignKey(x => x.PayrollPeriodId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Employee)
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
