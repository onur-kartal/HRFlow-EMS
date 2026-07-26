using AutoMapper;
using HRFlow.Business.DTOs.Logging;
using HRFlow.Business.Interfaces;
using HRFlow.Common.Constants;
using HRFlow.Data.Interfaces;
using HRFlow.Entities.Logging;

namespace HRFlow.Business.Services
{
    public class RequestLogService : IRequestLogService
    {
        private readonly IRequestLogRepository _requestLogRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public RequestLogService(IRequestLogRepository requestLogRepository, ICurrentUserService currentUser, IMapper mapper)
        {
            _requestLogRepository = requestLogRepository;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task LogAsync(RequestLogCreateDto dto)
        {
            var requestLog = _mapper.Map<RequestLog>(dto);
            requestLog.CreatedDate = DateTime.UtcNow;
            await _requestLogRepository.AddAsync(requestLog);
        }

        public async Task<List<RequestLogListDto>> GetListAsync()
        {
            if (!_currentUser.IsInRole(Roles.Admin))
                throw new UnauthorizedAccessException("Bu işlem için yetkiniz bulunmuyor.");

            return _mapper.Map<List<RequestLogListDto>>(await _requestLogRepository.GetListAsync());
        }
    }
}
