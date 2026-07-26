using HRFlow.Common.Interfaces;
using HRFlow.Entities.HumanResources;

namespace HRFlow.Data.Interfaces
{
    public interface IAnnouncementRepository : IGenericRepository<Announcement>
    {
        Task<List<Announcement>> GetAnnouncementListAsync();

        Task<List<Announcement>> GetActiveDashboardAnnouncementsAsync(int count);
    }
}
