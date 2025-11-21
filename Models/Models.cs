namespace CMCS.Web.Models
{
    public enum ClaimStatus
    {
        Draft = 0,
        Submitted = 1,
        Approved = 2,
        Rejected = 3
    }

    public class Claim
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string LecturerName { get; set; } = "Demo Lecturer";
        // Stored as text in dd/MM/yyyy format for simplicity; formatted consistently in the UI
        public string Month { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public decimal HourlyRate { get; set; }
        public double TotalHours { get; set; }
        public decimal TotalAmount => (decimal)TotalHours * HourlyRate;
        public ClaimStatus Status { get; set; } = ClaimStatus.Draft;

        public List<ClaimEntry> Entries { get; set; } = new();
        public List<Attachment> Attachments { get; set; } = new();
    }

    public class ClaimEntry
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ClaimId { get; set; }
        public DateOnly WorkDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
        public string Module { get; set; } = string.Empty;
        public string TaskType { get; set; } = string.Empty;
        public double Hours { get; set; }
        public string? Notes { get; set; }
    }

    public class Attachment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ClaimId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string StoragePath { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }

    public class Approval
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ClaimId { get; set; }
        public string ApproverRole { get; set; } = "PC/AM";
        public string Decision { get; set; } = "Approved";
        public string Comment { get; set; } = string.Empty;
        public DateTime DecidedAt { get; set; } = DateTime.UtcNow;
    }
}
