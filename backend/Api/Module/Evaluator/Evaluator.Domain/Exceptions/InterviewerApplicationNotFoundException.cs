using TalentMesh.Framework.Core.Exceptions;

namespace TalentMesh.Module.Evaluator.Domain.Exceptions
{
    public sealed class InterviewerApplicationNotFoundException : NotFoundException
    {
        public InterviewerApplicationNotFoundException(Guid id)
        : base($"InterviewerApplicationNotFoundException with id {id} not found")
        {
        }
    }
}
