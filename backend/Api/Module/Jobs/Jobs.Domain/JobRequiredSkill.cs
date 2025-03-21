using TalentMesh.Framework.Core.Domain;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Module.Job.Domain.Events;

namespace TalentMesh.Module.Job.Domain
{
    public class JobRequiredSkill : AuditableEntity, IAggregateRoot
    {
        public Guid JobId { get; private set; }
        public Guid SkillId { get; private set; }

        public static JobRequiredSkill Create(Guid jobId, Guid skillId)
        {
            var jobRequiredSkill = new JobRequiredSkill
            {
                JobId = jobId,
                SkillId = skillId
            };

            // Enqueue a domain event (ensure JobRequiredSkillCreated is defined)
            jobRequiredSkill.QueueDomainEvent(new JobRequiredSkillCreated { JobRequiredSkill = jobRequiredSkill });
            return jobRequiredSkill;
        }

        // Optional update method if you need to change the association later.
        public JobRequiredSkill Update(Guid jobId, Guid skillId)
        {
            if (!JobId.Equals(jobId))
            {
                JobId = jobId;
            }
            if (!SkillId.Equals(skillId))
            {
                SkillId = skillId;
            }
            this.QueueDomainEvent(new JobRequiredSkillUpdated { JobRequiredSkill = this });
            return this;
        }
    }
}
