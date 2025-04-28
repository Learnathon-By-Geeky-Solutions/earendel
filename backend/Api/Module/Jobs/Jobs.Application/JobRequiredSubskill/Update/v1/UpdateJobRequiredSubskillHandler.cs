using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Job.Domain;
using TalentMesh.Module.Job.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Job.Application.JobRequiredSubskill.Update.v1;
using TalentMesh.Module.Job.Domain.Extentsion;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TalentMesh.Module.Job.Application.JobRequiredSubskill.Update.v1
{
    public sealed class UpdateJobRequiredSubskillHandler(
        ILogger<UpdateJobRequiredSubskillHandler> logger,
        [FromKeyedServices("jobs:jobrequiredsubskill")] IRepository<Domain.JobRequiredSubskill> repository)
        : IRequestHandler<UpdateJobRequiredSubskillCommand, UpdateJobRequiredSubskillResponse>
    {
        public async Task<UpdateJobRequiredSubskillResponse> Handle(UpdateJobRequiredSubskillCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var entity = await repository.GetByIdAsync(request.Id, cancellationToken) ?? throw new JobRequiredSubskillNotFoundException(request.Id);

            // Update the entity – assuming your domain entity implements an Update method.
            var updatedEntity = entity.Update(request.JobId, request.SubskillId);
            await repository.UpdateAsync(updatedEntity, cancellationToken);
            logger.LogInformation("JobRequiredSubskill with id {Id} updated.", entity.Id);
            return new UpdateJobRequiredSubskillResponse(entity.Id);
        }
    }
}
