using TalentMesh.Module.Experties.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Application.Seniorities.EventHandlers;

[ExcludeFromCodeCoverage]
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
