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
        //ovaj int nije potreban -> id iz DTO modela
        Task<IEnumerable<VehicleModelDTO>> AddModelAsync(VehicleModelDTO modelDTO);
        Task<bool> UpdateModelAsync(VehicleModelDTO modelDTO);
        Task<bool> DeleteModelAsync(int id);
    }
}
