using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.DTOs.LeaveType
{
    public class LeaveTypeCreateDto
    {
        [Required(ErrorMessage = "İzin adı alanı zorunludur.")]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        public int DefaultDays { get; set; }
        public bool IsPaid { get; set; }
    }
}
