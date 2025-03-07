using TalentMesh.Module.Interviews.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Interviews.Application.InterviewQuestions.EventHandlers;

public class InterviewQuestionCreatedEventHandler(ILogger<InterviewQuestionCreatedEventHandler> logger) : INotificationHandler<InterviewQuestionCreated>
{
    public async Task Handle(InterviewQuestionCreated interview,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("handling Interview Question created domain event..");
        await Task.FromResult(interview);
        logger.LogInformation("finished handling Interview Question created domain event..");
    }
}
