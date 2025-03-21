using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Quizzes.Domain;
using TalentMesh.Module.Quizzes.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Quizzes.Application.QuizQuestions.Update.v1;

public sealed class UpdateQuizQuestionHandler(
    ILogger<UpdateQuizQuestionHandler> logger,
    [FromKeyedServices("quizquestions:quizquestion")] IRepository<QuizQuestion> repository)
    : IRequestHandler<UpdateQuizQuestionCommand, UpdateQuizQuestionResponse>
{
    public async Task<UpdateQuizQuestionResponse> Handle(UpdateQuizQuestionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var quizQuestion = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (quizQuestion == null)
        {
            throw new QuizQuestionNotFoundException(request.Id);
        }

        // Update the quiz question using the command parameters.
        var updatedQuizQuestion = quizQuestion.Update(
            request.QuestionText,
            request.CorrectOption);

        await repository.UpdateAsync(updatedQuizQuestion, cancellationToken);

        logger.LogInformation("Quiz question with id : {QuizQuestionId} updated.", updatedQuizQuestion.Id);

        return new UpdateQuizQuestionResponse(updatedQuizQuestion.Id);
    }
}
