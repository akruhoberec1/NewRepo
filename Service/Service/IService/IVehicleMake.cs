using Service.Models;
using Service.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service.IService
{
    public interface IVehicleMake
    {

        Task<IEnumerable<VehicleMakeDTO>> GetAllMakesAsync();
        Task<VehicleMakeDTO> GetMakeByIdAsync(int id);
        Task<IEnumerable<VehicleMakeDTO>> AddMakeAsync(VehicleMakeDTO makeDTO);
        Task<bool> UpdateMakeAsync(VehicleMakeDTO makeDto);
        Task<bool> DeleteMakeAsync(int id);
        Task<(IEnumerable<VehicleMakeDTO>makes,int totalCount)> FindMakesAsync(string searchQuery, int pageNum, int pageSize, string sortBy, string sortOrder);
        Task<IEnumerable<VehicleMakeDTO>> GetSortedMakesAsync(string sortOrder, string searchString, int pageSize, int? pageNumber);
    }
}
