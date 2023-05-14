

namespace Service.Models.DTOs
{
    public class VehicleMakeDTOResponse
    {
        public List<VehicleMake> VehicleMakes { get; set; } = new List<VehicleMake>();

        public int Pages { get; set; }  

        public int CurrentPage { get; set; }    

    }
}
