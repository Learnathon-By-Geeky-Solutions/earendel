using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Quizzes.Domain;
using TalentMesh.Module.Quizzes.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Quizzes.Application.QuizAttemptAnswers.Update.v1;

public sealed class UpdateQuizAttemptAnswerHandler(
    ILogger<UpdateQuizAttemptAnswerHandler> logger,
    [FromKeyedServices("quizattemptanswers:quizattemptanswer")] IRepository<QuizAttemptAnswer> repository)
    : IRequestHandler<UpdateQuizAttemptAnswerCommand, UpdateQuizAttemptAnswerResponse>
{
    public async Task<UpdateQuizAttemptAnswerResponse> Handle(UpdateQuizAttemptAnswerCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Retrieve the quiz attempt answer from the repository using the provided ID.
        var quizAttemptAnswer = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (quizAttemptAnswer is null || quizAttemptAnswer.DeletedBy != Guid.Empty)
        {
            throw new QuizAttemptAnswerNotFoundException(request.Id);
        }

        // Update the quiz attempt answer with the provided fields.
        var updatedQuizAttemptAnswer = quizAttemptAnswer.Update(
            request.AttemptId,
            request.QuestionId,
            request.SelectedOption,
            request.IsCorrect);

        // Save the updated quiz attempt answer back to the repository.
        await repository.UpdateAsync(updatedQuizAttemptAnswer, cancellationToken);

        logger.LogInformation("Quiz attempt answer with ID: {QuizAttemptAnswerId} updated.", updatedQuizAttemptAnswer.Id);

        // Return the response with the updated answer ID.
        return new UpdateQuizAttemptAnswerResponse(updatedQuizAttemptAnswer.Id);
    }
}
