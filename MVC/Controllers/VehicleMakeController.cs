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
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Index view");
            var makes = await _vehicleMakeService.GetAllMakesAsync();

            var makesVM = makes.Select(m => new VehicleMakeVM
            {
                Id = m.Id,
                Name = m.Name,
                Abrv = m.Abrv
            }).ToList();

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
                var makesVM = makes.Select(m => new VehicleMakeVM
                {
                    Id = m.Id,
                    Name = m.Name,
                    Abrv = m.Abrv
                }).ToList();
                return View("Index", makesVM);
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





//prvi pokušaji pretraživanja, sortiranja i filtriranja, idući put redom, ne odjednom

//public async Task<IActionResult> Index(int? pageNum, int? pageSize, string searchQuery, string sortBy, string sortOrder)
//{
//    pageNum ??= 1;
//    pageSize ??= 5;


//    sortBy ??= "Name";
//    sortOrder ??= "asc";


//    var (makes, totalCount) = await _vehicleMakeService.FindMakesAsync(searchQuery, pageSize.Value, pageNum.Value, sortBy, sortOrder);


//    ViewData["PageSize"] = pageSize;
//    ViewData["SearchQuery"] = searchQuery;
//    ViewData["SortBy"] = sortBy;
//    ViewData["SortOrder"] = sortOrder;
//    ViewData["TotalCount"] = totalCount;


//    var makesVM = new List<VehicleMakeVM>();

//    foreach (var m in makesVM)
//    {
//        var makeVM = new VehicleMakeVM
//        {
//            Id = m.Id,
//            Name = m.Name,
//            Abrv = m.Abrv,
//            //SortOrder = sortOrder,
//            //PageSize = pageSize.Value,
//        };

//        makesVM.Add(makeVM);
//    }

//    switch (sortBy.ToLower())
//    {
//        case "name":
//            makesVM = sortOrder.ToLower() == "asc" ? makesVM.OrderBy(m => m.Name).ToList() : makesVM.OrderByDescending(m => m.Name).ToList();
//            break;
//        case "abrv":
//            makesVM = sortOrder.ToLower() == "asc" ? makesVM.OrderBy(m => m.Abrv).ToList() : makesVM.OrderByDescending(m => m.Abrv).ToList();
//            break;
//        case "id":
//            makesVM = sortOrder.ToLower() == "asc" ? makesVM.OrderBy(m => m.Id).ToList() : makesVM.OrderByDescending(m => m.Id).ToList();
//            break;
//    }

//    var paginatedMakesVM = new PaginatedList<VehicleMakeVM>(makesVM,totalCount, pageNum.Value, pageSize.Value);
//    return View(paginatedMakesVM);
//}