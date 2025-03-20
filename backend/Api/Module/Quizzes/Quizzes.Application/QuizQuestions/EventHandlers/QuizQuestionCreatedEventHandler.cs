using TalentMesh.Module.Quizzes.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Application.QuizQuestions.EventHandlers;

[ExcludeFromCodeCoverage]
public class QuizQuestionCreatedEventHandler(ILogger<QuizQuestionCreatedEventHandler> logger) : INotificationHandler<QuizQuestionCreated>
{
    public async Task Handle(QuizQuestionCreated notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("handling quizquestions created domain event..");
        await Task.FromResult(notification);
        logger.LogInformation("finished handling quizquestion created domain event..");
    }
}
