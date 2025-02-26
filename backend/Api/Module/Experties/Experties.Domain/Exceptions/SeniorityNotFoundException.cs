using TalentMesh.Framework.Core.Exceptions;

namespace TalentMesh.Module.Experties.Domain.Exceptions;
public sealed class SeniorityNotFoundException : NotFoundException
{
    public SeniorityNotFoundException(Guid id)
        : base($"Seniority with id {id} not found")
    {
    }
}
