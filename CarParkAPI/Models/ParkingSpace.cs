using CarParkAPI.Enums;

namespace CarParkAPI.Models
{
    public class ParkingSpace
    {
        public int Id { get; set; }
        public SpaceStatus Status { get; set; }

        public string? VehicleRegistration { get; set; }

        public DateTime? TimeIn { get; set; }

    }
}
