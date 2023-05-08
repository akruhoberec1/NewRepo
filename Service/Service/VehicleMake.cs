using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Service.Data;
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
    public class VehicleMake : IVehicleMake
    {
        private readonly DataContext _context;
        IMapper _mapper;

        public VehicleMake(IMapper mapper, DataContext context)
        {
            _mapper = mapper;   
            _context = context; 
        }

        public async Task<List<CreateVehicleMakeDTO>> AddMakeAsync(CreateVehicleMakeDTO make)
        {
            make = await _context.AddAsync(make);

            return make;
        }
    }
}
