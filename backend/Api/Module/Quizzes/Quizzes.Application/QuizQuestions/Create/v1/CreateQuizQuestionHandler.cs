using TalentMesh.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Quizzes.Application.QuizQuestions.Create.v1;

public sealed class CreateQuizQuestionHandler(
    ILogger<CreateQuizQuestionHandler> logger,
    [FromKeyedServices("quizquestions:quizquestion")] IRepository<Quizzes.Domain.QuizQuestion> repository)
    : IRequestHandler<CreateQuizQuestionCommand, CreateQuizQuestionResponse>
{
    public async Task<CreateQuizQuestionResponse> Handle(CreateQuizQuestionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var quizQuestion = Quizzes.Domain.QuizQuestion.Create(
            request.QuestionText,
            request.Option1,
            request.Option2,
            request.Option3,
            request.Option4,
            request.CorrectOption
        );

        await repository.AddAsync(quizQuestion, cancellationToken);
        logger.LogInformation("Quiz question created {QuizQuestionId}", quizQuestion.Id);

        return new CreateQuizQuestionResponse(quizQuestion.Id);
    }
}
