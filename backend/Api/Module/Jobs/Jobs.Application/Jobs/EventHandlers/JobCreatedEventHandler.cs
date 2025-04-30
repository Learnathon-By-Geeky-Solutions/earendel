using TalentMesh.Module.Job.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Job.Application.Jobs.EventHandlers;

[ExcludeFromCodeCoverage]
public class JobCreatedEventHandler(ILogger<JobCreatedEventHandler> logger) : INotificationHandler<JobCreated>
{
    public async Task Handle(JobCreated notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("handling User created domain event..");
        await Task.FromResult(notification);
        logger.LogInformation("finished handling User created domain event..");
    }
}
