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
    public class VehicleMakeService : IVehicleMake
    {
        private readonly ILogger<VehicleMakeService> _logger;
        private readonly DataContext _context;
        IMapper _mapper;

        public VehicleMakeService(ILogger<VehicleMakeService> logger, IMapper mapper, DataContext context)
        {
            _logger = logger;
            _mapper = mapper;   
            _context = context; 
        }

        //GET ALL
        public async Task<IEnumerable<VehicleMakeDTO>> GetAllMakesAsync()
        {
          
            _logger.LogInformation("Getting all vehicle makes.");

            var makes = await _context.VehicleMakes.ToListAsync();

            var makeListDtos = makes.Select(m => _mapper.Map<VehicleMakeDTO>(m));
            //2. nacin -> ne radi ******* porque??
            //var makeListDtos2 = await _context.VehicleMakes.ProjectTo<VehicleMakeDTO>(_mapper.ConfigurationProvider).ToListAsync();

            return makeListDtos;
        }
        public async Task<VehicleMakeDTO> GetMakeByIdAsync(int id)
        {
            var make = await _context.VehicleMakes.FindAsync(id);

            return _mapper.Map<VehicleMakeDTO>(make);
        }

        public async Task<IEnumerable<VehicleMakeDTO>> AddMakeAsync(VehicleMakeDTO makeDTO)
        {
            var make = _mapper.Map<VehicleMake>(makeDTO);
            await _context.VehicleMakes.AddAsync(make);
            await _context.SaveChangesAsync();
            
            var makeList = await _context.VehicleMakes.ToListAsync();
            //var makeListDTO = _mapper.Map<List<VehicleMakeDTO>>(makeList);
            var makeListDtos = makeList.Select(m => _mapper.Map<VehicleMakeDTO>(m)).ToList();

            return makeListDtos;
        }
        //int nije potreban -> id iz VehicleMakeDTO modela
        public async Task<bool> UpdateMakeAsync(VehicleMakeDTO makeDTO)
        {
            var findMake = await _context.VehicleMakes.FindAsync(makeDTO.Id);
            if (findMake == null) return false;
            var make = _mapper.Map(makeDTO, findMake);

            _context.VehicleMakes.Update(make);
            var updatedMake = await _context.SaveChangesAsync();
            return updatedMake > 0; 
        } 

        public async Task<bool> DeleteMakeAsync(int id)
        {
            var findMake = await _context.VehicleMakes.FindAsync(id);
            if (findMake == null) return false;

            _context.VehicleMakes.Remove(findMake);

            var deletedMake = await _context.SaveChangesAsync();
            return deletedMake > 0;
        }

        public async Task<VehicleMakeDTO> GetMakeByNameAsync(string makeName)
        {
            var make = await _context.VehicleMakes
                .SingleOrDefaultAsync(m => m.Name == makeName);
            return _mapper.Map<VehicleMakeDTO>(make);
        }

    }
}
