using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Domain;
using TalentMesh.Module.Experties.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Update.v1;
namespace TalentMesh.Module.Experties.Application.Skills.Update.v1
{
    public sealed class UpdateSkillHandler(
        ILogger<UpdateSkillHandler> logger,
        [FromKeyedServices("skills:skill")] IRepository<Experties.Domain.Skill> repository,
        IMediator mediator
    ) : IRequestHandler<UpdateSkillCommand, UpdateSkillResponse>
    {
        public async Task<UpdateSkillResponse> Handle(UpdateSkillCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            // Retrieve the existing skill
            var skill = await repository.GetByIdAsync(request.Id, cancellationToken);
            if (skill is null)
            {
                throw new SkillNotFoundException(request.Id);
            }
            // Update the skill
            var updatedSkill = skill.Update(request.Name, request.Description);
            await repository.UpdateAsync(updatedSkill, cancellationToken);
            logger.LogInformation("Skill with ID: {SkillId} updated.", skill.Id);
            // If seniority levels are provided, update the junctions for this skill.
            if (request.SeniorityLevelIds != null && request.SeniorityLevelIds.Any())
            {
                // This command should update the junction records for the given skill.
                var updateJunctionCommand = new UpdateSeniorityLevelJunctionCommand(skill.Id, request.SeniorityLevelIds);
                await mediator.Send(updateJunctionCommand, cancellationToken);
            }
            return new UpdateSkillResponse(skill.Id);
        }
    }
}