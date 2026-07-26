using System.ComponentModel.DataAnnotations;

namespace HRFlow.Business.DTOs.Announcement
{
    public class AnnouncementCreateDto
    {
        [Required(ErrorMessage = "Başlık zorunludur.")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "İçerik zorunludur.")]
        [StringLength(2000)]
        public string Content { get; set; } = string.Empty;

        [Required(ErrorMessage = "Başlangıç tarihi zorunludur.")]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Bitiş tarihi zorunludur.")]
        public DateTime EndDate { get; set; } = DateTime.Today;
    }
}
