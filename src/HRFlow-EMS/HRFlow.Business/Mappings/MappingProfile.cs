using AutoMapper;
using HRFlow.Business.DTOs.Department;
using HRFlow.Business.DTOs.Employee;
using HRFlow.Business.DTOs.LeaveRequest;
using HRFlow.Business.DTOs.LeaveType;
using HRFlow.Business.DTOs.Position;
using HRFlow.Business.DTOs.OvertimeRequest;
using HRFlow.Business.DTOs.Announcement;
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
        }
    }
}
