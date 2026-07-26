using HRFlow.Entities.Base;

namespace HRFlow.Entities.HumanResources
{
    public class Announcement : BaseEntity
    {
        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        public string CreatedBy { get; set; } = string.Empty;
    }
}
