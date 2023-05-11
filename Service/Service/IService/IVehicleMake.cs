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
        //ovaj int nije potreban -> id vaditi iz VehicleMakeDTO modela
        Task<IEnumerable<VehicleMakeDTO>> AddMakeAsync(VehicleMakeDTO makeDTO);
        Task<bool> UpdateMakeAsync(VehicleMakeDTO makeDto);
        Task<bool> DeleteMakeAsync(int id);
        //Task<VehicleMakeDTO> GetMakeByIdAsync(string makeName);
    }
}
