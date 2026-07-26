using AutoMapper;
using HRFlow.Business.DTOs.OvertimeRequest;
using HRFlow.Business.Interfaces;
using HRFlow.Common.Constants;
using HRFlow.Common.Enums;
using HRFlow.Common.Interfaces;
using HRFlow.Data.Interfaces;
using HRFlow.Entities.HumanResources;

namespace HRFlow.Business.Services
{
    public class OvertimeRequestService : GenericService<OvertimeRequest>, IOvertimeRequestService
    {
        private readonly IOvertimeRequestRepository _overtimeRequestRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public OvertimeRequestService(
            IGenericRepository<OvertimeRequest> repository,
            IOvertimeRequestRepository overtimeRequestRepository,
            ICurrentUserService currentUser,
            IMapper mapper)
            : base(repository)
        {
            _overtimeRequestRepository = overtimeRequestRepository;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task CreateAsync(OvertimeRequestCreateDto dto)
        {
            if (_currentUser.EmployeeId <= 0)
                throw new Exception("Giriş yapan kullanıcının çalışan kaydı bulunamadı.");

            if (!dto.StartTime.HasValue || !dto.EndTime.HasValue || dto.EndTime <= dto.StartTime)
                throw new Exception("Bitiş saati başlangıç saatinden sonra olmalıdır.");

            if (await _overtimeRequestRepository.HasTimeConflictAsync(
                _currentUser.EmployeeId,
                dto.WorkDate,
                dto.StartTime.Value,
                dto.EndTime.Value))
            {
                throw new Exception("Seçilen zaman aralığında bu personele ait başka bir fazla mesai talebi bulunmaktadır.");
            }

            var overtimeRequest = _mapper.Map<OvertimeRequest>(dto);
            overtimeRequest.EmployeeId = _currentUser.EmployeeId;
            overtimeRequest.StartTime = dto.StartTime.Value;
            overtimeRequest.EndTime = dto.EndTime.Value;
            overtimeRequest.TotalHours = Convert.ToDecimal((dto.EndTime.Value - dto.StartTime.Value).TotalHours);
            overtimeRequest.Status = OvertimeStatus.Pending;

            await _repository.AddAsync(overtimeRequest);
            await _repository.SaveChangesAsync();
        }

        public async Task<List<OvertimeRequestListDto>> GetMyRequestsAsync()
        {
            if (_currentUser.EmployeeId <= 0)
                throw new Exception("Giriş yapan kullanıcının çalışan kaydı bulunamadı.");

            var overtimeRequests = await _overtimeRequestRepository
                .GetOvertimeRequestsByEmployeeIdAsync(_currentUser.EmployeeId);

            return _mapper.Map<List<OvertimeRequestListDto>>(overtimeRequests);
        }

        public async Task<List<OvertimeRequestListDto>> GetPendingRequestsAsync()
        {
            EnsureManagerHrOrAdmin();

            var overtimeRequests = await _overtimeRequestRepository.GetPendingOvertimeRequestsAsync();

            return _mapper.Map<List<OvertimeRequestListDto>>(overtimeRequests);
        }

        public async Task<List<OvertimeRequestListDto>> GetAllRequestsAsync()
        {
            EnsureAdmin();

            var overtimeRequests = await _overtimeRequestRepository.GetOvertimeRequestListAsync();

            return _mapper.Map<List<OvertimeRequestListDto>>(overtimeRequests);
        }

        public Task ApproveAsync(int id)
        {
            return UpdatePendingRequestStatusAsync(id, OvertimeStatus.Approved);
        }

        public Task RejectAsync(int id)
        {
            return UpdatePendingRequestStatusAsync(id, OvertimeStatus.Rejected);
        }

        public async Task CancelAsync(int id)
        {
            var overtimeRequest = await _repository.GetByIdAsync(id);

            if (overtimeRequest == null)
                throw new Exception("Fazla mesai talebi bulunamadı.");

            if (IsAdmin())
            {
                overtimeRequest.Status = OvertimeStatus.Cancelled;
                ClearApprovalInformation(overtimeRequest);
            }
            else
            {
                if (overtimeRequest.Status != OvertimeStatus.Pending)
                    throw new Exception("Sadece bekleyen fazla mesai talepleri iptal edilebilir.");

                if (IsEmployee() && overtimeRequest.EmployeeId != _currentUser.EmployeeId)
                    throw new UnauthorizedAccessException("Sadece kendi fazla mesai talebinizi iptal edebilirsiniz.");

                EnsureEmployeeManagerOrHr();
                overtimeRequest.Status = OvertimeStatus.Cancelled;
                ClearApprovalInformation(overtimeRequest);
            }

            _repository.Update(overtimeRequest);
            await _repository.SaveChangesAsync();
        }

        public async Task ChangeStatusAsync(OvertimeRequestStatusChangeDto dto)
        {
            EnsureAdmin();

            var overtimeRequest = await _repository.GetByIdAsync(dto.Id);

            if (overtimeRequest == null)
                throw new Exception("Fazla mesai talebi bulunamadı.");

            overtimeRequest.Status = dto.Status;

            if (dto.Status == OvertimeStatus.Approved)
            {
                overtimeRequest.ApprovedBy = _currentUser.UserId;
                overtimeRequest.ApprovedDate = DateTime.UtcNow;
            }
            else
            {
                ClearApprovalInformation(overtimeRequest);
            }

            _repository.Update(overtimeRequest);
            await _repository.SaveChangesAsync();
        }

        private async Task UpdatePendingRequestStatusAsync(int id, OvertimeStatus status)
        {
            EnsureManagerHrOrAdmin();

            var overtimeRequest = await _repository.GetByIdAsync(id);

            if (overtimeRequest == null)
                throw new Exception("Fazla mesai talebi bulunamadı.");

            if (!IsAdmin() && overtimeRequest.Status != OvertimeStatus.Pending)
                throw new Exception("Sadece bekleyen fazla mesai talepleri üzerinde işlem yapılabilir.");

            overtimeRequest.Status = status;

            if (status == OvertimeStatus.Approved)
            {
                overtimeRequest.ApprovedBy = _currentUser.UserId;
                overtimeRequest.ApprovedDate = DateTime.UtcNow;
            }
            else
            {
                ClearApprovalInformation(overtimeRequest);
            }

            _repository.Update(overtimeRequest);
            await _repository.SaveChangesAsync();
        }

        private void ClearApprovalInformation(OvertimeRequest overtimeRequest)
        {
            overtimeRequest.ApprovedBy = null;
            overtimeRequest.ApprovedDate = null;
        }

        private bool IsAdmin()
        {
            return _currentUser.IsInRole(Roles.Admin);
        }

        private bool IsEmployee()
        {
            return _currentUser.IsInRole(Roles.Employee);
        }

        private bool IsManagerOrHr()
        {
            return _currentUser.IsInRole(Roles.Manager) || _currentUser.IsInRole(Roles.HR);
        }

        private void EnsureAdmin()
        {
            if (!IsAdmin())
                throw new UnauthorizedAccessException("Bu işlem için yetkiniz bulunmuyor.");
        }

        private void EnsureManagerHrOrAdmin()
        {
            if (!IsAdmin() && !IsManagerOrHr())
                throw new UnauthorizedAccessException("Bu işlem için yetkiniz bulunmuyor.");
        }

        private void EnsureEmployeeManagerOrHr()
        {
            if (!IsEmployee() && !IsManagerOrHr())
                throw new UnauthorizedAccessException("Bu işlem için yetkiniz bulunmuyor.");
        }
    }
}
