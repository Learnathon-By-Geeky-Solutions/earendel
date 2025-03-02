using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Module.Quizzes.Domain.Exceptions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using TalentMesh.Module.Quizzes.Domain;
using MediatR;

namespace TalentMesh.Module.Quizzes.Application.QuizAttemptAnswers.Get.v1;

public sealed class GetQuizAttemptAnswerHandler(
    [FromKeyedServices("quizattemptanswers:quizattemptanswerReadOnly")] IReadRepository<QuizAttemptAnswer> repository,
    ICacheService cache)
    : IRequestHandler<GetQuizAttemptAnswerRequest, QuizAttemptAnswerResponse>
{
    public async Task<QuizAttemptAnswerResponse> Handle(GetQuizAttemptAnswerRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync(
            $"QuizAttemptAnswer:{request.Id}",
            async () =>
            {
                var quizAttemptAnswer = await repository.GetByIdAsync(request.Id, cancellationToken);
                if (quizAttemptAnswer == null || quizAttemptAnswer.DeletedBy is not null)
                    throw new QuizAttemptAnswerNotFoundException(request.Id);

                return new QuizAttemptAnswerResponse(
                    quizAttemptAnswer.Id,
                    quizAttemptAnswer.AttemptId,
                    quizAttemptAnswer.QuestionId,
                    quizAttemptAnswer.SelectedOption,
                    quizAttemptAnswer.IsCorrect
                );
            },
            cancellationToken: cancellationToken
        );

        return item!;
    }
}
