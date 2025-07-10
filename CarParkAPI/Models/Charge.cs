namespace CarParkAPI.Models
{
    public class Charge
    {
        public string VehicleRegistration { get; set; }

        public DateTime? TimeIn { get; set; }

        public DateTime? TimeOut { get; set; }

        public double TotalCharge { get; set; }
    }
}
