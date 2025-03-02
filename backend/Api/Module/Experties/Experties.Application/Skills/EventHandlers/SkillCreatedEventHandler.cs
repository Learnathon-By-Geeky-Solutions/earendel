// using TalentMesh.Module.Experties.Domain.Events;
// using MediatR;
// using Microsoft.Extensions.Logging;

// namespace TalentMesh.Module.Experties.Application.Skills.EventHandlers;

// public class SkillCreatedEventHandler(ILogger<SkillCreatedEventHandler> logger) : INotificationHandler<SkillCreated>
// {
//     public async Task Handle(SkillCreated notification,
//         CancellationToken cancellationToken)
//     {
//         logger.LogInformation("handling Skill created domain event..");
//         await Task.FromResult(notification);
//         logger.LogInformation("finished handling Skill created domain event..");
//     }
// }



using TalentMesh.Module.Experties.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using TalentMesh.Framework.Infrastructure.Messaging;
using TalentMesh.Module.Experties.Domain;

namespace TalentMesh.Module.Experties.Application.Skills.EventHandlers
{
    public class JobCreatedEventHandler : INotificationHandler<SkillCreated>
    {
        private readonly ILogger<JobCreatedEventHandler> _logger;
        private readonly IMessageBus _messageBus;

        public JobCreatedEventHandler(ILogger<JobCreatedEventHandler> logger, IMessageBus messageBus)
        {
            _logger = logger;
            _messageBus = messageBus;
        }

        public async Task Handle(SkillCreated notification, CancellationToken cancellationToken)
        {
            if (notification.Skill is not null)
            {
                _logger.LogInformation("Handling SkillCreated event for Skill ID: {SkillId}", notification.Skill.Id);

                // Prepare a message object with details from the job.
                var skillMessage = new
                {
                    SkillId = notification.Skill.Id,
                    Name = notification.Skill.Name,
                    Description = notification.Skill.Description,
                };

                // Publish the message to a RabbitMQ exchange.
                // Example: Exchange "job.events", Routing Key "job.created"
                await _messageBus.PublishAsync(skillMessage, exchange: "skill.events", routingKey: "skill.created", cancellationToken);

                _logger.LogInformation("Published SkillCreated message for Skill ID: {SkillId}", notification.Skill.Id);
            }
            else
            {
                _logger.LogWarning("JobCreated event received without a valid Job entity.");
            }
        }
    }
}

