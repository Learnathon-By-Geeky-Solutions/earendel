using TalentMesh.Module.Experties.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Experties.Application.Skills.EventHandlers;

public class SkillCreatedEventHandler(ILogger<SkillCreatedEventHandler> logger) : INotificationHandler<SkillCreated>
{
    public async Task Handle(SkillCreated notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("handling Skill created domain event..");
        await Task.FromResult(notification);
        logger.LogInformation("finished handling Skill created domain event..");
    }
}
