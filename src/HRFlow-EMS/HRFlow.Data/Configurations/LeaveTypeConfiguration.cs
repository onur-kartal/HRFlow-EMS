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
    public class LeaveTypeConfiguration : IEntityTypeConfiguration<LeaveType>
    {
        public void Configure(EntityTypeBuilder<LeaveType> builder)
        {
            builder.Property(x=>x.Name)
                    .HasMaxLength(100)
                    .IsRequired();
            builder.Property(x=>x.DefaultDays)
                    .IsRequired();
            builder.Property(x => x.IsPaid)
                    .IsRequired();
        }
    }
}
