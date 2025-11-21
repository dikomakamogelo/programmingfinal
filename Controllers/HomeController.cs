using CMCS.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var claims = await _db.Claims
                .Include(c => c.Attachments)
                .OrderByDescending(c => c.Month)
                .ToListAsync();

            return View(claims);
        }
    }
}
