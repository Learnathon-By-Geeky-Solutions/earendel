using TalentMesh.Module.Interviews.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Interviews.Application.Interviews.EventHandlers;

public class InterviewCreatedEventHandler(ILogger<InterviewCreatedEventHandler> logger) : INotificationHandler<InterviewCreated>
{
    public async Task Handle(InterviewCreated interview,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("handling Interview created domain event..");
        await Task.FromResult(interview);
        logger.LogInformation("finished handling Interview created domain event..");
    }
}
