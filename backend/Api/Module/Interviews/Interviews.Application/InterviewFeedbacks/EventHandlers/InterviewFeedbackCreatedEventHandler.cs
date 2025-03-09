using TalentMesh.Module.Interviews.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Interviews.Application.Interviews.EventHandlers;

public class InterviewFeedbackCreatedEventHandler(ILogger<InterviewFeedbackCreatedEventHandler> logger) : INotificationHandler<InterviewFeedbackCreated>
{
    public async Task Handle(InterviewFeedbackCreated interview,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("handling Interview feedback created domain event..");
        await Task.FromResult(interview);
        logger.LogInformation("finished handling Interview feedback created domain event..");
    }
}
