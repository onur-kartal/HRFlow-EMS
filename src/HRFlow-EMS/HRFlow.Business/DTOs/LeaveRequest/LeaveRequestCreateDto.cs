using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.DTOs.LeaveRequest
{
    public class LeaveRequestCreateDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "İzin türü seçiniz.")]
        public int LeaveTypeId { get; set; }

        [Required(ErrorMessage = "Başlangıç tarihi zorunludur.")]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Bitiş tarihi zorunludur.")]
        public DateTime EndDate { get; set; } = DateTime.Today;

        [StringLength(500)]
        public string? Description { get; set; }
    }
}
