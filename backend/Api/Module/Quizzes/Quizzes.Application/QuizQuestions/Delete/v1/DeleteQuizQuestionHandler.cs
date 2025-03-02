using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Quizzes.Domain;
using TalentMesh.Module.Quizzes.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Quizzes.Application.QuizQuestions.Delete.v1;
public sealed class DeleteQuizQuestionHandler(
    ILogger<DeleteQuizQuestionHandler> logger,
    [FromKeyedServices("quizquestions:quizquestion")] IRepository<Quizzes.Domain.QuizQuestion> repository)
    : IRequestHandler<DeleteQuizQuestionCommand>
{
    public async Task Handle(DeleteQuizQuestionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var quizQuestion = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (quizQuestion == null || quizQuestion.DeletedBy is not null) throw new QuizQuestionNotFoundException(request.Id);
        await repository.DeleteAsync(quizQuestion, cancellationToken);
        logger.LogInformation("QuizQuestion with id : {QuizQuestionId} deleted", quizQuestion.Id);
    }
}
