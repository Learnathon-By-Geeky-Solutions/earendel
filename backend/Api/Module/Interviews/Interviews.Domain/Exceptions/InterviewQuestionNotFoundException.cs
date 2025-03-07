using TalentMesh.Framework.Core.Exceptions;

namespace TalentMesh.Module.Interviews.Domain.Exceptions;
public sealed class InterviewQuestionNotFoundException : NotFoundException
{
    public InterviewQuestionNotFoundException(Guid id)
        : base($"Interview question with id {id} not found")
    {
    }
}
