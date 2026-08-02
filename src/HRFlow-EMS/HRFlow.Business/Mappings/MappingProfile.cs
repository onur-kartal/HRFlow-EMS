using AutoMapper;
using HRFlow.Business.DTOs.Department;
using HRFlow.Business.DTOs.Employee;
using HRFlow.Business.DTOs.LeaveRequest;
using HRFlow.Business.DTOs.LeaveType;
using HRFlow.Business.DTOs.Position;
using HRFlow.Business.DTOs.OvertimeRequest;
using HRFlow.Business.DTOs.Announcement;
using HRFlow.Business.DTOs.Logging;
using HRFlow.Entities.Logging;
using HRFlow.Entities.HumanResources;
using HRFlow.Entities.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<HRFlow.Entities.Identity.SystemUser, HRFlow.Business.DTOs.Account.ProfileDto>()
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => src.Employee.FirstName + " " + src.Employee.LastName))
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.UserName ?? string.Empty))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email ?? string.Empty))
                .ForMember(dest => dest.DepartmentName,
                    opt => opt.MapFrom(src => src.Employee.Department.Name))
                .ForMember(dest => dest.PositionName,
                    opt => opt.MapFrom(src => src.Employee.Position.Name))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Employee.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Employee.LastName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Employee.PhoneNumber))
                .ForMember(dest => dest.PersonalEmail, opt => opt.MapFrom(src => src.Employee.PersonalEmail))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.Employee.BirthDate))
                .ForMember(dest => dest.ProfileImagePath, opt => opt.MapFrom(src => src.Employee.ProfileImagePath))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Employee.Address))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Employee.City))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.Employee.District))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.Employee.PostalCode))
                .ForMember(dest => dest.HireDate, opt => opt.MapFrom(src => src.Employee.HireDate))
                .ForMember(dest => dest.RoleName,
                    opt => opt.Ignore());

            //employee
            CreateMap<Entities.HumanResources.Employee, DTOs.Employee.EmployeeListDto>()
                    .ForMember(dest => dest.DepartmentName,
                        opt => opt.MapFrom(src => src.Department.Name))

                    .ForMember(dest => dest.PositionName,
                        opt => opt.MapFrom(src => src.Position.Name))

                    .ForMember(dest => dest.FullName,
                        opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))

                    .ForMember(dest => dest.Id,
                        opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.HasUser,
                        opt => opt.MapFrom(src => src.SystemUser != null)); 

            CreateMap<EmployeeCreateDto, Employee>();

            CreateMap<EmployeeUpdateDto, Employee>();

            CreateMap<Employee, EmployeeUpdateDto>();

            CreateMap<Employee, EmployeeDetailDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position.Name));

            CreateMap<Employee, UpcomingBirthdayDto>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position.Name))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate!.Value))
                .ForMember(dest => dest.DaysLeft, opt => opt.Ignore());

            CreateMap<Employee, EmployeeLookupDto>()
                    .ForMember(dest => dest.FullName,
                        opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));



            //department
            CreateMap<Entities.Organization.Department, DTOs.Department.DepartmentListDto>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<DepartmentCreateDto, Department>();

            CreateMap<DepartmentUpdateDto, Department>();

            CreateMap<Department, DepartmentUpdateDto>();

            //position
            CreateMap<Entities.Organization.Position, DTOs.Position.PositionListDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<PositionCreateDto, Position>();

            CreateMap<PositionUpdateDto, Position>();

            CreateMap<Position, PositionUpdateDto>();

            //leavetype
            CreateMap<Entities.HumanResources.LeaveType, DTOs.LeaveType.LeaveTypeListDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            CreateMap<LeaveTypeCreateDto, LeaveType>();

            CreateMap<LeaveTypeUpdateDto, LeaveType>();

            CreateMap<LeaveType, LeaveTypeUpdateDto>();

            CreateMap<LeaveType, LeaveTypeLookupDto>();

            //leaverequest
            CreateMap<LeaveRequest, LeaveRequestListDto>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Employee.FirstName + " " + src.Employee.LastName))
                .ForMember(dest => dest.LeaveTypeName,opt => opt.MapFrom(src => src.LeaveType.Name))
                .ForMember(dest => dest.TotalDays,opt => opt.MapFrom(src => src.TotalDays));

            CreateMap<LeaveRequestCreateDto, LeaveRequest>();

            CreateMap<LeaveRequest, LeaveRequestUpdateDto>().ReverseMap()
                .ForMember(dest => dest.TotalDays, opt => opt.Ignore())
                .ForMember(dest => dest.ApprovedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ApprovedDate, opt => opt.Ignore());

            CreateMap<LeaveRequest, PendingLeaveDto>()
                .ForMember(dest => dest.EmployeeFullName,
                    opt => opt.MapFrom(src => src.Employee.FirstName + " " + src.Employee.LastName))
                .ForMember(dest => dest.LeaveTypeName,
                    opt => opt.MapFrom(src => src.LeaveType.Name));

            CreateMap<LeaveRequest, UpcomingLeaveDto>()
                .ForMember(dest => dest.EmployeeFullName,
                    opt => opt.MapFrom(src => src.Employee.FirstName + " " + src.Employee.LastName))
                .ForMember(dest => dest.LeaveTypeName,
                    opt => opt.MapFrom(src => src.LeaveType.Name))
                .ForMember(dest => dest.TotalDays,
                    opt => opt.MapFrom(src => (src.EndDate - src.StartDate).Days));

            //overtimerequest
            CreateMap<OvertimeRequestCreateDto, OvertimeRequest>()
                .ForMember(dest => dest.StartTime, opt => opt.Ignore())
                .ForMember(dest => dest.EndTime, opt => opt.Ignore())
                .ForMember(dest => dest.TotalHours, opt => opt.Ignore())
                .ForMember(dest => dest.EmployeeId, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.ApprovedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ApprovedDate, opt => opt.Ignore());

            CreateMap<OvertimeRequest, OvertimeRequestListDto>()
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => src.Employee.FirstName + " " + src.Employee.LastName));

            //announcement
            CreateMap<AnnouncementCreateDto, Announcement>();

            CreateMap<Announcement, AnnouncementUpdateDto>().ReverseMap()
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());

            CreateMap<Announcement, AnnouncementListDto>();

            CreateMap<Announcement, AnnouncementDashboardDto>();

            CreateMap<AuditLogCreateDto, AuditLog>();
            CreateMap<AuditLog, AuditLogListDto>();
            CreateMap<RequestLogCreateDto, RequestLog>();
            CreateMap<RequestLog, RequestLogListDto>();
            CreateMap<PayrollPeriod, DTOs.Payroll.PayrollPeriodListDto>()
                .ForMember(d => d.PayrollCount, o => o.MapFrom(s => s.EmployeePayrolls.Count));
            CreateMap<PayrollPeriod, DTOs.Payroll.PayrollPeriodDetailDto>()
                .ForMember(d => d.PayrollCount, o => o.MapFrom(s => s.EmployeePayrolls.Count))
                .ForMember(d => d.Payrolls, o => o.MapFrom(s => s.EmployeePayrolls));
            CreateMap<EmployeePayroll, DTOs.Payroll.EmployeePayrollListDto>()
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.Employee.FirstName + " " + s.Employee.LastName))
                .ForMember(d => d.DepartmentName, o => o.MapFrom(s => s.Employee.Department.Name))
                .ForMember(d => d.PositionName, o => o.MapFrom(s => s.Employee.Position.Name))
                .ForMember(d => d.ProfileImagePath, o => o.MapFrom(s => s.Employee.ProfileImagePath))
                .ForMember(d => d.PeriodName, o => o.MapFrom(s => s.PayrollPeriod.Name));
            CreateMap<EmployeePayroll, DTOs.Payroll.EmployeePayrollDetailDto>().IncludeBase<EmployeePayroll, DTOs.Payroll.EmployeePayrollListDto>();
            CreateMap<EmployeePayroll, DTOs.Payroll.MyPayrollListDto>().IncludeBase<EmployeePayroll, DTOs.Payroll.EmployeePayrollListDto>();
        }
    }
}
