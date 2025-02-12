using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.User.Domain;
using TalentMesh.Module.User.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.User.Application.Users.Delete.v1;
public sealed class DeleteUserHandler(
    ILogger<DeleteUserHandler> logger,
    [FromKeyedServices("user:users")] IRepository<User.Domain.User> repository)
    : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var user = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = user ?? throw new UserNotFoundException(request.Id);
        await repository.DeleteAsync(user, cancellationToken);
        logger.LogInformation("User with id : {UserId} deleted", user.Id);
    }
}
