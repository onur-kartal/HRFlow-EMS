using AutoMapper;
using HRFlow.Business.DTOs.Department;
using HRFlow.Business.DTOs.Employee;
using HRFlow.Business.Interfaces;
using HRFlow.Common.Interfaces;
using HRFlow.Data.Interfaces;
using HRFlow.Entities.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.Services
{
    public class DepartmentService : GenericService<Department>, IDepartmentService
    {
        private readonly IMapper _mapper;
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(
            IGenericRepository<Department> repository,
            IDepartmentRepository departmentRepository,
            IMapper mapper)
        : base(repository)
        {
            _mapper = mapper;
            _departmentRepository = departmentRepository;
        }

        public async Task CreateAsync(DepartmentCreateDto dto)
        {
            var department=_mapper.Map<Department>(dto);
            await _repository.AddAsync(department);
            await _repository.SaveChangesAsync();
        }

        public async Task<DepartmentUpdateDto?> GetByIdForUpdateAsync(int id)
        {
            var department = await _repository.GetByIdAsync(id);

            if (department == null)
                return null;

            return _mapper.Map<DepartmentUpdateDto>(department);
        }

        public async Task<List<DepartmentListDto>> GetDepartmentListAsync()
        {
            var departments= await _departmentRepository.GetDepartmentListAsync();
            return _mapper.Map<List<DepartmentListDto>>(departments);
        }

        public async Task UpdateAsync(DepartmentUpdateDto dto)
        {
            var department = await _repository.GetByIdAsync(dto.Id);

            if (department == null)
                return;

            _mapper.Map(dto, department);

            _repository.Update(department);

            await _repository.SaveChangesAsync();
        }
    }
}
