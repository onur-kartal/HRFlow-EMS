using AutoMapper;
using HRFlow.Business.DTOs.Logging;
using HRFlow.Business.Interfaces;
using HRFlow.Common.Constants;
using HRFlow.Data.Interfaces;
using HRFlow.Entities.Logging;

namespace HRFlow.Business.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public AuditLogService(IAuditLogRepository auditLogRepository, ICurrentUserService currentUser, IMapper mapper)
        {
            _auditLogRepository = auditLogRepository;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task LogAsync(AuditLogCreateDto dto)
        {
            var auditLog = _mapper.Map<AuditLog>(dto);
            auditLog.CreatedDate = DateTime.UtcNow;
            auditLog.UserId ??= _currentUser.UserId;
            auditLog.EmployeeId ??= _currentUser.EmployeeId > 0 ? _currentUser.EmployeeId : null;
            auditLog.UserName ??= _currentUser.UserName;
            auditLog.Role ??= GetCurrentRole();
            auditLog.IpAddress = _currentUser.IpAddress;

            await _auditLogRepository.AddAsync(auditLog);
        }

        public async Task<List<AuditLogListDto>> GetListAsync()
        {
            EnsureAdmin();
            return _mapper.Map<List<AuditLogListDto>>(await _auditLogRepository.GetListAsync());
        }

        private string? GetCurrentRole()
        {
            if (_currentUser.IsInRole(Roles.Admin)) return Roles.Admin;
            if (_currentUser.IsInRole(Roles.HR)) return Roles.HR;
            if (_currentUser.IsInRole(Roles.Manager)) return Roles.Manager;
            if (_currentUser.IsInRole(Roles.Employee)) return Roles.Employee;
            return null;
        }

        private void EnsureAdmin()
        {
            if (!_currentUser.IsInRole(Roles.Admin))
                throw new UnauthorizedAccessException("Bu işlem için yetkiniz bulunmuyor.");
        }
    }
}
