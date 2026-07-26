using System.ComponentModel.DataAnnotations;

namespace HRFlow.Business.DTOs.OvertimeRequest
{
    public class OvertimeRequestCreateDto
    {
        [Required(ErrorMessage = "Çalışma tarihi zorunludur.")]
        public DateTime WorkDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Başlangıç saati zorunludur.")]
        public TimeSpan? StartTime { get; set; }

        [Required(ErrorMessage = "Bitiş saati zorunludur.")]
        public TimeSpan? EndTime { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
    }
}
