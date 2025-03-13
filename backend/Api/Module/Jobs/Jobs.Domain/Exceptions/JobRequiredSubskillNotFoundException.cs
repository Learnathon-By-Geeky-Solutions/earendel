using TalentMesh.Framework.Core.Exceptions;

namespace TalentMesh.Module.Job.Domain.Exceptions;
public sealed class JobRequiredSubskillNotFoundException : NotFoundException
{
    public JobRequiredSubskillNotFoundException(Guid id)
        : base($"Job with id {id} not found")
    {
    }
}
