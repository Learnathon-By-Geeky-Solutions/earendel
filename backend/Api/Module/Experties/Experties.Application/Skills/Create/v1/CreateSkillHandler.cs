using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentMesh.Framework.Infrastructure.Messaging;
using System.Threading;
using System.Threading.Tasks;

namespace TalentMesh.Module.Experties.Application.Skills.Create.v1
{
    public sealed class CreateSkillHandler(
        ILogger<CreateSkillHandler> logger,
        [FromKeyedServices("skills:skill")] IRepository<Experties.Domain.Skill> repository,
        IMessageBus messageBus)
        : IRequestHandler<CreateSkillCommand, CreateSkillResponse>
    {
        public async Task<CreateSkillResponse> Handle(CreateSkillCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var skill = Experties.Domain.Skill.Create(request.Name!, request.Description);
            await repository.AddAsync(skill, cancellationToken);
            logger.LogInformation("Skill created {SkillId}", skill.Id);

            // Prepare a message payload to publish via RabbitMQ.
            var skillMessage = new
            {
                SkillId = skill.Id,
                Name = skill.Name,
                Description = skill.Description
            };

            // Publish the message to the "skill.events" exchange with "skill.created" routing key.
            await messageBus.PublishAsync(skillMessage, "skill.events.user", "skill.created.user", cancellationToken);

            return new CreateSkillResponse(skill.Id);
        }
    }
}
