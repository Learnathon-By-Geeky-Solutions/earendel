using TalentMesh.Framework.Core.Exceptions;

namespace TalentMesh.Module.Experties.Domain.Exceptions;
public sealed class SkillNotFoundException : NotFoundException
{
    public SkillNotFoundException(Guid id)
        : base($"Skill with id {id} not found")
    {
    }
}
