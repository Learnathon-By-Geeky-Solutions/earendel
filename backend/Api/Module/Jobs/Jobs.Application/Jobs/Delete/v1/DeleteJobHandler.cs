using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Job.Domain;
using TalentMesh.Module.Job.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Job.Domain.Extentsion;

namespace TalentMesh.Module.Job.Application.Jobs.Delete.v1;
public sealed class DeleteJobHandler(
    ILogger<DeleteJobHandler> logger,
    [FromKeyedServices("jobs:job")] IRepository<Job.Domain.Jobs> repository)
    : IRequestHandler<DeleteJobCommand>
{
    public async Task Handle(DeleteJobCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var job = await repository.GetByIdAsync(request.Id, cancellationToken) ?? throw new JobNotFoundException(request.Id);
        await repository.DeleteAsync(job!, cancellationToken);
        logger.LogInformation("User with id : {UserId} deleted", job?.Id);
    }
}
