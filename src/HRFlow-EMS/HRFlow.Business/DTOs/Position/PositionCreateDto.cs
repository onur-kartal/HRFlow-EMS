using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.DTOs.Position
{
    public class PositionCreateDto
    {
        [Required(ErrorMessage = "Pozisyon adı alanı zorunludur.")]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;
    }
}
