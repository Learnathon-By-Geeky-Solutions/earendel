using TalentMesh.Framework.Core.Exceptions;

namespace TalentMesh.Module.User.Domain.Exceptions;
public sealed class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(Guid id)
        : base($"User with id {id} not found")
    {
    }
}
