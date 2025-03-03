using TalentMesh.Framework.Core.Exceptions;

namespace TalentMesh.Module.Experties.Domain.Exceptions;
public sealed class SeniorityLevelJunctionNotFoundException : NotFoundException
{
    public SeniorityLevelJunctionNotFoundException(Guid id)
        : base($"SeniorityLevelJunction with id {id} not found")
    {
    }
}
