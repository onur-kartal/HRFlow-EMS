namespace HRFlow.Business.DTOs.Announcement
{
    public class AnnouncementListDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
    }
}
