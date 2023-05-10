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
            //viewModel bi trebao biti isto što i DTO pa nije potrebno mapiranje?!

            return View(makes);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Edit view");

            var make = await _vehicleMakeService.GetMakeByIdAsync(id);

            return View(make);
        }

        [HttpPost]
        public async Task<IActionResult> Update(VehicleMakeDTO makeDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _vehicleMakeService.UpdateMakeAsync(makeDTO);
                if (result)
                {
                    return RedirectToAction("Index", "VehicleMake");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to update vehicle make.");
                }
            }
            return View(makeDTO);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _vehicleMakeService.DeleteMakeAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Create view");


            return View();
        }

            [HttpPost]
        public async Task<IActionResult> AddMake(VehicleMakeDTO makeDTO)
        {
            if(!ModelState.IsValid) 
            {
                return View(makeDTO);
            }
                var makeListDtos = await _vehicleMakeService.AddMakeAsync(makeDTO);
                return View("Index", makeListDtos);
            
        }
    }
}
