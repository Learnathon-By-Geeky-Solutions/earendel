using TalentMesh.Framework.Core.Domain;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Module.Evaluator.Domain.Events;

namespace TalentMesh.Module.Evaluator.Domain
{
    public class InterviewerAvailability : AuditableEntity, IAggregateRoot
    {
        public Guid InterviewerId { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public bool IsAvailable { get; private set; } = true;

        public static InterviewerAvailability Create(Guid interviewerId, DateTime startTime, DateTime endTime, bool isAvailable = true)
        {
            var availability = new InterviewerAvailability
            {
                InterviewerId = interviewerId,
                StartTime = startTime,
                EndTime = endTime,
                IsAvailable = isAvailable
            };

            availability.QueueDomainEvent(new InterviewerAvailabilityCreated { InterviewerAvailability = availability });
            return availability;
        }

        public InterviewerAvailability Update(DateTime startTime, DateTime endTime, bool isAvailable)
        {
            if (StartTime != startTime)
                StartTime = startTime;

            if (EndTime != endTime)
                EndTime = endTime;

            if (IsAvailable != isAvailable)
                IsAvailable = isAvailable;

            this.QueueDomainEvent(new InterviewerAvailabilityUpdated { InterviewerAvailability = this });
            return this;
        }
    }
}
