using HRFlow.Business.DTOs.Announcement;
using HRFlow.Entities.HumanResources;

namespace HRFlow.Business.Interfaces
{
    public interface IAnnouncementService : IGenericService<Announcement>
    {
        Task<List<AnnouncementListDto>> GetAnnouncementListAsync();

        Task CreateAsync(AnnouncementCreateDto dto);

        Task<AnnouncementUpdateDto?> GetByIdForUpdateAsync(int id);

        Task UpdateAsync(AnnouncementUpdateDto dto);

        Task DeleteAnnouncementAsync(int id);

        Task ChangeStatusAsync(int id);

        Task<List<AnnouncementDashboardDto>> GetActiveDashboardAnnouncementsAsync(int count);
    }
}
