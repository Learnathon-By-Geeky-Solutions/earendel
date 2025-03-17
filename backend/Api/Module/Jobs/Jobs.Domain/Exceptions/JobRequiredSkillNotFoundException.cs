using TalentMesh.Framework.Core.Exceptions;

namespace TalentMesh.Module.Job.Domain.Exceptions;
public sealed class JobRequiredSkillNotFoundException : NotFoundException
{
    public JobRequiredSkillNotFoundException(Guid id)
        : base($"Job with id {id} not found")
    {
    }
}
