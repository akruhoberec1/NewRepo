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
        Task<List<VehicleMakeDTO>> GetAllMakesAsync();
        Task<VehicleMakeDTO> GetMakeByIdAsync(int id);

        Task<List<VehicleMakeDTO>> AddMakeAsync(VehicleMakeDTO makeDTO);
        Task<bool> UpdateMakeAsync(int id, VehicleMakeDTO makeDto);
        Task<bool> DeleteMakeAsync(int id);
    }
}
