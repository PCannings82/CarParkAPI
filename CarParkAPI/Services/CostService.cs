using CarParkAPI.DBContext;
using CarParkAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarParkAPI.Services
{
    public class CostService : ICostService
    {

        private readonly CarParkDB _context;

        public CostService(CarParkDB context)
        {
            _context = context;
        }

        public async void addCostToTotal(double totalCharge)
        {
            // Assume you're tracking revenue in the record with Id = 1
            var costRecord = await _context.Cost.FirstOrDefaultAsync(c => c.Id == 1);

            if (costRecord == null)
            {
                // If it doesn't exist, create it
                costRecord = new Cost { TotalCollected = totalCharge };
                _context.Cost.Add(costRecord);
            }
            else
            {
                costRecord.TotalCollected += totalCharge;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<ActionResult<Cost>> getTotalCost()
        {
            return await _context.Cost.FirstOrDefaultAsync(c => c.Id == 1);
        }
    }
}
