using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Service.Models.DTOs;
using Service.Service.IService;

namespace MVC.Controllers
{
    public class VehicleMakeController : Controller
    {
        protected IVehicleMake VehicleMake;
        private readonly IMapper _mapper;

        public VehicleMakeController(IMapper mapper, IVehicleMake vehicleMake)
        {
            _mapper = mapper;
            VehicleMake = vehicleMake;
        }


        [HttpGet]
        public async Task<ActionResult> FindAllAsync(VehicleMakeDTO vehicleMake)
        {

        }





    }
}
