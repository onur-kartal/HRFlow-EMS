using HRFlow.Entities.HumanResources;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Data.Configurations
{
    public class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
    {
        public void Configure(EntityTypeBuilder<LeaveRequest> builder)
        {
            builder.Property(x => x.Description)
                   .HasMaxLength(500);

            builder.HasOne(x => x.Employee)
                   .WithMany()
                   .HasForeignKey(x => x.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.LeaveType)
                   .WithMany()
                   .HasForeignKey(x => x.LeaveTypeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
