using TalentMesh.Framework.Core.Domain;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Module.Evaluator.Domain.Events;

namespace TalentMesh.Module.Evaluator.Domain
{
    public class InterviewerApplication : AuditableEntity, IAggregateRoot
    {
        public Guid JobId { get; private set; }
        public Guid InterviewerId { get; private set; }
        public DateTime AppliedDate { get; private set; }
        public string Status { get; private set; } = default!; // e.g., "pending", "approved", "rejected"
        public string? Comments { get; private set; }

        public static InterviewerApplication Create(Guid jobId, Guid interviewerId, string? comments)
        {
            var application = new InterviewerApplication
            {
                JobId = jobId,
                InterviewerId = interviewerId,
                AppliedDate = DateTime.UtcNow,
                Status = "pending",  // default status
                Comments = comments
            };

            application.QueueDomainEvent(new InterviewerApplicationCreated { InterviewerApplication = application });
            return application;
        }

        public InterviewerApplication Update(string? status, string? comments)
        {
            if (status is not null && !string.Equals(Status, status, StringComparison.OrdinalIgnoreCase))
                Status = status;

            if (comments is not null && !string.Equals(Comments, comments, StringComparison.OrdinalIgnoreCase))
                Comments = comments;

            this.QueueDomainEvent(new InterviewerApplicationUpdated { InterviewerApplication = this });
            return this;
        }
    }
}
