using AutoMapper;
using HRFlow.Business.DTOs.Employee;
using HRFlow.Entities.HumanResources;
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
            CreateMap<Entities.HumanResources.Employee, DTOs.Employee.EmployeeListDto>()
                    .ForMember(dest => dest.DepartmentName,
                        opt => opt.MapFrom(src => src.Department.Name))

                    .ForMember(dest => dest.PositionName,
                        opt => opt.MapFrom(src => src.Position.Name))

                    .ForMember(dest => dest.FullName,
                        opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));

            CreateMap<EmployeeCreateDto, Employee>();

            CreateMap<EmployeeUpdateDto, Employee>();

            CreateMap<Employee, EmployeeUpdateDto>();

            CreateMap<Employee, EmployeeUpdateDto>();

            CreateMap<EmployeeUpdateDto, Employee>();
        }
    }
}
