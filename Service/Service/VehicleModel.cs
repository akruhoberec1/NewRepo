﻿using AutoMapper;
using Service.Data;
using Service.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class VehicleModel : IVehicleModel
    {
        private readonly DataContext _context;
        IMapper _mapper;

        public VehicleModel(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }
    }
}
