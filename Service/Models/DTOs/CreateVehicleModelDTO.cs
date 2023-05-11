using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.DTOs
{
    public class CreateVehicleModelDTO
    {
        public string Name { get; set; }
        public string Abrv { get; set; }
        public int MakeId { get; set; } 
    }
}
