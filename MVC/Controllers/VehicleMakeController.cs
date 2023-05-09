using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using Service.Service.IService;

namespace MVC.Controllers
{
    public class VehicleMakeController : Controller
    {
        private readonly IVehicleMake _vehicleMakeService;
        private readonly IMapper _mapper;

        public VehicleMakeController(IVehicleMake vehicleMakeService, IMapper mapper)
        {
            _vehicleMakeService = vehicleMakeService;   
            _mapper = mapper;   
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var makes = await _vehicleMakeService.GetAllMakesAsync();
            var makeVMs = _mapper.Map<List<VehicleMakeVM>>(makes);

            return View(makeVMs);
        }





    }
}
