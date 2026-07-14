using HRFlow.Business.DTOs.Department;
using HRFlow.Business.DTOs.Position;
using HRFlow.Entities.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.Interfaces
{
    public interface IPositionService : IGenericService<Position>
    {
        Task<List<PositionListDto>> GetPositionListAsync();

        Task CreateAsync(PositionCreateDto dto);

        Task<PositionUpdateDto?> GetByIdForUpdateAsync(int id);

        Task UpdateAsync(PositionUpdateDto dto);
    }
}
