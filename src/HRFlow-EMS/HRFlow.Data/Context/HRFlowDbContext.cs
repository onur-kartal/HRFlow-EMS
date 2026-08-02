using HRFlow.Entities.HumanResources;
using HRFlow.Entities.Identity;
using HRFlow.Entities.Organization;
using HRFlow.Entities.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Data.Context
{
    public class HRFlowDbContext : IdentityDbContext<SystemUser>
    {
        public HRFlowDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees => Set<Employee>();

        public DbSet<Department> Departments => Set<Department>();

        public DbSet<Position> Positions => Set<Position>();

        public DbSet<LeaveType> LeaveTypes => Set<LeaveType>();

        public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();

        public DbSet<OvertimeRequest> OvertimeRequests => Set<OvertimeRequest>();

        public DbSet<Announcement> Announcements => Set<Announcement>();

        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        public DbSet<RequestLog> RequestLogs => Set<RequestLog>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<PayrollPeriod> PayrollPeriods => Set<PayrollPeriod>();
        public DbSet<EmployeePayroll> EmployeePayrolls => Set<EmployeePayroll>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SystemUser>().ToTable("SystemUsers");

            modelBuilder.Entity<IdentityRole>().ToTable("Roles");

            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");

            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");

            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");

            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");

            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HRFlowDbContext).Assembly);
        }
    }
}
