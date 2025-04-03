using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentMesh.Framework.Infrastructure.Messaging;
using System.Threading;
using System.Threading.Tasks;
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Create.v1;

namespace TalentMesh.Module.Experties.Application.Skills.Create.v1
{
    public sealed class CreateSkillHandler(
        ILogger<CreateSkillHandler> logger,
        [FromKeyedServices("skills:skill")] IRepository<Experties.Domain.Skill> repository,
        IMessageBus messageBus,
        IMediator mediator) // Add IMediator dependency
        : IRequestHandler<CreateSkillCommand, CreateSkillResponse>
    {
        public async Task<CreateSkillResponse> Handle(CreateSkillCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var skill = Experties.Domain.Skill.Create(request.Name!, request.Description);
            await repository.AddAsync(skill, cancellationToken);
            logger.LogInformation("Skill created {SkillId}", skill.Id);

            // Publish skill creation event
            var skillMessage = new
            {
                SkillId = skill.Id,
                Name = skill.Name,
                Description = skill.Description
            };

            await messageBus.PublishAsync(skillMessage, "skill.events.user", "skill.created.user", cancellationToken);

            // **Trigger CreateSeniorityLevelJunctionHandler**
            if (request.SeniorityLevels?.Any() == true)
            {
                var createJunctionCommand = new CreateSeniorityLevelJunctionCommand(skill.Id, request.SeniorityLevels);
                await mediator.Send(createJunctionCommand, cancellationToken);
            }

            return new CreateSkillResponse(skill.Id);
        }
    }
}
