using HRFlow.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace HRFlow.Business.DTOs.OvertimeRequest
{
    public class OvertimeRequestStatusChangeDto
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [EnumDataType(typeof(OvertimeStatus))]
        public OvertimeStatus Status { get; set; }
    }
}
