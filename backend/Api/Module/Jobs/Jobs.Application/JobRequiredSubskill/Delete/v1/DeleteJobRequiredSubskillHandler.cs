using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Job.Domain;
using TalentMesh.Module.Job.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Job.Domain.Extentsion;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TalentMesh.Module.Job.Application.JobRequiredSubskill.Delete.v1
{
    public sealed class DeleteJobRequiredSubskillHandler(
        ILogger<DeleteJobRequiredSubskillHandler> logger,
        [FromKeyedServices("jobs:jobrequiredsubskill")] IRepository<Domain.JobRequiredSubskill> repository)
        : IRequestHandler<DeleteJobRequiredSubskillCommand>
    {
        public async Task Handle(DeleteJobRequiredSubskillCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var entity = await repository.GetByIdAsync(request.Id, cancellationToken) ?? throw new JobRequiredSubskillNotFoundException(request.Id);
            
            await repository.DeleteAsync(entity!, cancellationToken);
            logger.LogInformation("JobRequiredSubskill with id {Id} deleted", entity.Id);
        }
    }
}
