using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
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
        private readonly DataContext _context;
        IMapper _mapper;

        public VehicleMakeService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;   
            _context = context; 
        }

        //GET ALL
        public async Task<IEnumerable<VehicleMakeDTO>> GetAllMakesAsync()
        {
            var makes = await _context.VehicleMakes.ToListAsync();

            var makeListDtos = makes.Select(m => _mapper.Map<VehicleMakeDTO>(m));


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
        //int id nije potreban jer se moze iskoristiti id iz VehicleMakeDTO modela
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
    }
}
