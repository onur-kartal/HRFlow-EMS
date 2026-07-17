using AutoMapper;
using HRFlow.Business.DTOs.LeaveRequest;
using HRFlow.Business.DTOs.Position;
using HRFlow.Business.Interfaces;
using HRFlow.Common.Enums;
using HRFlow.Common.Interfaces;
using HRFlow.Data.Interfaces;
using HRFlow.Entities.HumanResources;
using HRFlow.Entities.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.Services
{
    public class LeaveRequestService : GenericService<LeaveRequest>, ILeaveRequestService
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IMapper _mapper;

        public LeaveRequestService(
            IGenericRepository<LeaveRequest> repository,
            ILeaveRequestRepository leaveRequestRepository,
            IMapper mapper)
            : base(repository)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _mapper = mapper;
        }

        public async Task ApproveAsync(int id)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);

            if (leaveRequest == null)
                throw new Exception("İzin talebi bulunamadı.");

            if (leaveRequest.Status == LeaveStatus.Approved)
                throw new Exception("Bu izin talebi zaten onaylanmış.");

            var dto = _mapper.Map<LeaveRequestUpdateDto>(leaveRequest);

            dto.Status = LeaveStatus.Approved;

            await UpdateAsync(dto);
        }

        public async Task CreateAsync(LeaveRequestCreateDto dto)
        {
            // Tarih kontrolü
            if (dto.EndDate < dto.StartDate)
            {
                throw new Exception("Bitiş tarihi, başlangıç tarihinden önce olamaz.");
            }
            if (await _leaveRequestRepository.HasDateConflictAsync(dto.EmployeeId, dto.StartDate, dto.EndDate))
            {
                throw new Exception("Seçilen tarih aralığında bu personele ait başka bir izin talebi bulunmaktadır.");
            }
            var leaveRequest = _mapper.Map<LeaveRequest>(dto);
            await _repository.AddAsync(leaveRequest);
            await _repository.SaveChangesAsync();
        }

        public async Task<LeaveRequestUpdateDto?> GetByIdForUpdateAsync(int id)
        {
            var leaveRequest = await GetByIdAsync(id);
            if (leaveRequest == null)
                return null;
            return _mapper.Map<LeaveRequestUpdateDto?>(leaveRequest);
        }

        public async Task<int> GetLeaveRequestCountAsync()
        {
            return await _leaveRequestRepository.GetLeaveRequestCountAsync();
        }

        public async Task<List<LeaveRequestListDto>> GetLeaveRequestListAsync()
        {
            var leaveRequest = await _leaveRequestRepository.GetLeaveRequestListAsync();

            return _mapper.Map<List<LeaveRequestListDto>>(leaveRequest);
        }

        public async Task RejectAsync(int id)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);

            if (leaveRequest == null)
                throw new Exception("İzin talebi bulunamadı.");

            if (leaveRequest.Status == LeaveStatus.Rejected)
                throw new Exception("Bu izin talebi zaten reddedilmiş.");

            var dto = _mapper.Map<LeaveRequestUpdateDto>(leaveRequest);

            dto.Status = LeaveStatus.Rejected;

            await UpdateAsync(dto);
        }

        public async Task UpdateAsync(LeaveRequestUpdateDto dto)
        {
           
            if (dto.EndDate < dto.StartDate)
                throw new Exception("Bitiş tarihi başlangıç tarihinden önce olamaz.");
            // 2. Çakışan izin kontrolü
            if (await _leaveRequestRepository.HasDateConflictAsync(
                dto.EmployeeId,
                dto.StartDate,
                dto.EndDate,
                dto.Id))
            {
                throw new Exception("Seçilen tarih aralığında bu personele ait başka bir izin talebi bulunmaktadır.");
            }
            var leaveRequest = await _repository.GetByIdAsync(dto.Id);

            if (leaveRequest == null)
                return;

            if(leaveRequest.IsDeleted)
                throw new Exception("Sadece bekleyen izin talepleri güncellenebilir.");

            if (leaveRequest.Status != LeaveStatus.Pending)
            {
                throw new Exception("Sadece bekleyen izin talepleri güncellenebilir.");
            }

            _mapper.Map(dto, leaveRequest);

            _repository.Update(leaveRequest);

            await _repository.SaveChangesAsync();
        }
    }
}
