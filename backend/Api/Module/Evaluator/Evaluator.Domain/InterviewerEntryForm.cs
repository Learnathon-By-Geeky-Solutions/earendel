using TalentMesh.Framework.Core.Domain;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Module.Evaluator.Domain.Events;

namespace TalentMesh.Module.Evaluator.Domain
{
    public class InterviewerEntryForm : AuditableEntity, IAggregateRoot
    {
        public Guid UserId { get; private set; }
        public string? AdditionalInfo { get; private set; }
        public string Status { get; private set; } = default!;  // e.g., "pending", "approved", "rejected"
        public string? CV { get; private set; } = default!; // URL or path to the CV file
        public static InterviewerEntryForm Create(Guid userId, string? additionalInfo, string? cv = default)
        {
            var entryForm = new InterviewerEntryForm
            {
                UserId = userId,
                AdditionalInfo = additionalInfo,
                Status = "pending" ,// default status
                CV = cv
            };

            entryForm.QueueDomainEvent(new InterviewerEntryFormCreated { InterviewerEntryForm = entryForm });
            return entryForm;
        }

        public InterviewerEntryForm Update(string? additionalInfo, string? status)
        {
            if (additionalInfo is not null && !string.Equals(AdditionalInfo, additionalInfo, StringComparison.OrdinalIgnoreCase))
                AdditionalInfo = additionalInfo;

            if (status is not null && !string.Equals(Status, status, StringComparison.OrdinalIgnoreCase))
                Status = status;

            this.QueueDomainEvent(new InterviewerEntryFormUpdated { InterviewerEntryForm = this });
            return this;
        }

        public InterviewerEntryForm UploadCv(string cvPath)
        {
            CV = cvPath;
            this.QueueDomainEvent(new InterviewerEntryFormUpdated { InterviewerEntryForm = this });
            return this;
        }

        public void Approve()
        {
            Status = "approved";
            this.QueueDomainEvent(new InterviewerEntryFormUpdated { InterviewerEntryForm = this });
        }
    }
}
