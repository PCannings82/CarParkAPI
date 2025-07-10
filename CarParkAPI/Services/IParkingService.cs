using CarParkAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarParkAPI.Services
{
    public interface IParkingService
    {
        Task<ActionResult<IEnumerable<ParkingSpace>>> GetAllAsync();
        Task<ParkedCar> AddAsync(Park park);
        Task<ActionResult<Spaces>> GetSpaces();
        Task<ActionResult<string>> DeleteCar(string vehicleRegistration);
        Task<ActionResult<Charge>> Exit(Exit exit);
    }
}
