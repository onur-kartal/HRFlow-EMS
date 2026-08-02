using HRFlow.Business.Interfaces;
using HRFlow.Business.Mappings;
using HRFlow.Business.Services;
using HRFlow.Common.Interfaces;
using HRFlow.Data.Context;
using HRFlow.Data.Interfaces;
using HRFlow.Data.Repositories;
using HRFlow.Entities.HumanResources;
using HRFlow.Entities.Identity;
using HRFlow.Entities.Organization;
using HRFlow.Entities.Logging;
using HRFlow.Web.Services;
using Microsoft.AspNetCore.Identity;
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

            services.AddIdentity<SystemUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;

                options.User.RequireUniqueEmail = false;
            }).AddEntityFrameworkStores<HRFlowDbContext>().AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";

                options.AccessDeniedPath = "/Account/AccessDenied";

                options.LogoutPath = "/Account/Logout";

                options.Cookie.Name = "HRFlowAuth";

                options.SlidingExpiration = true;

                options.ExpireTimeSpan = TimeSpan.FromHours(8);
            });

            services.AddScoped<EmployeeRepository>();
            services.AddScoped<DepartmentRepository>();
            services.AddScoped<PositionRepository>();

            services.AddScoped<IGenericRepository<Employee>, EmployeeRepository>();
            services.AddScoped<IGenericRepository<Department>, DepartmentRepository>();
            services.AddScoped<IGenericRepository<Position>, PositionRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();

            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IPositionService, PositionService>();

            services.AddScoped<IDashboardService, DashboardService>();

            services.AddScoped<ILeaveTypeService, LeaveTypeService>();
            services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
            services.AddScoped<IGenericRepository<LeaveType>, LeaveTypeRepository>();

            services.AddScoped<IGenericRepository<LeaveRequest>, LeaveRequestRepository>();
            services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
            services.AddScoped<ILeaveRequestService, LeaveRequestService>();

            services.AddScoped<IGenericRepository<OvertimeRequest>, OvertimeRequestRepository>();
            services.AddScoped<IOvertimeRequestRepository, OvertimeRequestRepository>();
            services.AddScoped<IOvertimeRequestService, OvertimeRequestService>();

            services.AddScoped<IGenericRepository<PayrollPeriod>, PayrollPeriodRepository>();
            services.AddScoped<IPayrollPeriodRepository, PayrollPeriodRepository>();
            services.AddScoped<IGenericRepository<EmployeePayroll>, EmployeePayrollRepository>();
            services.AddScoped<IEmployeePayrollRepository, EmployeePayrollRepository>();
            services.AddScoped<IPayrollPeriodService, PayrollPeriodService>();
            services.AddScoped<IEmployeePayrollService, EmployeePayrollService>();
            services.AddScoped<IPayrollPdfService, PayrollPdfService>();

            services.AddScoped<IGenericRepository<Announcement>, AnnouncementRepository>();
            services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
            services.AddScoped<IAnnouncementService, AnnouncementService>();

            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<IAuditLogService, AuditLogService>();
            services.AddScoped<IRequestLogRepository, RequestLogRepository>();
            services.AddScoped<IRequestLogService, RequestLogService>();
            services.AddScoped<IGenericRepository<Notification>, NotificationRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationService, NotificationService>();

            services.AddScoped<IAccountService, AccountService>();

            services.AddHttpContextAccessor();

            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddAutoMapper(cfg => { }, typeof(MappingProfile));
            return services;
        }
    }
}
