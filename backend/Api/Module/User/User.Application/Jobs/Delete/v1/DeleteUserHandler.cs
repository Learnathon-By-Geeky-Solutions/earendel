using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Job.Domain;
using TalentMesh.Module.Job.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Job.Application.Jobs.Delete.v1;
public sealed class DeleteUserHandler(
    ILogger<DeleteUserHandler> logger,
    [FromKeyedServices("user:users")] IRepository<Job.Domain.Jobs> repository)
    : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var user = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = user ?? throw new JobNotFoundException(request.Id);
        await repository.DeleteAsync(user, cancellationToken);
        logger.LogInformation("User with id : {UserId} deleted", user.Id);
    }
}
