using CarParkAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CarParkAPI.DBContext
{
    public class CarParkDB : DbContext
    {
        public CarParkDB(DbContextOptions<CarParkDB> options)
    : base(options)
        {
        }

        public DbSet<ParkingSpace> ParkingSpace { get; set; } = null!;
        public DbSet<Cost> Cost { get; set; } = null!;
    }
}
