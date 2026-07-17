using AutoMapper;
using HRFlow.Business.DTOs.Department;
using HRFlow.Business.DTOs.LeaveType;
using HRFlow.Business.Interfaces;
using HRFlow.Common.Interfaces;
using HRFlow.Data.Interfaces;
using HRFlow.Data.Repositories;
using HRFlow.Entities.HumanResources;
using HRFlow.Entities.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.Services
{
    public class LeaveTypeService : GenericService<LeaveType>, ILeaveTypeService
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IMapper _mapper;

        public LeaveTypeService(
            IGenericRepository<LeaveType> repository,
            ILeaveTypeRepository leaveTypeRepository,
             IMapper mapper) : base(repository)
        {
            _leaveTypeRepository = leaveTypeRepository;
            _mapper = mapper;
        }

        public async Task CreateAsync(LeaveTypeCreateDto dto)
        {
            var leaveType = _mapper.Map<LeaveType>(dto);
            await _repository.AddAsync(leaveType);
            await _repository.SaveChangesAsync();
        }

        public async Task<LeaveTypeUpdateDto?> GetByIdForUpdateAsync(int id)
        {
            var leaveType = await _repository.GetByIdAsync(id);

            if (leaveType == null)
                return null;

            return _mapper.Map<LeaveTypeUpdateDto>(leaveType);
        }

        public async Task<int> GetLeaveTypeCountAsync()
        {
            return await _leaveTypeRepository.GetLeaveTypeCountAsync();
        }

        public async Task<List<LeaveTypeListDto>> GetleaveTypeListAsync()
        {
            var leaveType = await _leaveTypeRepository.GetLeaveTypeListAsync();
            return _mapper.Map<List<LeaveTypeListDto>>(leaveType);
        }

        public async Task UpdateAsync(LeaveTypeUpdateDto dto)
        {
            var leaveType = await _repository.GetByIdAsync(dto.Id);

            if (leaveType == null)
                return;

            _mapper.Map(dto, leaveType);

            _repository.Update(leaveType);

            await _repository.SaveChangesAsync();
        }

        public async Task<List<LeaveTypeLookupDto>> GetLeaveTypeLookupAsync()
        {
            var leaveTypes = await _leaveTypeRepository.GetLeaveTypeListAsync();

            return _mapper.Map<List<LeaveTypeLookupDto>>(leaveTypes);
        }
    }
}
