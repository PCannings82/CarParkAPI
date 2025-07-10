
using CarParkAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarParkAPI.Services
{
    public interface ICostService
    {
        void addCostToTotal(double totalCharge);
        Task<ActionResult<Cost>> getTotalCost();
    }
}
