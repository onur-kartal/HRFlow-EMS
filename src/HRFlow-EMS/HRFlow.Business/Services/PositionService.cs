using AutoMapper;
using HRFlow.Business.DTOs.Position;
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
    public class PositionService : GenericService<Position>, IPositionService
    {
        private readonly IMapper _mapper;
        private readonly IPositionRepository _positionRepository;
        private readonly IAuditLogService _auditLogService;

        public PositionService(IGenericRepository<Position> repository,IPositionRepository positionRepository,IMapper mapper, IAuditLogService auditLogService)
       : base(repository)
        {
            _mapper = mapper;
            _positionRepository = positionRepository;
            _auditLogService = auditLogService;
        }

        public async Task CreateAsync(PositionCreateDto dto)
        {
            var position=_mapper.Map<Position>(dto);
            await _repository.AddAsync(position);
            await _repository.SaveChangesAsync();
            await _auditLogService.LogAsync(new AuditLogCreateDto { Module=AuditModule.Position,Action=AuditAction.Created,EntityId=position.Id,Description=$"{position.Name} pozisyonu oluşturuldu." });
        }

        public async Task<PositionUpdateDto?> GetByIdForUpdateAsync(int id)
        {
            var position=await GetByIdAsync(id);
            if (position == null) 
                return null;
            return _mapper.Map<PositionUpdateDto?>(position);
        }

        public async Task<List<PositionListDto>> GetPositionListAsync()
        {
            var position=await _positionRepository.GetPositionListAsync();
            return _mapper.Map<List<PositionListDto>>(position);
        }

        public async Task UpdateAsync(PositionUpdateDto dto)
        {
            var position = await _repository.GetByIdAsync(dto.Id);

            if (position == null)
                return;

            _mapper.Map(dto, position);

            _repository.Update(position);

            await _repository.SaveChangesAsync();
            await _auditLogService.LogAsync(new AuditLogCreateDto { Module=AuditModule.Position,Action=AuditAction.Updated,EntityId=position.Id,Description=$"{position.Name} pozisyonu güncellendi." });
        }
        public override async Task DeleteAsync(int id){var item=await _repository.GetByIdAsync(id);if(item==null)return;_repository.Delete(item);await _repository.SaveChangesAsync();await _auditLogService.LogAsync(new AuditLogCreateDto { Module=AuditModule.Position,Action=AuditAction.Deleted,EntityId=id,Description=$"{item.Name} pozisyonu silindi."});}
    }
}
