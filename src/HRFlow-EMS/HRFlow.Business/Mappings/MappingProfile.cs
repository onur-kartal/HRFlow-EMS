using AutoMapper;
using HRFlow.Business.DTOs.Department;
using HRFlow.Business.DTOs.Employee;
using HRFlow.Business.DTOs.LeaveRequest;
using HRFlow.Business.DTOs.LeaveType;
using HRFlow.Business.DTOs.Position;
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
            //employee
            CreateMap<Entities.HumanResources.Employee, DTOs.Employee.EmployeeListDto>()
                    .ForMember(dest => dest.DepartmentName,
                        opt => opt.MapFrom(src => src.Department.Name))

                    .ForMember(dest => dest.PositionName,
                        opt => opt.MapFrom(src => src.Position.Name))

                    .ForMember(dest => dest.FullName,
                        opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))

                    .ForMember(dest => dest.Id,
                        opt => opt.MapFrom(src => src.Id));

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
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Employee.FirstName + " " + src.Employee.LastName))
                .ForMember(dest => dest.LeaveTypeName,opt => opt.MapFrom(src => src.LeaveType.Name))
                .ForMember(dest => dest.TotalDays,opt => opt.MapFrom(src => (src.EndDate - src.StartDate).Days + 1));

            CreateMap<LeaveRequestCreateDto, LeaveRequest>();

            CreateMap<LeaveRequest, LeaveRequestUpdateDto>().ReverseMap();
        }
    }
}
