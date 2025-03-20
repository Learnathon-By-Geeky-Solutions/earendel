using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Quizzes.Application.QuizAttemptAnswers.Get.v1;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Application.QuizAttemptAnswers.Search.v1;
[ExcludeFromCodeCoverage]
public sealed class SearchQuizAttemptAnswersHandler(
    [FromKeyedServices("quizattemptanswers:quizattemptanswerReadOnly")] IReadRepository<Quizzes.Domain.QuizAttemptAnswer> repository)
    : IRequestHandler<SearchQuizAttemptAnswersCommand, PagedList<QuizAttemptAnswerResponse>>
{
    public async Task<PagedList<QuizAttemptAnswerResponse>> Handle(SearchQuizAttemptAnswersCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchQuizAttemptAnswerSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<QuizAttemptAnswerResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}
