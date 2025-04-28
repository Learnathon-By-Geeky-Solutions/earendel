using System;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Domain;
using TalentMesh.Module.Experties.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace TalentMesh.Module.Experties.Application.SubSkills.Update.v1
{
    public sealed class UpdateSubSkillHandler(
        ILogger<UpdateSubSkillHandler> logger,
        [FromKeyedServices("subskills:subskill")] IRepository<SubSkill> repository)
        : IRequestHandler<UpdateSubSkillCommand, UpdateSubSkillResponse>
    {
        public async Task<UpdateSubSkillResponse> Handle(UpdateSubSkillCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            // Retrieve or throw immediately (no branching)
            var subSkill = await repository.GetByIdAsync(request.Id, cancellationToken)
                           ?? throw new SubSkillNotFoundException(request.Id);

            // Update and persist
            var updated = subSkill.Update(request.Name, request.Description, request.SkillId);
            await repository.UpdateAsync(updated, cancellationToken);

            logger.LogInformation("SubSkill with ID: {SubSkillId} updated.", updated.Id);
            return new UpdateSubSkillResponse(updated.Id);
        }
    }
}
