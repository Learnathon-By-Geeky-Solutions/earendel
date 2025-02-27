using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Quizzes.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Quizzes.Application.QuizAttempts.Create.v1;
public sealed class CreateQuizAttemptHandler(
    ILogger<CreateQuizAttemptHandler> logger,
    [FromKeyedServices("quizattempts:quizattempt")] IRepository<Quizzes.Domain.QuizAttempt> repository)
    : IRequestHandler<CreateQuizAttemptCommand, CreateQuizAttemptResponse>
{
    public async Task<CreateQuizAttemptResponse> Handle(CreateQuizAttemptCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var quizAttempt = Quizzes.Domain.QuizAttempt.Create(request.UserId, request.Score, request.TotalQuestions);
        await repository.AddAsync(quizAttempt, cancellationToken);
        logger.LogInformation("quizAttempt created {QuizAttemptId}", quizAttempt.Id);
        return new CreateQuizAttemptResponse(quizAttempt.Id);
    }
}
