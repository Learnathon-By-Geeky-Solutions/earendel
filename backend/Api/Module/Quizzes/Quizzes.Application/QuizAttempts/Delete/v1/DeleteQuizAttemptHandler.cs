using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Quizzes.Domain;
using TalentMesh.Module.Quizzes.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Quizzes.Application.QuizAttempts.Delete.v1;
public sealed class DeleteQuizAttemptHandler(
    ILogger<DeleteQuizAttemptHandler> logger,
    [FromKeyedServices("quizattempts:quizattempt")] IRepository<Quizzes.Domain.QuizAttempt> repository)
    : IRequestHandler<DeleteQuizAttemptCommand>
{
    public async Task Handle(DeleteQuizAttemptCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var quizAttempt = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (quizAttempt == null || quizAttempt.DeletedBy != Guid.Empty) throw new QuizAttemptNotFoundException(request.Id);
        await repository.DeleteAsync(quizAttempt, cancellationToken);
        logger.LogInformation("QuizAttempt with id : {QuizAttemptId} deleted", quizAttempt.Id);
    }
}
