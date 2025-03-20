using TalentMesh.Module.Experties.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Application.Rubrics.EventHandlers;

[ExcludeFromCodeCoverage]
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
