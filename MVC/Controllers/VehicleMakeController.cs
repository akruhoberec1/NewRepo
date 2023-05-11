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
        private readonly IVehicleModel _vehicleModelService;
        private readonly IVehicleMake _vehicleMakeService;
        private readonly IMapper _mapper;

        public VehicleMakeController(ILogger<VehicleMakeController> logger, IVehicleMake vehicleMakeService, IMapper mapper, IVehicleModel vehicleModelService)
        {
            _logger = logger;
            _vehicleMakeService = vehicleMakeService;
            _mapper = mapper;
            _vehicleModelService = vehicleModelService; 

        }

        [HttpGet]
        public async Task<IActionResult> Index(int? pageNumber, int? pageSize, string searchQuery, string sortBy, string sortOrder)
        {
            pageNumber ??= 1;
            pageSize ??= 5;

            // set default sorting options if not provided
            sortBy ??= "Name";
            sortOrder ??= "asc";

            // get list of makes from service
            var makes = await _vehicleMakeService.FindMakesAsync(searchQuery, pageSize.Value, pageNumber.Value, sortBy, sortOrder);

            // set ViewData for page size and sorting options
            ViewData["PageSize"] = pageSize;
            ViewData["SearchQuery"] = searchQuery;
            ViewData["SortBy"] = sortBy;
            ViewData["SortOrder"] = sortOrder;

            // map DTOs to view models
            var makesVM = new List<VehicleMakeVM>();

            foreach (var m in makes)
            {
                var makeVM = new VehicleMakeVM
                {
                    Id = m.Id,
                    Name = m.Name,
                    Abrv = m.Abrv
                };

                makesVM.Add(makeVM);
            }

            return View(makesVM);
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

            if (id == null)
            {
                return NotFound();
            }
            var makeDTO = await _vehicleMakeService.GetMakeByIdAsync(id);
            if (makeDTO == null)
            {
                return NotFound();
            }
            var hasModels = await _vehicleModelService.CountModelsByMakeIdAsync(id);
            if (hasModels)
            {
                ModelState.AddModelError("", "Cannot delete make that has it's own models!");
                var makes = await _vehicleMakeService.GetAllMakesAsync();
                return View("Index", makes);
            }

            var deleted = await _vehicleMakeService.DeleteMakeAsync(id);
            
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
