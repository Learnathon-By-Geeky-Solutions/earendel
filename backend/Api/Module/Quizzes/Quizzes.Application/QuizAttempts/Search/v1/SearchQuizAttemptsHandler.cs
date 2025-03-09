using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Quizzes.Application.QuizAttempts.Get.v1;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace TalentMesh.Module.Quizzes.Application.QuizAttempts.Search.v1;
public sealed class SearchQuizAttemptsHandler(
    [FromKeyedServices("quizattempts:quizattemptReadOnly")] IReadRepository<Quizzes.Domain.QuizAttempt> repository)
    : IRequestHandler<SearchQuizAttemptsCommand, PagedList<QuizAttemptResponse>>
{
    public async Task<PagedList<QuizAttemptResponse>> Handle(SearchQuizAttemptsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchQuizAttemptSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<QuizAttemptResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}
