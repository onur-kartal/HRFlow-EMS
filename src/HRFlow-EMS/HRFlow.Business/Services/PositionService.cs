using AutoMapper;
using HRFlow.Business.DTOs.Position;
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
    public class PositionService : GenericService<Position>, IPositionService
    {
        private readonly IMapper _mapper;
        private readonly IPositionRepository _positionRepository;

        public PositionService(IGenericRepository<Position> repository,IPositionRepository positionRepository,IMapper mapper)
       : base(repository)
        {
            _mapper = mapper;
            _positionRepository = positionRepository;
        }

        public async Task CreateAsync(PositionCreateDto dto)
        {
            var position=_mapper.Map<Position>(dto);
            await _repository.AddAsync(position);
            await _repository.SaveChangesAsync();
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
        }
    }
}
