using TalentMesh.Framework.Core.Exceptions;

namespace TalentMesh.Module.Interviews.Domain.Exceptions;
public sealed class InterviewFeedbackNotFoundException : NotFoundException
{
    public InterviewFeedbackNotFoundException(Guid id)
        : base($"Interview feedback with id {id} not found")
    {
    }
}
