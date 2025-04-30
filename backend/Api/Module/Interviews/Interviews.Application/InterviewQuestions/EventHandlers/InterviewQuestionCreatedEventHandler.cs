using TalentMesh.Module.Interviews.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Application.InterviewQuestions.EventHandlers;

[ExcludeFromCodeCoverage]
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
