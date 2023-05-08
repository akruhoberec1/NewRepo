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
        Task<List<CreateVehicleMakeDTO>> AddMakeAsync(CreateVehicleMakeDTO make);

    }
}
