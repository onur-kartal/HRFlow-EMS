using HRFlow.Data.Context;
using HRFlow.Data.Interfaces;
using HRFlow.Entities.HumanResources;
using Microsoft.EntityFrameworkCore;

namespace HRFlow.Data.Repositories
{
    public class AnnouncementRepository : GenericRepository<Announcement>, IAnnouncementRepository
    {
        public AnnouncementRepository(HRFlowDbContext context)
            : base(context)
        {
        }

        public async Task<List<Announcement>> GetAnnouncementListAsync()
        {
            return await _context.Announcements
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<Announcement>> GetActiveDashboardAnnouncementsAsync(int count)
        {
            var today = DateTime.Today;

            return await _context.Announcements
                .Where(x => !x.IsDeleted &&
                            x.IsActive &&
                            x.StartDate.Date <= today &&
                            x.EndDate.Date >= today)
                .OrderByDescending(x => x.StartDate)
                .Take(count)
                .ToListAsync();
        }
    }
}
