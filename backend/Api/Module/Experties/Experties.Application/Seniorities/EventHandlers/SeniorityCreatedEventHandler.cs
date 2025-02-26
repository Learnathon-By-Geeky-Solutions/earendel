using TalentMesh.Module.Experties.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Experties.Application.Seniorities.EventHandlers;

public class SeniorityCreatedEventHandler(ILogger<SeniorityCreatedEventHandler> logger) : INotificationHandler<SeniorityCreated>
{
    public async Task Handle(SeniorityCreated notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("handling Seniority created domain event..");
        await Task.FromResult(notification);
        logger.LogInformation("finished handling Seniority created domain event..");
    }
}
