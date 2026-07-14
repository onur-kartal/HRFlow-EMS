using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.DTOs.Department
{
    public class DepartmentUpdateDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Departman adı alanı zorunludur.")]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;
    }
}
