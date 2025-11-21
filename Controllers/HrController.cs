using CMCS.Web.Data;
using CMCS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Web.Controllers
{
    public class HrController : Controller
    {
        private readonly AppDbContext _db;
        public HrController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var approved = await _db.Claims
                .Where(c => c.Status == ClaimStatus.Approved)
                .ToListAsync();

            var total = approved.Sum(c => c.TotalAmount);
            ViewBag.TotalAmount = total;

            return View(approved);
        }
    }
}
