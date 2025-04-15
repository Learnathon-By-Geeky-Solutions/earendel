using System;

namespace TalentMesh.Module.HRView.HRFunc   
{
    public class JobApplicationDto
    {
        public Guid Id { get; set; } // JobApplication ID
        public Guid JobId { get; set; }
        public Guid CandidateId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Status { get; set; } = default!;
        public string? CoverLetter { get; set; } // Include if needed
        public DateTimeOffset CreatedOn { get; set; } // From AuditableEntity

        // Optional: Include basic Job details if required
        public string? JobName { get; set; }
    }
}