using TalentMesh.Framework.Core.Domain;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Module.Job.Domain.Events;

namespace TalentMesh.Module.Job.Domain
{
    public class JobApplication : AuditableEntity, IAggregateRoot
    {
        public Guid JobId { get; set; }
        public Guid CandidateId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Status { get; set; } = default!;  // e.g., "applied", "under review", "accepted", "rejected"
        public string? CoverLetter { get; set; }
        public virtual Jobs Job { get; set; } = default!;

        
        public static JobApplication Create(Guid jobId, Guid candidateId, string? coverLetter)
        {
            var application = new JobApplication
            {
                JobId = jobId,
                CandidateId = candidateId,
                ApplicationDate = DateTime.UtcNow,
                Status = "applied",
                CoverLetter = coverLetter
            };

            application.QueueDomainEvent(new JobApplicationCreated() { JobApplication = application });
            return application;
        }

        /* @brief Updates the status and/or cover letter of the job application.
         * @param status The new status of the job application.
         * @param coverLetter The new cover letter content.
         */
        public JobApplication Update(string? status, string? coverLetter)
        {
            if (status is not null && !string.Equals(Status, status, StringComparison.OrdinalIgnoreCase))
                Status = status;

            if (coverLetter is not null && !string.Equals(CoverLetter, coverLetter, StringComparison.OrdinalIgnoreCase))
                CoverLetter = coverLetter;

            this.QueueDomainEvent(new JobApplicationUpdated() { JobApplication = this });
            return this;
        }

     

    }
}
