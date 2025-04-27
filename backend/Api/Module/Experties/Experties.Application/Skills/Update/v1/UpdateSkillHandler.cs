using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Domain;
using TalentMesh.Module.Experties.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Update.v1;

namespace TalentMesh.Module.Experties.Application.Skills.Update.v1
{
    public sealed class UpdateSkillHandler(
        ILogger<UpdateSkillHandler> logger,
        [FromKeyedServices("skills:skill")] IRepository<Skill> repository,
        IMediator mediator
    ) : IRequestHandler<UpdateSkillCommand, UpdateSkillResponse>
    {
        public async Task<UpdateSkillResponse> Handle(UpdateSkillCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            // Retrieve or throw immediately
            var skill = await repository.GetByIdAsync(request.Id, cancellationToken)
                         ?? throw new SkillNotFoundException(request.Id);

            // Update and persist
            var updatedSkill = skill.Update(request.Name, request.Description);
            await repository.UpdateAsync(updatedSkill, cancellationToken);
            logger.LogInformation("Skill with ID: {SkillId} updated.", skill.Id);

            // Always attempt to send junction update (empty list yields no-op)
            var levels = request.SeniorityLevelIds ?? new List<Guid>();
            await mediator.Send(new UpdateSeniorityLevelJunctionCommand(skill.Id, levels), cancellationToken);

            return new UpdateSkillResponse(skill.Id);
        }
    }
}
