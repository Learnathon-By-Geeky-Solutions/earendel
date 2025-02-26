using TalentMesh.Framework.Core.Exceptions;

namespace TalentMesh.Module.Experties.Domain.Exceptions;
public sealed class RubricNotFoundException : NotFoundException
{
    public RubricNotFoundException(Guid id)
        : base($"Rubric with id {id} not found")
    {
    }
}
