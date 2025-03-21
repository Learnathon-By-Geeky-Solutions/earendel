using TalentMesh.Framework.Core.Exceptions;

namespace TalentMesh.Module.Evaluator.Domain.Exceptions
{
    public sealed class InterviewerAvailabilityNotFoundException : NotFoundException
    {
        public InterviewerAvailabilityNotFoundException(Guid id)
        : base($"InterviewerAvailabilityNotFoundException with id {id} not found")
        {
        }
    }
}
