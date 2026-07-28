using HRFlow.Entities.HumanResources;
using HRFlow.Entities.Identity;
using HRFlow.Entities.Organization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Data.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(x => x.FirstName)
                    .HasMaxLength(100)
                    .IsRequired();

            builder.Property(x => x.LastName)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.Email)
                   .HasMaxLength(150)
                   .IsRequired();

            builder.Property(x => x.PhoneNumber)
                   .HasMaxLength(20);
            builder.Property(x => x.PersonalEmail).HasMaxLength(150);
            builder.Property(x => x.ProfileImagePath).HasMaxLength(500);
            builder.Property(x => x.Address).HasMaxLength(500);
            builder.Property(x => x.City).HasMaxLength(100);
            builder.Property(x => x.District).HasMaxLength(100);
            builder.Property(x => x.PostalCode).HasMaxLength(10);

            builder.Property(x => x.Salary)
                   .HasColumnType("decimal(18,2)");

            builder.HasOne(x => x.Department)
                    .WithMany(x => x.Employees)
                    .HasForeignKey(x => x.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Position)
                   .WithMany(x => x.Employees)
                   .HasForeignKey(x => x.PositionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SystemUser)
                   .WithOne(x => x.Employee)
                   .HasForeignKey<SystemUser>(x => x.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
