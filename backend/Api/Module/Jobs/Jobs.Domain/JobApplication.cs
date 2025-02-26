using System.Net.Http.Headers;
using TalentMesh.Framework.Core.Domain;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Module.Job.Domain.Events;


namespace TalentMesh.Module.Job.Domain
{
    public class JobApplication : AuditableEntity, IAggregateRoot
    {
        public int JobId { get;  set; }
        public int CandidateId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Status { get; set; } = default!;  // e.g., "applied", "under review", "accepted", "rejected"
        public string? CoverLetter { get;  set; }

        //public virtual Job.Domain.Jobs Job { get;  set; } = default!;

        /// <summary>
        /// Creates a new job application with the default status set to "applied".
        /// </summary>
        /// <param name="jobId">Identifier of the job posting.</param>
        /// <param name="candidateId">Identifier of the candidate applying.</param>
        /// <param name="coverLetter">Optional cover letter content.</param>
        /// <returns>A new instance of <see cref="JobApplication"/>.</returns>
        public static JobApplication Create(int jobId, int candidateId, string? coverLetter)
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

        /// <summary>
        /// Updates the current job application instance.
        /// </summary>
        /// <param name="status">New status value.</param>
        /// <param name="coverLetter">New cover letter content.</param>
        /// <returns>The updated instance.</returns>
        public JobApplication Update(string? status, string? coverLetter)
        {
            if (status is not null && !string.Equals(Status, status, StringComparison.OrdinalIgnoreCase))
                Status = status;

            if (coverLetter is not null && !string.Equals(CoverLetter, coverLetter, StringComparison.OrdinalIgnoreCase))
                CoverLetter = coverLetter;

            this.QueueDomainEvent(new JobApplicationUpdated() { JobApplication = this });
            return this;
        }

        /// <summary>
        /// Creates an updated instance of a job application with a given identifier.
        /// </summary>
        /// <param name="id">Identifier of the application.</param>
        /// <param name="jobId">Identifier of the job posting.</param>
        /// <param name="candidateId">Identifier of the candidate.</param>
        /// <param name="applicationDate">The date when the application was submitted.</param>
        /// <param name="status">Status of the application.</param>
        /// <param name="coverLetter">Cover letter content.</param>
        /// <returns>A new instance with updated values.</returns>
        public static JobApplication Update(
            int jobId,
            int candidateId,
            DateTime applicationDate,
            string status,
            string? coverLetter)
        {
            var application = new JobApplication
            {
                JobId = jobId,
                CandidateId = candidateId,
                ApplicationDate = applicationDate,
                Status = status,
                CoverLetter = coverLetter
            };

            application.QueueDomainEvent(new JobApplicationUpdated() { JobApplication = application });
            return application;
        }
    }
}
