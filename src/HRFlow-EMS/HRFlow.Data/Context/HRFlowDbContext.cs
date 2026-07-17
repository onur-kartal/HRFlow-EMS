using HRFlow.Entities.HumanResources;
using HRFlow.Entities.Organization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Data.Context
{
    public class HRFlowDbContext : DbContext
    {
        public HRFlowDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees => Set<Employee>();

        public DbSet<Department> Departments => Set<Department>();

        public DbSet<Position> Positions => Set<Position>();

        public DbSet<LeaveType> LeaveTypes => Set<LeaveType>();

        public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HRFlowDbContext).Assembly);
        }
    }
}
