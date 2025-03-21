using TalentMesh.Framework.Core.Domain;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Module.Job.Domain.Events;

namespace TalentMesh.Module.Job.Domain
{
    public class JobRequiredSubskill : AuditableEntity, IAggregateRoot
    {
        public Guid JobId { get; private set; }
        public Guid SubskillId { get; private set; }

        public static JobRequiredSubskill Create(Guid jobId, Guid subskillId)
        {
            var jobRequiredSubskill = new JobRequiredSubskill
            {
                JobId = jobId,
                SubskillId = subskillId
            };

            // Enqueue a domain event (ensure JobRequiredSubskillCreated is defined)
            jobRequiredSubskill.QueueDomainEvent(new JobRequiredSubskillCreated { JobRequiredSubskill = jobRequiredSubskill });
            return jobRequiredSubskill;
        }

        // Optional update method if needed.
        public JobRequiredSubskill Update(Guid jobId, Guid subskillId)
        {
            if (!JobId.Equals(jobId))
            {
                JobId = jobId;
            }
            if (!SubskillId.Equals(subskillId))
            {
                SubskillId = subskillId;
            }
            this.QueueDomainEvent(new JobRequiredSubskillUpdated { JobRequiredSubskill = this });
            return this;
        }
    }
}
