using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using MVC.Models;
using Service.Models.DTOs;
using Service.Service;
using Service.Service.IService;

namespace MVC.Controllers
{
    public class VehicleModelController : Controller
    {
        private readonly ILogger<VehicleModelController> _logger;
        private readonly IVehicleModel _vehicleModelService;
        private readonly IMapper _mapper;
        private readonly IVehicleMake _vehicleMakeService;

        public VehicleModelController(ILogger<VehicleModelController> logger, IVehicleModel vehicleModelService, IMapper mapper, IVehicleMake vehicleMakeService)
        {
            _logger = logger;
            _vehicleModelService = vehicleModelService;
            _vehicleMakeService = vehicleMakeService;
            _mapper = mapper;
        }


        //VEHICLE MODEL ADMIN METHOD 

        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder, string searchString, int pageSize = 5, int? pageNumber = 1)
        {
            _logger.LogInformation("Index view");
            var models = await _vehicleModelService.GetSortedModelsAsync(sortOrder, searchString, pageSize, pageNumber);

            var modelsVM = models.Select(m => new VehicleModelVM
            {
                Id = m.Id,
                Name = m.Name,
                Abrv = m.Abrv,
                MakeName = m.MakeName,  
            }).ToList();

            ViewBag.IdSortParm = sortOrder == "id_asc" ? "id_desc" : "id_asc";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.AbrvSortParm = sortOrder == "abrv_asc" ? "abrv_desc" : "abrv_asc";
            ViewBag.CurrentFilter = searchString;
            ViewBag.SortOrder = sortOrder;
            ViewBag.PageSize = Request.Query.ContainsKey("pageSize") ? Request.Query["pageSize"].ToString() : "5";
            ViewBag.Message = TempData["Message"]?.ToString();
            ViewBag.ErrorMessage = TempData["ErrorMessage"]?.ToString();
            ViewBag.Message = TempData["Message"]?.ToString();



            return View(PaginatedList<VehicleModelVM>.Create(modelsVM, pageNumber ?? 1, pageSize));
        }



        //OUR HOME METHOD WHERE WE FILTER BY MAKES ONLY

        [HttpGet]
        public async Task<IActionResult> Index1(int pageSize = 5, int? pageNumber = 1, string makeName = null)
        {
 
                _logger.LogInformation("Index view");

                var models = await _vehicleModelService.GetModelsByMakeNameAsync(pageSize, pageNumber, makeName);

                var modelsVM = models.Select(m => new VehicleModelVM
                {
                    Id = m.Id,
                    Name = m.Name,
                    Abrv = m.Abrv,
                    MakeName = m.MakeName,
                }).ToList();

            return View(PaginatedList<VehicleModelVM>.Create(modelsVM, pageNumber ?? 1, pageSize));

        }



        //OUR EDIT METHOD WITH SELECT FOR MAKES

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Edit view");

            var model = await _vehicleModelService.GetModelById(id);

            var makes = await _vehicleMakeService.GetAllMakesAsync();
            ViewBag.Makes = makes.Select(m => new SelectListItem
            {
                Text = m.Name,
                Value = m.Id.ToString()

            });

            return View(model);
        }



        //UPDATE METHOD REQUIRES FIELDS AND MAKE // RETURNS PROPER MESSAGES

        public async Task<IActionResult> Update(VehicleModelDTO modelDTO)
        {
            _logger.LogInformation("Updating vehicle");

            if (!ModelState.IsValid)
            {
                var make = await _vehicleMakeService.GetMakeByIdAsync((int)modelDTO.MakeId);
                if (make == null)
                {
                    ModelState.AddModelError("", "Invalid make name.");
                }
                else
                {

                    if (string.IsNullOrEmpty(modelDTO.Name) || string.IsNullOrEmpty(modelDTO.Abrv) || modelDTO.MakeId == null)
                    {

                        ModelState.AddModelError("", "Please fill in all the required fields.");

                        var makes = await _vehicleMakeService.GetAllMakesAsync();
                        ViewBag.Makes = makes.Select(m => new SelectListItem
                        {
                            Text = m.Name,
                            Value = m.Id.ToString()

                        });

                        return View("Edit", modelDTO);
                    }

                    var result = await _vehicleModelService.UpdateModelAsync(modelDTO);
                    if (result)
                    {
                        TempData["Message"] = "Vehicle Model updated!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to update vehicle make.";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return RedirectToAction(nameof(Index));

        }


        //DELETE METHOD RETURNS PROPER MESSAGE AND PARAMS 

        [HttpPost]
        public async Task<IActionResult> Delete(int id, int pageNumber, int pageSize)
        {
            var deleted = await _vehicleModelService.DeleteModelAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            TempData["Message"] = "Vehicle Model deleted!";
            return RedirectToAction(nameof(Index), new { pageNumber, pageSize });
        }



        //CREATE METHOD WITH MAKE SELECT LIST

        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Create view");

            var makes = await _vehicleMakeService.GetAllMakesAsync();

            ViewBag.Makes = new SelectList(makes, "Id", "Name");

            return View();
        }



        //ADD MODEL METHOD //BUTTON IN CREATE.cshtml FORM // RETURNS PROPER MESSAGE

        [HttpPost]
        public async Task<IActionResult> AddModel(CreateVehicleModelDTO modelDTO)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index","VehicleModel");
            }
            var makeListDtos = await _vehicleModelService.AddModelAsync(modelDTO);
            TempData["Message"] = "Vehicle Model created!";
            return RedirectToAction(nameof(Index));

        }

    }
}