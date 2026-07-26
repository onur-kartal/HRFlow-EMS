using AutoMapper;
using HRFlow.Business.DTOs.Department;
using HRFlow.Business.DTOs.Employee;
using HRFlow.Business.Interfaces;
using HRFlow.Common.Interfaces;
using HRFlow.Data.Interfaces;
using HRFlow.Entities.Organization;
using HRFlow.Business.DTOs.Logging;
using HRFlow.Entities.Enums;
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
        private readonly IAuditLogService _auditLogService;

        public DepartmentService(
            IGenericRepository<Department> repository,
            IDepartmentRepository departmentRepository,
            IMapper mapper, IAuditLogService auditLogService)
        : base(repository)
        {
            _mapper = mapper;
            _departmentRepository = departmentRepository;
            _auditLogService = auditLogService;
        }

        public async Task CreateAsync(DepartmentCreateDto dto)
        {
            var department=_mapper.Map<Department>(dto);
            await _repository.AddAsync(department);
            await _repository.SaveChangesAsync();
            await _auditLogService.LogAsync(new AuditLogCreateDto { Module=AuditModule.Department, Action=AuditAction.Created, EntityId=department.Id, Description=$"{department.Name} departmanı oluşturuldu." });
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
            await _auditLogService.LogAsync(new AuditLogCreateDto { Module=AuditModule.Department, Action=AuditAction.Updated, EntityId=department.Id, Description=$"{department.Name} departmanı güncellendi." });
        }
        public override async Task DeleteAsync(int id){ var item=await _repository.GetByIdAsync(id); if(item==null)return; _repository.Delete(item); await _repository.SaveChangesAsync(); await _auditLogService.LogAsync(new AuditLogCreateDto { Module=AuditModule.Department,Action=AuditAction.Deleted,EntityId=id,Description=$"{item.Name} departmanı silindi."}); }
    }
}
