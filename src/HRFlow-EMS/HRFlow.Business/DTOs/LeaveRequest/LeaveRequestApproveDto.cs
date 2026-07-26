using System.ComponentModel.DataAnnotations;

namespace HRFlow.Business.DTOs.LeaveRequest
{
    public class LeaveRequestApproveDto
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
    }
}
