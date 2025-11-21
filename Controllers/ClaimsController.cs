using CMCS.Web.Data;
using CMCS.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CMCS.Web.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        private static readonly string[] AllowedExt = { ".pdf", ".docx", ".xlsx" };
        private const long MaxBytes = 5 * 1024 * 1024; // 5 MB

        public ClaimsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new Claim
            {
                HourlyRate = 450M,
                Entries = new List<ClaimEntry>
                {
                    new ClaimEntry
                    {
                        WorkDate = DateOnly.FromDateTime(DateTime.Today),
                        Module = "INF101",
                        TaskType = "Teaching",
                        Hours = 2
                    }
                }
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Claim model, IFormFile? upload)
        {
            if (model.Entries == null || !model.Entries.Any() || model.Entries.Sum(e => e.Hours) <= 0)
            {
                ModelState.AddModelError(string.Empty, "Please add at least one entry with positive hours.");
            }

            if (upload != null && upload.Length > 0)
            {
                var ext = Path.GetExtension(upload.FileName).ToLowerInvariant();
                if (!AllowedExt.Contains(ext))
                {
                    ModelState.AddModelError(string.Empty, "File type not allowed. Use .pdf, .docx, or .xlsx");
                }
                if (upload.Length > MaxBytes)
                {
                    ModelState.AddModelError(string.Empty, "File too large. Max 5 MB.");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.TotalHours = model.Entries.Sum(e => e.Hours);
            model.Status = ClaimStatus.Submitted;
            model.Month = DateTime.Now.ToString("dd/MM/yyyy");

            foreach (var e in model.Entries)
            {
                e.ClaimId = model.Id;
            }

            _db.Claims.Add(model);
            await _db.SaveChangesAsync();

            if (upload != null && upload.Length > 0)
            {
                var dir = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(dir);
                var fileName = $"{model.Id}_{Path.GetFileName(upload.FileName)}";
                var path = Path.Combine(dir, fileName);

                using var fs = System.IO.File.Create(path);
                await upload.CopyToAsync(fs);

                _db.Attachments.Add(new Attachment
                {
                    ClaimId = model.Id,
                    FileName = upload.FileName,
                    ContentType = upload.ContentType,
                    FileSize = upload.Length,
                    StoragePath = $"/uploads/{fileName}"
                });
                await _db.SaveChangesAsync();
            }

            TempData["Msg"] = "Claim submitted successfully.";
            return RedirectToAction("Index", "Home");
        }
    }
}
