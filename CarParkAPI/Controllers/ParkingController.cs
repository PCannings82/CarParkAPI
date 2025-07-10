using CarParkAPI.Models;
using CarParkAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarParkAPI.Controllers
{
    [Route("api/1/[controller]")]
    [ApiController]
    public class ParkingController : ControllerBase
    {

        private readonly IParkingService _parkingService;
        private readonly ICostService _costService;

        public ParkingController(IParkingService parkingService, ICostService costService)
        {
            _parkingService = parkingService;
            _costService = costService;
        }

        [HttpGet]
        [Route("parkingspaces")]
        public async Task<ActionResult<IEnumerable<ParkingSpace>>> GetParkingSpaces()
        {
            return await _parkingService.GetAllAsync();
        }

        [HttpPost]
        public async Task<ActionResult<ParkedCar>> PostParking(Park park)
        {
            var updatedSpace = await _parkingService.AddAsync(park);

            if (updatedSpace == null)
                return NotFound("No available parking spaces.");

            return Ok(updatedSpace);
        }

        [HttpGet]
        public async Task<ActionResult<Spaces>> GetParking()
        {
            var spaces = await _parkingService.GetSpaces();

            if (spaces == null)
                return NotFound("No space data available");

            return Ok(spaces);
        }

        [HttpDelete]
        public async Task<ActionResult<string>> DeleteParking([FromQuery] string vehicleRegistration)
        {
            var carTowed = await _parkingService.DeleteCar(vehicleRegistration);

            if (carTowed == null)
                return NotFound("Car not found");

            return Ok(carTowed);
        }

        [HttpPost]
        [Route("exit")]
        public async Task<ActionResult<Charge>> ExitParking(Exit exit)
        {
            var charge = await _parkingService.Exit(exit);

            if (charge == null)
                return NotFound("No charge applied");

            return Ok(charge);
        }

        [HttpGet]
        [Route("costs")]
        public async Task<ActionResult<Cost>> GetCost()
        {
            var cost = await _costService.getTotalCost();

            if (cost == null)
                return NotFound("No cost data available");

            return Ok(cost);
        }

    }
}
