using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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



        //INDEX METHOD

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

            ViewBag.IdSortParm = sortOrder == "id_asc" ? "id_desc" : "id_asc";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.AbrvSortParm = sortOrder == "abrv_asc" ? "abrv_desc" : "abrv_asc";
            ViewBag.CurrentFilter = searchString;
            ViewBag.SortOrder = sortOrder;
            ViewBag.PageSize = Request.Query.ContainsKey("pageSize") ? Request.Query["pageSize"].ToString() : "5";
            ViewBag.ErrorMessage = TempData["ErrorMessage"]?.ToString();
            ViewBag.Message = TempData["Message"]?.ToString();

            return View(PaginatedList<VehicleMakeVM>.Create(makesVM, pageNumber ??1, pageSize));
        }



        //EDIT METHOD

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Edit view");

            var make = await _vehicleMakeService.GetMakeByIdAsync(id);

            return View(make);
        }



        //UPDATE METHOD //BUTTON IN EDIT FORM

        [HttpPost]
        public async Task<IActionResult> Update(VehicleMakeDTO makeDTO)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(makeDTO.Name) || string.IsNullOrEmpty(makeDTO.Abrv))
                {
                    ModelState.AddModelError("", "Please fill in all the required fields.");
                    return View("Edit", makeDTO);
                }

                var result = await _vehicleMakeService.UpdateMakeAsync(makeDTO);
                if (result)
                {
                    TempData["Message"] = "Vehicle make updated!";
                    return RedirectToAction(nameof(Index));
                }
                    else
                {
                    TempData["ErrorMessage"] = "Failed to update vehicle make.";
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return View("Edit", makeDTO);
            }
        }



        //DELETE METHOD , WONT DELETE MAKES WITH MODELS AND RETURNS PROPER MESSAGE OR ERROR MESSAGE

        [HttpPost]
        public async Task<IActionResult> Delete(int id, int pageSize, int pageNumber)
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
                TempData["ErrorMessage"] = "Cannot delete make that has its own models!";
                return RedirectToAction(nameof(Index), new {pageSize, pageNumber});
            }
            TempData["Message"] = "Vehicle make deleted!";
            var deleted = await _vehicleMakeService.DeleteMakeAsync(id);

            return RedirectToAction(nameof(Index), new {pageSize, pageNumber});
        }



        //CREATE METHOD

        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Create view");


            return View();
        }


        //ADD MAKE BUTTON IN CREATE FORM , REQUIRES FIELDS AND RETURNS MESSAGES

        [HttpPost]
        public async Task<IActionResult> AddMake(VehicleMakeDTO makeDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(makeDTO);
            }

            await _vehicleMakeService.AddMakeAsync(makeDTO);
            TempData["Message"] = "New vehicle make created!!";
            return RedirectToAction(nameof(Index));

        }
    }
}



