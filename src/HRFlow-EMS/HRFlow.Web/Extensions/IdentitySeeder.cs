using HRFlow.Common.Constants;
using HRFlow.Data.Context;
using HRFlow.Entities.HumanResources;
using HRFlow.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Web.Extensions
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<HRFlowDbContext>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<SystemUser>>();

            // Roller
            string[] roles =
                            {
                               Roles.Admin,
                               Roles.HR,
                               Roles.Manager,
                               Roles.Employee
                            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Admin personeli
            var adminEmployee = await dbContext.Employees
                .FirstOrDefaultAsync(x => x.Email == "admin@hrflow.com");

            if (adminEmployee == null)
            {
                adminEmployee = new Employee
                {
                    FirstName = "System",
                    LastName = "Administrator",
                    Email = "admin@hrflow.com",
                    PhoneNumber = null,
                    HireDate = DateTime.Today,
                    Salary = 0,
                    IsActive = true,
                    DepartmentId = 1,
                    PositionId = 1
                };

                dbContext.Employees.Add(adminEmployee);
                await dbContext.SaveChangesAsync();
            }

            // Admin kullanıcısı
            var adminUser = await userManager.FindByNameAsync("admin");

            if (adminUser == null)
            {
                adminUser = new SystemUser
                {
                    UserName = "admin",
                    Email = "admin@hrflow.com",
                    EmployeeId = adminEmployee.Id
                };

                var result = await userManager.CreateAsync(adminUser, "123456");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, Roles.Admin);
                }
            }
        }
    }
}
