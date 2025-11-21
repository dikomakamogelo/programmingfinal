using CMCS.Web.Data;
using CMCS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Web.Controllers
{
    public class ApproverController : Controller
    {
        private readonly AppDbContext _db;
        public ApproverController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var pending = await _db.Claims
                .Include(c => c.Attachments)
                .Where(c => c.Status == ClaimStatus.Submitted)
                .ToListAsync();

            return View(pending);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Decide(Guid id, string decision, string? comment)
        {
            var claim = await _db.Claims.FirstOrDefaultAsync(c => c.Id == id);
            if (claim == null) return NotFound();

            claim.Status = decision.ToLower() == "approve"
                ? ClaimStatus.Approved
                : ClaimStatus.Rejected;

            _db.Approvals.Add(new Approval
            {
                ClaimId = claim.Id,
                ApproverRole = "PC/AM",
                Decision = claim.Status == ClaimStatus.Approved ? "Approved" : "Rejected",
                Comment = comment ?? string.Empty
            });

            await _db.SaveChangesAsync();

            TempData["Msg"] = $"Claim {decision}d.";
            return RedirectToAction(nameof(Index));
        }
    }
}
