using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Quizzes.Domain;
using TalentMesh.Module.Quizzes.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Quizzes.Application.QuizAttempts.Update.v1;
public sealed class UpdateQuizAttemptHandler(
    ILogger<UpdateQuizAttemptHandler> logger,
    [FromKeyedServices("quizattempts:quizattempt")] IRepository<Quizzes.Domain.QuizAttempt> repository)
    : IRequestHandler<UpdateQuizAttemptCommand, UpdateQuizAttemptResponse>
{
    public async Task<UpdateQuizAttemptResponse> Handle(UpdateQuizAttemptCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var quizAttempt = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (quizAttempt is null || quizAttempt.DeletedBy != Guid.Empty)
        {
            throw new QuizAttemptNotFoundException(request.Id);
        }
        var updatedQuiz = quizAttempt.Update(request.UserId, request.Score, request.TotalQuestions);
        await repository.UpdateAsync(updatedQuiz, cancellationToken);
        logger.LogInformation("Quiz with id : {Quiz} updated.", updatedQuiz.Id);
        return new UpdateQuizAttemptResponse(updatedQuiz.Id);
    }
}
