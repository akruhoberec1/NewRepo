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
        //uvijek treba koristiti apstrakciju, ako koristimo interface, a ne strogo odredjenu kolekciju, kasnije je lakse napraviti promjenu u kodu
        Task<IEnumerable<VehicleMakeDTO>> GetAllMakesAsync();
        Task<VehicleMakeDTO> GetMakeByIdAsync(int id);

        //mozemo koristiti id iz VehicleMakeDTO modela
        Task<IEnumerable<VehicleMakeDTO>> AddMakeAsync(VehicleMakeDTO makeDTO);
        Task<bool> UpdateMakeAsync(VehicleMakeDTO makeDto);
        Task<bool> DeleteMakeAsync(int id);
    }
}
