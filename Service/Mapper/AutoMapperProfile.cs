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
            CreateMap<VehicleMakeDTO, VehicleMake>();

            ////CreateMap<IEnumerable<VehicleMake>, IEnumerable<VehicleMakeDTO>>().ReverseMap();
            CreateMap<VehicleModel, VehicleModelDTO>()
                .ForMember(dest => dest.MakeName, opt => opt.MapFrom(src => src.VehicleMake.Name));
            CreateMap<CreateVehicleModelDTO, VehicleModel>();
        }

    }
} 
