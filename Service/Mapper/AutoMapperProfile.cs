using AutoMapper;
using Service.Models;
using Service.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<VehicleMake, VehicleMakeDTO>();
            CreateMap<CreateVehicleMakeDTO, VehicleMake>();
        }

    }
}
