using TalentMesh.Module.Experties.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Experties.Application.Rubrics.EventHandlers;

public class RubricCreatedEventHandler(ILogger<RubricCreatedEventHandler> logger) : INotificationHandler<RubricCreated>
{
    public async Task Handle(RubricCreated notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("handling rubric created domain event..");
        await Task.FromResult(notification);
        logger.LogInformation("finished handling rubric created domain event..");
    }
}
