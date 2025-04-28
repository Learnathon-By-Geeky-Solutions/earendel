using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Job.Domain;
using TalentMesh.Module.Job.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Job.Application.JobRequiredSkill.Update.v1;
using TalentMesh.Module.Job.Domain.Extentsion;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TalentMesh.Module.Job.Application.JobRequiredSkill.Update.v1
{
    public sealed class UpdateJobRequiredSkillHandler(
        ILogger<UpdateJobRequiredSkillHandler> logger,
        [FromKeyedServices("jobs:jobrequiredskill")] IRepository<Domain.JobRequiredSkill> repository)
        : IRequestHandler<UpdateJobRequiredSkillCommand, UpdateJobRequiredSkillResponse>
    {
        public async Task<UpdateJobRequiredSkillResponse> Handle(UpdateJobRequiredSkillCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
                throw new JobRequiredSkillNotFoundException(request.Id);

            // Update the entity – assuming your domain entity implements an Update method.
            var updatedEntity = entity.Update(request.JobId, request.SkillId);
            await repository.UpdateAsync(updatedEntity, cancellationToken);
            logger.LogInformation("JobRequiredSkill with id {Id} updated.", entity.Id);
            return new UpdateJobRequiredSkillResponse(entity.Id);
        }
    }
}
