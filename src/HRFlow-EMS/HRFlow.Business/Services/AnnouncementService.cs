using AutoMapper;
using HRFlow.Business.DTOs.Announcement;
using HRFlow.Business.Interfaces;
using HRFlow.Common.Constants;
using HRFlow.Common.Interfaces;
using HRFlow.Data.Interfaces;
using HRFlow.Entities.HumanResources;

namespace HRFlow.Business.Services
{
    public class AnnouncementService : GenericService<Announcement>, IAnnouncementService
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public AnnouncementService(
            IGenericRepository<Announcement> repository,
            IAnnouncementRepository announcementRepository,
            ICurrentUserService currentUser,
            IMapper mapper)
            : base(repository)
        {
            _announcementRepository = announcementRepository;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<List<AnnouncementListDto>> GetAnnouncementListAsync()
        {
            EnsureAnnouncementManager();

            var announcements = await _announcementRepository.GetAnnouncementListAsync();

            return _mapper.Map<List<AnnouncementListDto>>(announcements);
        }

        public async Task CreateAsync(AnnouncementCreateDto dto)
        {
            EnsureAnnouncementManager();
            ValidateDates(dto.StartDate, dto.EndDate);

            if (string.IsNullOrWhiteSpace(_currentUser.UserName))
                throw new Exception("Giriş yapan kullanıcının kullanıcı adı bulunamadı.");

            var announcement = _mapper.Map<Announcement>(dto);
            announcement.IsActive = true;
            announcement.CreatedBy = _currentUser.UserName;

            await _repository.AddAsync(announcement);
            await _repository.SaveChangesAsync();
        }

        public async Task<AnnouncementUpdateDto?> GetByIdForUpdateAsync(int id)
        {
            EnsureAnnouncementManager();

            var announcement = await _repository.GetByIdAsync(id);

            return announcement == null
                ? null
                : _mapper.Map<AnnouncementUpdateDto>(announcement);
        }

        public async Task UpdateAsync(AnnouncementUpdateDto dto)
        {
            EnsureAnnouncementManager();
            ValidateDates(dto.StartDate, dto.EndDate);

            var announcement = await _repository.GetByIdAsync(dto.Id);

            if (announcement == null)
                throw new Exception("Duyuru bulunamadı.");

            _mapper.Map(dto, announcement);

            _repository.Update(announcement);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAnnouncementAsync(int id)
        {
            EnsureAnnouncementManager();

            var announcement = await _repository.GetByIdAsync(id);

            if (announcement == null)
                throw new Exception("Duyuru bulunamadı.");

            _repository.Delete(announcement);
            await _repository.SaveChangesAsync();
        }

        public async Task ChangeStatusAsync(int id)
        {
            EnsureAnnouncementManager();

            var announcement = await _repository.GetByIdAsync(id);

            if (announcement == null)
                throw new Exception("Duyuru bulunamadı.");

            announcement.IsActive = !announcement.IsActive;

            _repository.Update(announcement);
            await _repository.SaveChangesAsync();
        }

        public async Task<List<AnnouncementDashboardDto>> GetActiveDashboardAnnouncementsAsync(int count)
        {
            var announcements = await _announcementRepository.GetActiveDashboardAnnouncementsAsync(count);

            return _mapper.Map<List<AnnouncementDashboardDto>>(announcements);
        }

        private void ValidateDates(DateTime startDate, DateTime endDate)
        {
            if (endDate.Date < startDate.Date)
                throw new Exception("Bitiş tarihi başlangıç tarihinden önce olamaz.");
        }

        private void EnsureAnnouncementManager()
        {
            if (!_currentUser.IsInRole(Roles.Admin) && !_currentUser.IsInRole(Roles.HR))
                throw new UnauthorizedAccessException("Bu işlem için yetkiniz bulunmuyor.");
        }
    }
}
