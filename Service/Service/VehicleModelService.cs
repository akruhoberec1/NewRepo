using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ninject.Activation.Caching;
using Service.Data;
using Service.Models;
using Service.Models.DTOs;
using Service.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class VehicleModelService : IVehicleModel
    {
        private readonly ILogger<VehicleModelService> _logger;
        private readonly DataContext _context;
        IMapper _mapper;


        public VehicleModelService(IMapper mapper, DataContext context, ILogger<VehicleModelService> logger)
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<VehicleModelDTO>> GetModelsAsync()
        {
            _logger.LogInformation("Getting all vehicle models!");

            var models = await _context.VehicleModels.Include(vm => vm.VehicleMake).ToListAsync();

            var modelListDtos = models.Select(m => _mapper.Map<VehicleModelDTO>(m));

            return modelListDtos;
        }

        public async Task<IEnumerable<VehicleModelDTO>> GetSortedModelsAsync(string sortOrder,string searchString)
        {
            var models = await GetModelsAsync();

            searchString = searchString?.ToLower();

            if(!String.IsNullOrEmpty(searchString)) 
            { 
                models = models.Where(m => m.Name.ToLower().Contains(searchString) || m.Abrv.ToLower().Contains(searchString) || m.MakeName.ToLower().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "id_asc":
                    models = models.OrderBy(m => m.Id);
                    break;
                case "id_desc":
                    models = models.OrderByDescending(m => m.Id);
                    break;
                case "name_desc":
                    models = models.OrderByDescending(m => m.Name);
                    break;
                case "abrv_desc":
                    models = models.OrderByDescending(m => m.Abrv);
                    break;
                case "abrv_asc":
                    models = models.OrderBy(m => m.Abrv);
                    break;
                case "make_desc":
                    models = models.OrderByDescending(m => m.MakeName);
                    break;
                case "make_asc":
                    models = models.OrderBy(m => m.MakeName);
                    break;
                default:
                    models = models.OrderBy(m => m.Name);
                    break;
            }

            return models;
        }

        public async Task<IEnumerable<VehicleModelDTO>> AddModelAsync(CreateVehicleModelDTO modelDTO)
        {
            var model = _mapper.Map<VehicleModel>(modelDTO);
            await _context.VehicleModels.AddAsync(model);
            await _context.SaveChangesAsync();

            var modelList = await _context.VehicleModels.ToListAsync();

            var modelListDtos = modelList.Select(m => _mapper.Map<VehicleModelDTO>(m)).ToList();

            return modelListDtos;
        }

        public async Task<bool> DeleteModelAsync(int id)
        {
            var findModel = await _context.VehicleModels.FindAsync(id);
            if (findModel == null) return false;

            _context.VehicleModels.Remove(findModel);

            var deletedModel = await _context.SaveChangesAsync();
            return deletedModel > 0;
        }

        public async Task<VehicleModelDTO> GetModelById(int id)
        {
            var model = await _context.VehicleModels.FindAsync(id);

            return _mapper.Map<VehicleModelDTO>(model);
        }

        public async Task<bool> UpdateModelAsync(VehicleModelDTO modelDTO)
        {
            var findModel = await _context.VehicleModels
                .Include(vm => vm.VehicleMake)
                .FirstOrDefaultAsync(vm => vm.Id == modelDTO.Id);
            var findMake = await _context.VehicleMakes
                .FirstOrDefaultAsync(vm => vm.Id == modelDTO.MakeId);

            if (findModel == null) return false;

            findModel.Name = modelDTO.Name;
            findModel.Abrv = modelDTO.Abrv;
            findModel.MakeId = findMake.Id;

            _context.VehicleModels.Update(findModel);
            var updatedModel = await _context.SaveChangesAsync();
            return updatedModel > 0;
        }

        public async Task<IEnumerable<VehicleModelDTO>> GetModelsByMakeNameAsync(string makeName = null)
        {
            IQueryable<VehicleModel> query = _context.VehicleModels.Include(vm => vm.VehicleMake);

            if (!string.IsNullOrEmpty(makeName))
            {
                query = query.Where(vm => vm.VehicleMake.Name.Contains(makeName));
            }

            var models = await query.AsNoTracking().ToListAsync();
            var modelsDTO = _mapper.Map<IEnumerable<VehicleModelDTO>>(models);

            return modelsDTO;
        }

        public async Task<bool> CountModelsByMakeIdAsync(int makeId)
        {
            var countModels =  await _context.VehicleModels
                .CountAsync(vm => vm.MakeId == makeId);

            return countModels > 0 ? true : false;
        }

    }
}

