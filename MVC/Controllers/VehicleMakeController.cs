using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using Service.Models.DTOs;
using Service.Service.IService;

namespace MVC.Controllers
{
    public class VehicleMakeController : Controller
    {
        private readonly ILogger<VehicleMakeController> _logger;
        private readonly IVehicleMake _vehicleMakeService;
        private readonly IMapper _mapper;

        public VehicleMakeController(ILogger<VehicleMakeController> logger, IVehicleMake vehicleMakeService, IMapper mapper)
        {
            _logger = logger;
            _vehicleMakeService = vehicleMakeService;   
            _mapper = mapper;   
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Index view");
            var makes = await _vehicleMakeService.GetAllMakesAsync();
            //var makeVMs = _mapper.Map<List<VehicleMakeVM>>(makes);
            //viewModel ti je isto kao i DTO koji dobijes pozivanjem GetAllMakesAsync metode, stoga je nije potrebno mapirati jos jednom u kotroleru

            return View(makes);
        }

        public async Task<IActionResult> UpdateMakeAsync(VehicleMakeDTO makeDto)
        {
            _logger.LogInformation("Update view");

            var make = await _vehicleMakeService.UpdateMakeAsync(makeDto);

            return View(make);
        }
    }
}
