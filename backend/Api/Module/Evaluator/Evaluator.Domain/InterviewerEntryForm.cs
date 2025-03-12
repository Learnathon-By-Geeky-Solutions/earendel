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

        public static InterviewerEntryForm Create(Guid userId, string? additionalInfo)
        {
            var entryForm = new InterviewerEntryForm
            {
                UserId = userId,
                AdditionalInfo = additionalInfo,
                Status = "pending" // default status
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
    }
}
