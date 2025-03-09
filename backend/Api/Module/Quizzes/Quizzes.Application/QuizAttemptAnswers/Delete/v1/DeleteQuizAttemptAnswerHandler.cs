using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Quizzes.Domain;
using TalentMesh.Module.Quizzes.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Quizzes.Application.QuizAttemptAnswers.Delete.v1;
public sealed class DeleteQuizAttemptAnswerHandler(
    ILogger<DeleteQuizAttemptAnswerHandler> logger,
    [FromKeyedServices("quizattemptanswers:quizattemptanswer")] IRepository<Quizzes.Domain.QuizAttemptAnswer> repository)
    : IRequestHandler<DeleteQuizAttemptAnswerCommand>
{
    public async Task Handle(DeleteQuizAttemptAnswerCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var QuizAttemptAnswer = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (QuizAttemptAnswer == null || QuizAttemptAnswer.DeletedBy != Guid.Empty) throw new QuizAttemptAnswerNotFoundException(request.Id);
        await repository.DeleteAsync(QuizAttemptAnswer, cancellationToken);
        logger.LogInformation("QuizAttemptAnswer with id : {QuizAttemptAnswerId} deleted", QuizAttemptAnswer.Id);
    }
}
