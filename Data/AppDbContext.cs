using CMCS.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Claim> Claims => Set<Claim>();
        public DbSet<ClaimEntry> ClaimEntries => Set<ClaimEntry>();
        public DbSet<Attachment> Attachments => Set<Attachment>();
        public DbSet<Approval> Approvals => Set<Approval>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Claim>()
                .HasMany(c => c.Entries)
                .WithOne()
                .HasForeignKey(e => e.ClaimId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Claim>()
                .HasMany(c => c.Attachments)
                .WithOne()
                .HasForeignKey(a => a.ClaimId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed one demo claim row so the dashboard is not empty
            var demoId = Guid.NewGuid();
            modelBuilder.Entity<Claim>().HasData(new Claim
            {
                Id = demoId,
                LecturerName = "Demo Lecturer",
                Month = DateTime.Now.ToString("dd/MM/yyyy"),
                HourlyRate = 450M,
                TotalHours = 4,
                Status = ClaimStatus.Submitted
            });
        }
    }
}
