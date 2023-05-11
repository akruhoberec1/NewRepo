using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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





        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Index view");
            var models = await _vehicleModelService.GetModelsAsync();

            return View(models);
        }






        [HttpGet]
        public async Task<IActionResult> Index1(string makeName)
        {
 
                _logger.LogInformation("Index view");

                var models = await _vehicleModelService.GetModelsByMakeNameAsync(makeName);

                return View(models);


            //_logger.LogInformation("Index view");
            //var models = await _vehicleModelService.GetModelsAsync();

            //return View(models);
            ////var modelsDTO = await _vehicleModelService.GetModelsAsync();
            //////if (!string.IsNullOrEmpty(MakeName))
            //////{
            //////    modelsDTO = modelsDTO.Where(m => m.MakeName.ToLower().Contains(MakeName.ToLower()));
            //////}
            ////return View(modelsDTO);
        }

        



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






        [HttpPost]
        //public async Task<IActionResult> Update(VehicleModelDTO modelDTO)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = await _vehicleModelService.UpdateModelAsync(modelDTO);
        //        if (result)
        //        {
        //            return RedirectToAction("Index", "VehicleModel");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Failed to update vehicle make.");
        //        }
        //    }
        //    return View(modelDTO);
        //}






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
                    var result = await _vehicleModelService.UpdateModelAsync(modelDTO);
                    if (result)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to update vehicle make.");
                    }
                }
            }

            var modelsDTO = await _vehicleModelService.GetModelsAsync();
            return View("Index", modelsDTO);
        }
    





        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _vehicleModelService.DeleteModelAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Create view");

            var makes = await _vehicleMakeService.GetAllMakesAsync();

            ViewBag.Makes = new SelectList(makes, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddModel(CreateVehicleModelDTO modelDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(modelDTO);
            }
            var makeListDtos = await _vehicleModelService.AddModelAsync(modelDTO);
            return RedirectToAction("Index","VehicleModel");

        }

    }
}