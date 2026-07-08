using HRFlow.Business.Interfaces;
using HRFlow.Business.Services;
using HRFlow.Common.Interfaces;
using HRFlow.Data.Context;
using HRFlow.Data.Repositories;
using HRFlow.Entities.HumanResources;
using HRFlow.Entities.Organization;
using Microsoft.EntityFrameworkCore;

namespace HRFlow.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            services.AddDbContext<HRFlowDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<EmployeeRepository>();
            services.AddScoped<DepartmentRepository>();
            services.AddScoped<PositionRepository>();

            services.AddScoped<IGenericRepository<Employee>, EmployeeRepository>();
            services.AddScoped<IGenericRepository<Department>, DepartmentRepository>();
            services.AddScoped<IGenericRepository<Position>, PositionRepository>();

            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IPositionService, PositionService>();

            return services;
        }
    }
}
