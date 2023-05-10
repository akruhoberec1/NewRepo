using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

            var models = await _context.VehicleModels.ToListAsync();

            var modelListDtos = models.Select(m => _mapper.Map<VehicleModelDTO>(m));

            return modelListDtos;
        }

        public async Task<IEnumerable<VehicleModelDTO>> AddModelAsync(VehicleModelDTO modelDTO)
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
            var findModel = await _context.VehicleModels.FindAsync(modelDTO.Id);
            if (findModel == null) return false;
            var model = _mapper.Map(modelDTO, findModel);

            _context.VehicleModels.Update(model);
            var updatedModel = await _context.SaveChangesAsync();
            return updatedModel > 0;
        }
        
    }
}
