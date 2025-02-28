using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Module.Quizzes.Domain.Exceptions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using TalentMesh.Module.Quizzes.Domain;
using MediatR;

namespace TalentMesh.Module.Quizzes.Application.QuizQuestions.Get.v1;

public sealed class GetQuizQuestionHandler(
    [FromKeyedServices("quizquestions:quizquestionReadOnly")] IReadRepository<Quizzes.Domain.QuizQuestion> repository,
    ICacheService cache)
    : IRequestHandler<GetQuizQuestionRequest, QuizQuestionResponse>
{
    public async Task<QuizQuestionResponse> Handle(GetQuizQuestionRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync(
            $"quizquestion:{request.Id}",
            async () =>
            {
                var quizQuestion = await repository.GetByIdAsync(request.Id, cancellationToken);
                if (quizQuestion == null || quizQuestion.DeletedBy != Guid.Empty)
                    throw new QuizQuestionNotFoundException(request.Id);

                return new QuizQuestionResponse(
                    quizQuestion.Id,
                    quizQuestion.QuestionText,
                    quizQuestion.Option1,
                    quizQuestion.Option2,
                    quizQuestion.Option3,
                    quizQuestion.Option4,
                    quizQuestion.CorrectOption
                );
            },
            cancellationToken: cancellationToken
        );

        return item!;
    }
}
