using Service.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service.IService
{
    public interface IVehicleModel
    {
        Task<IEnumerable<VehicleModelDTO>> GetModelsAsync();
        Task<VehicleModelDTO> GetModelById(int id);
        Task<IEnumerable<VehicleModelDTO>> AddModelAsync(CreateVehicleModelDTO modelDTO);
        Task<bool> UpdateModelAsync(VehicleModelDTO modelDTO);
        Task<bool> DeleteModelAsync(int id);
        Task<IEnumerable<VehicleModelDTO>> GetModelsByMakeNameAsync(string makeName = null);
        Task<bool> CountModelsByMakeIdAsync(int makeId);
        Task<IEnumerable<VehicleModelDTO>> GetSortedModelsAsync(string sortOrder, string searchString);
    }
}
