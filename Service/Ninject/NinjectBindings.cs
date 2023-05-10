using Ninject.Modules;
using Service.Models.DTOs;
using Service.Models;
using Service.Service;
using Service.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Service.Mapper;
using Microsoft.EntityFrameworkCore;
using Service.Data;

namespace Service.Ninject
{
    public  class NinjectBindings : NinjectModule
    {
        public override void Load()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            var mapper = mapperConfiguration.CreateMapper();


            Bind<IMapper>().ToConstant(mapper);

            Bind<IVehicleMake>().To<VehicleMakeService>();
            Bind<IVehicleModel>().To<VehicleModelService>();        

        }
    }
}
