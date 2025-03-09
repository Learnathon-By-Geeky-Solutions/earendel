using MediatR;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Quizzes.Domain.Events;

namespace TalentMesh.Module.Quizzes.Application.QuizAttemptAnswers.EventHandlers;

public class QuizAttemptAnswerCreatedEventHandler(ILogger<QuizAttemptAnswerCreatedEventHandler> logger) : INotificationHandler<QuizAttemptAnswerCreated>
{
    public async Task Handle(QuizAttemptAnswerCreated notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("handling QuizAttemptAnswers created domain event..");
        await Task.FromResult(notification);
        logger.LogInformation("finished handling QuizAttemptAnswer created domain event..");
    }
}
