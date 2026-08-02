using AutoMapper;
using HRFlow.Business.DTOs.LeaveRequest;
using HRFlow.Business.DTOs.Position;
using HRFlow.Business.Interfaces;
using HRFlow.Common.Enums;
using HRFlow.Common.Interfaces;
using HRFlow.Data.Interfaces;
using HRFlow.Entities.HumanResources;
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
    public class LeaveRequestService : GenericService<LeaveRequest>, ILeaveRequestService
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;
        private readonly IAuditLogService _auditLogService;
        private readonly INotificationService _notificationService;

        public LeaveRequestService(
            IGenericRepository<LeaveRequest> repository,
            ILeaveRequestRepository leaveRequestRepository,
            ICurrentUserService currentUser,
            IMapper mapper, IAuditLogService auditLogService, INotificationService notificationService)
            : base(repository)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _currentUser = currentUser;
            _mapper = mapper;
            _auditLogService = auditLogService;
            _notificationService = notificationService;
        }

        public async Task ApproveAsync(LeaveRequestApproveDto dto)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(dto.Id);

            if (leaveRequest == null)
                throw new Exception("İzin talebi bulunamadı.");

            EnsureManagerHrOrAdmin();

            if (!IsAdmin() && leaveRequest.Status != LeaveStatus.Pending)
                throw new Exception("Sadece bekleyen izin talepleri onaylanabilir.");

            var previousStatus = leaveRequest.Status;
            leaveRequest.Status = LeaveStatus.Approved;
            leaveRequest.ApprovedBy = _currentUser.UserId;
            leaveRequest.ApprovedDate = DateTime.UtcNow;

            _repository.Update(leaveRequest);
            await _repository.SaveChangesAsync();
            await _auditLogService.LogAsync(new AuditLogCreateDto { Module = AuditModule.LeaveRequest, Action = AuditAction.Approved, EntityId = leaveRequest.Id, Description = "İzin talebi onaylandı." });
            await NotifyStatusChangeAsync(leaveRequest, previousStatus);
        }

        public async Task CreateAsync(LeaveRequestCreateDto dto)
        {
            if (dto.EndDate < dto.StartDate)
            {
                throw new Exception("Bitiş tarihi, başlangıç tarihinden önce olamaz.");
            }
            if (_currentUser.EmployeeId <= 0)
                throw new Exception("Giriş yapan kullanıcının çalışan kaydı bulunamadı.");

            if (await _leaveRequestRepository.HasDateConflictAsync(_currentUser.EmployeeId, dto.StartDate, dto.EndDate))
            {
                throw new Exception("Seçilen tarih aralığında bu personele ait başka bir izin talebi bulunmaktadır.");
            }
            var leaveRequest = _mapper.Map<LeaveRequest>(dto);
            leaveRequest.EmployeeId = _currentUser.EmployeeId;
            leaveRequest.Status = LeaveStatus.Pending;
            leaveRequest.TotalDays = (dto.EndDate.Date - dto.StartDate.Date).Days + 1;

            await _repository.AddAsync(leaveRequest);
            await _repository.SaveChangesAsync();
            await _auditLogService.LogAsync(new AuditLogCreateDto { Module = AuditModule.LeaveRequest, Action = AuditAction.Created, EntityId = leaveRequest.Id, Description = "İzin talebi oluşturuldu." });
        }

        public async Task CancelAsync(int id)
        {
            var leaveRequest = await _repository.GetByIdAsync(id);

            if (leaveRequest == null)
                throw new Exception("İzin talebi bulunamadı.");

            if (!IsAdmin() && leaveRequest.Status != LeaveStatus.Pending)
                throw new Exception("Sadece bekleyen izin talepleri iptal edilebilir.");

            if (IsEmployee() && leaveRequest.EmployeeId != _currentUser.EmployeeId)
                throw new UnauthorizedAccessException("Sadece kendi izin talebinizi iptal edebilirsiniz.");

            if (!IsEmployee() && !IsManagerOrHr() && !IsAdmin())
                throw new UnauthorizedAccessException("Bu işlem için yetkiniz bulunmuyor.");

            var previousStatus = leaveRequest.Status;
            leaveRequest.Status = LeaveStatus.Cancelled;

            _repository.Update(leaveRequest);

            await _repository.SaveChangesAsync();
            await NotifyStatusChangeAsync(leaveRequest, previousStatus);
        }

        public async Task<LeaveRequestUpdateDto?> GetByIdForUpdateAsync(int id)
        {
            var leaveRequest = await GetByIdAsync(id);

            if (leaveRequest == null)
                return null;

            EnsureAdmin();

            return _mapper.Map<LeaveRequestUpdateDto>(leaveRequest);
        }

        public async Task<int> GetLeaveRequestCountAsync()
        {
            return await _leaveRequestRepository.GetLeaveRequestCountAsync();
        }

        public async Task<List<LeaveRequestListDto>> GetLeaveRequestListAsync()
        {
            List<LeaveRequest> leaveRequest;

            if (IsAdmin())
            {
                leaveRequest = await _leaveRequestRepository.GetLeaveRequestListAsync();
            }
            else if (IsManagerOrHr())
            {
                leaveRequest = await _leaveRequestRepository.GetPendingLeaveRequestListAsync();
            }
            else
            {
                leaveRequest = await _leaveRequestRepository.GetLeaveRequestsByEmployeeIdAsync(_currentUser.EmployeeId);
            }

            return _mapper.Map<List<LeaveRequestListDto>>(leaveRequest);
        }

        public async Task<List<PendingLeaveDto>> GetPendingLeaveRequestsAsync(int count)
        {
            var leaveRequests = await _leaveRequestRepository.GetPendingLeaveRequestsAsync(count);

            return _mapper.Map<List<PendingLeaveDto>>(leaveRequests);
        }

        public async Task<int> GetTodayOnLeaveCountAsync()
        {
            return await _leaveRequestRepository.GetTodayOnLeaveCountAsync();
        }

        public async Task RejectAsync(int id)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);

            if (leaveRequest == null)
                throw new Exception("İzin talebi bulunamadı.");

            EnsureManagerHrOrAdmin();

            if (!IsAdmin() && leaveRequest.Status != LeaveStatus.Pending)
                throw new Exception("Sadece bekleyen izin talepleri reddedilebilir.");

            var previousStatus = leaveRequest.Status;
            leaveRequest.Status = LeaveStatus.Rejected;
            leaveRequest.ApprovedBy = null;
            leaveRequest.ApprovedDate = null;

            _repository.Update(leaveRequest);
            await _repository.SaveChangesAsync();
            await NotifyStatusChangeAsync(leaveRequest, previousStatus);
            await _auditLogService.LogAsync(new AuditLogCreateDto { Module = AuditModule.LeaveRequest, Action = AuditAction.Rejected, EntityId = leaveRequest.Id, Description = "İzin talebi reddedildi." });
            await _auditLogService.LogAsync(new AuditLogCreateDto { Module = AuditModule.LeaveRequest, Action = AuditAction.Cancelled, EntityId = leaveRequest.Id, Description = "İzin talebi iptal edildi." });
        }

        public async Task UpdateAsync(LeaveRequestUpdateDto dto)
        {
            EnsureAdmin();

            if (dto.EndDate < dto.StartDate)
                throw new Exception("Bitiş tarihi başlangıç tarihinden önce olamaz.");
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

            var previousStatus = leaveRequest.Status;
            _mapper.Map(dto, leaveRequest);
            leaveRequest.TotalDays = (dto.EndDate.Date - dto.StartDate.Date).Days + 1;

            if (dto.Status == LeaveStatus.Approved)
            {
                leaveRequest.ApprovedBy = _currentUser.UserId;
                leaveRequest.ApprovedDate = DateTime.UtcNow;
            }
            else
            {
                leaveRequest.ApprovedBy = null;
                leaveRequest.ApprovedDate = null;
            }

            _repository.Update(leaveRequest);

            await _repository.SaveChangesAsync();
            await _auditLogService.LogAsync(new AuditLogCreateDto { Module = AuditModule.LeaveRequest, Action = AuditAction.Updated, EntityId = leaveRequest.Id, Description = "İzin talebi güncellendi." });
            await NotifyStatusChangeAsync(leaveRequest, previousStatus);
        }
        public async Task<List<UpcomingLeaveDto>> GetUpcomingLeaveRequestsAsync(int count)
        {
            var leaveRequests =
                await _leaveRequestRepository.GetUpcomingLeaveRequestsAsync(count);

            return _mapper.Map<List<UpcomingLeaveDto>>(leaveRequests);
        }

        private bool IsAdmin()
        {
            return _currentUser.IsInRole(HRFlow.Common.Constants.Roles.Admin);
        }

        private bool IsManagerOrHr()
        {
            return _currentUser.IsInRole(HRFlow.Common.Constants.Roles.Manager) ||
                   _currentUser.IsInRole(HRFlow.Common.Constants.Roles.HR);
        }

        private bool IsEmployee()
        {
            return _currentUser.IsInRole(HRFlow.Common.Constants.Roles.Employee);
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

        private Task NotifyStatusChangeAsync(LeaveRequest leaveRequest, LeaveStatus previousStatus)
        {
            if (previousStatus == leaveRequest.Status)
            {
                return Task.CompletedTask;
            }

            return leaveRequest.Status switch
            {
                LeaveStatus.Approved => _notificationService.CreateForEmployeeAsync(
                    leaveRequest.EmployeeId,
                    "İzin Talebi",
                    "İzin talebiniz onaylandı.",
                    NotificationType.Leave,
                    "/LeaveRequest",
                    AuditModule.LeaveRequest,
                    leaveRequest.Id,
                    NotificationEventType.LeaveApproved),
                LeaveStatus.Rejected => _notificationService.CreateForEmployeeAsync(
                    leaveRequest.EmployeeId,
                    "İzin Talebi",
                    "İzin talebiniz reddedildi.",
                    NotificationType.Leave,
                    "/LeaveRequest",
                    AuditModule.LeaveRequest,
                    leaveRequest.Id,
                    NotificationEventType.LeaveRejected),
                LeaveStatus.Cancelled => _notificationService.CreateForEmployeeAsync(
                    leaveRequest.EmployeeId,
                    "İzin Talebi",
                    "İzin talebiniz iptal edildi.",
                    NotificationType.Leave,
                    "/LeaveRequest",
                    AuditModule.LeaveRequest,
                    leaveRequest.Id,
                    NotificationEventType.LeaveCancelled),
                _ => Task.CompletedTask
            };
        }
    }
}
