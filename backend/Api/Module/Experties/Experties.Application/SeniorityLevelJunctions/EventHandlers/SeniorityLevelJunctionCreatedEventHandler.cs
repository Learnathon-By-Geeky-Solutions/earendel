using TalentMesh.Module.Experties.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.EventHandlers;

public class SeniorityLevelJunctionCreatedEventHandler(ILogger<SeniorityLevelJunctionCreatedEventHandler> logger) : INotificationHandler<SeniorityLevelJunctionCreated>
{
    public async Task Handle(SeniorityLevelJunctionCreated notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("handling SeniorityLevelJunction created domain event..");
        await Task.FromResult(notification);
        logger.LogInformation("finished handling SeniorityLevelJunction created domain event..");
    }
}
