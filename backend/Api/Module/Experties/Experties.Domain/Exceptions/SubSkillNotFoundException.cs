using TalentMesh.Framework.Core.Exceptions;

namespace TalentMesh.Module.Experties.Domain.Exceptions;
public sealed class SubSkillNotFoundException : NotFoundException
{
    public SubSkillNotFoundException(Guid id)
        : base($"SubSkill with id {id} not found")
    {
    }
}
