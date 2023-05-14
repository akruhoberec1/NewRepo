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
        public async Task<IActionResult> Index(string sortOrder, string searchString, int pageSize = 5, int? pageNumber = 1)
        {
            _logger.LogInformation("Index view");


            var makes = await _vehicleMakeService.GetSortedMakesAsync(sortOrder, searchString, pageSize, pageNumber);

            var makesVM = makes.Select(m => new VehicleMakeVM
            {
                Id = m.Id,
                Name = m.Name,
                Abrv = m.Abrv
            }).ToList();

            ViewData["IdSortParm"] = sortOrder == "id_asc" ? "id_desc" : "id_asc";
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["AbrvSortParm"] = sortOrder == "abrv_asc" ? "abrv_desc" : "abrv_asc";
            ViewData["CurrentFilter"] = searchString;
            ViewData["SortOrder"] = sortOrder;

            return View(PaginatedList<VehicleMakeVM>.Create(makesVM, pageNumber ??1, pageSize));
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
            return RedirectToAction(nameof(Index));
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
                //var makesVM = makes.Select(m => new VehicleMakeVM
                //{
                //    Id = m.Id,
                //    Name = m.Name,
                //    Abrv = m.Abrv
                //}).ToList();
                return RedirectToAction(nameof(Index));
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
            if (!ModelState.IsValid)
            {
                return View(makeDTO);
            }

            var makeListDTOs = await _vehicleMakeService.AddMakeAsync(makeDTO);
            var makeListVMs = makeListDTOs.Select(m => new VehicleMakeVM
            {
                Id = m.Id,
                Name = m.Name,
                Abrv = m.Abrv
            }).ToList();

            return View("Index", makeListVMs);

        }
    }
}



