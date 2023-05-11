using Service.Models.BaseEn;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Service.Models
{
    public class VehicleModel : BaseEntity
    {


        public int MakeId { get; set; }
        [ForeignKey("MakeId")]
        [JsonIgnore]
        public virtual VehicleMake VehicleMake { get; set; }    
    }
}
