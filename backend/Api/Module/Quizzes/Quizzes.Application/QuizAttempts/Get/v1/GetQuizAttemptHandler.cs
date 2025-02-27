using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Module.Quizzes.Domain.Exceptions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using TalentMesh.Module.Quizzes.Domain;
using MediatR;

namespace TalentMesh.Module.Quizzes.Application.QuizAttempts.Get.v1;
public sealed class GetQuizAttemptHandler(
    [FromKeyedServices("quizattempts:quizattemptReadOnly")] IReadRepository<Quizzes.Domain.QuizAttempt> repository,
    ICacheService cache)
    : IRequestHandler<GetQuizAttemptRequest, QuizAttemptResponse>
{
    public async Task<QuizAttemptResponse> Handle(GetQuizAttemptRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"quizattempt:{request.Id}",
            async () =>
            {
                var quizItem = await repository.GetByIdAsync(request.Id, cancellationToken);
                if (quizItem == null || quizItem.DeletedBy != Guid.Empty) throw new QuizAttemptNotFoundException(request.Id);
                return new QuizAttemptResponse(quizItem.Id, quizItem.UserId, quizItem.Score, quizItem.TotalQuestions);
            },
            cancellationToken: cancellationToken);
        return item!;
    }
}
