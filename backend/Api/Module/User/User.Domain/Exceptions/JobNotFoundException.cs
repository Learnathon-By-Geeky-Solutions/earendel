using TalentMesh.Framework.Core.Exceptions;

namespace TalentMesh.Module.Job.Domain.Exceptions;
public sealed class JobNotFoundException : NotFoundException
{
    public JobNotFoundException(Guid id)
        : base($"User with id {id} not found")
    {
    }
}
