using CarParkAPI.DBContext;
using CarParkAPI.Enums;
using CarParkAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarParkAPI.Services
{
    public class ParkingService : IParkingService
    {
        private readonly CarParkDB _context;
        private readonly ICostService _costService;

        public ParkingService(CarParkDB context, ICostService costService)
        {
            _context = context;
            _costService = costService;
        }

        public async Task<ParkedCar> AddAsync(Park park)
        {
            var availableSpace = await _context.ParkingSpace
                .Where(p => p.Status == SpaceStatus.Available)
                .OrderBy(p => p.Id)
                .FirstOrDefaultAsync();

            if (availableSpace == null)
                throw new Exception("No available parking spaces.");

            availableSpace.Status = SpaceStatus.Occupied;
            availableSpace.VehicleRegistration = park.VehicleRegistration;
            availableSpace.TimeIn = DateTime.UtcNow;

            var parkedCar = new ParkedCar
            {
                VehicleRegistration = availableSpace.VehicleRegistration,
                SpaceNumber = availableSpace.Id,
            };

            await _context.SaveChangesAsync();

            return parkedCar;
        }

        public async Task<ActionResult<string>> DeleteCar(string vehicleRegistration)
        {
            var towedCar = await _context.ParkingSpace
                .FirstOrDefaultAsync(p => p.VehicleRegistration == vehicleRegistration && p.Status == SpaceStatus.Occupied);

            if (towedCar == null)
                return null;

            towedCar.VehicleRegistration = null;
            towedCar.TimeIn = null;
            towedCar.Status = SpaceStatus.Available;

            await _context.SaveChangesAsync();
            return "Car has been towed";
        }

        public async Task<ActionResult<Charge>> Exit(Exit exit)
        {
            var car = await _context.ParkingSpace
            .FirstOrDefaultAsync(p => p.VehicleRegistration == exit.VehicleRegistration && p.Status == SpaceStatus.Occupied);
            
            if (car == null)
                return null;

            var timeOut = DateTime.UtcNow;
            var timeIn = car.TimeIn ?? timeOut;

            var duration = timeOut - timeIn;
            var totalMinutes = Math.Ceiling(duration.TotalMinutes);

            double ratePerMinute = 0.10d;
            double totalCharge = totalMinutes * ratePerMinute;

            _costService.addCostToTotal(totalCharge);

            car.Status = SpaceStatus.Available;
            car.VehicleRegistration = null;
            car.TimeIn = null;

            await _context.SaveChangesAsync();

            return new Charge
            {
                VehicleRegistration = exit.VehicleRegistration,
                TimeIn = timeIn,
                TimeOut = timeOut,
                TotalCharge = totalCharge
            };
        }

        public async Task<ActionResult<IEnumerable<ParkingSpace>>> GetAllAsync()
        {
           return await _context.ParkingSpace.ToListAsync();
        }

        public async Task<ActionResult<Spaces>> GetSpaces()
        {
            var occupiedCount = await _context.ParkingSpace
            .CountAsync(p => p.Status == SpaceStatus.Occupied);

            int totalSpaces = await _context.ParkingSpace.CountAsync();

            var spaces = new Spaces
            {
                AvailableSpaces = totalSpaces - occupiedCount,
                OccupiedSpaces = occupiedCount,
            };

            return spaces;
        }
    }
}
