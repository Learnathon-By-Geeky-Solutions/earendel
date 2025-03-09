using TalentMesh.Framework.Core.Exceptions;

namespace TalentMesh.Module.Interviews.Domain.Exceptions;
public sealed class InterviewNotFoundException : NotFoundException
{
    public InterviewNotFoundException(Guid id)
        : base($"Interview with id {id} not found")
    {
    }
}
