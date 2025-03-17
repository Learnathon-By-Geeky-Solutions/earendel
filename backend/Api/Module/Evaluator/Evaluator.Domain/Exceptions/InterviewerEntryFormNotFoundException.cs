using TalentMesh.Framework.Core.Exceptions;

namespace TalentMesh.Module.Evaluator.Domain.Exceptions
{
    public sealed class InterviewerEntryFormNotFoundException : NotFoundException
    {
        public InterviewerEntryFormNotFoundException(Guid id)
        : base($"InterviewerEntryFormNotFoundException with id {id} not found")
        {
        }
    }
}
