using Service.Models.BaseEn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Service.Models
{
    public class VehicleMake : BaseEntity
    {

        public VehicleMake() 
        {
            VehicleModels = new HashSet<VehicleModel>();
        }


        [JsonIgnore]
        public virtual ICollection<VehicleModel> VehicleModels { get; set; }
    }
}
