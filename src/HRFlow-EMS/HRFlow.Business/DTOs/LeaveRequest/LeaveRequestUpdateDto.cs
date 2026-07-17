using HRFlow.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.DTOs.LeaveRequest
{
    public class LeaveRequestUpdateDto
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Personel seçiniz.")]
        public int EmployeeId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "İzin türü seçiniz.")]
        public int LeaveTypeId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public LeaveStatus Status { get; set; }
    }
}
