using TalentMesh.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Quizzes.Domain;

namespace TalentMesh.Module.Quizzes.Application.QuizAttemptAnswers.Create.v1;

public sealed class CreateQuizAttemptAnswerHandler(
    ILogger<CreateQuizAttemptAnswerHandler> logger,
    [FromKeyedServices("quizattemptanswers:quizattemptanswer")] IRepository<QuizAttemptAnswer> repository)
    : IRequestHandler<CreateQuizAttemptAnswerCommand, CreateQuizAttemptAnswerResponse>
{
    public async Task<CreateQuizAttemptAnswerResponse> Handle(CreateQuizAttemptAnswerCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var quizAttemptAnswer = QuizAttemptAnswer.Create(
            request.AttemptId,
            request.QuestionId,
            request.SelectedOption,
            request.IsCorrect
        );

        await repository.AddAsync(quizAttemptAnswer, cancellationToken);
        logger.LogInformation("Quiz attempt answer created with ID: {QuizAttemptAnswerId}", quizAttemptAnswer.Id);

        return new CreateQuizAttemptAnswerResponse(quizAttemptAnswer.Id);
    }
}
