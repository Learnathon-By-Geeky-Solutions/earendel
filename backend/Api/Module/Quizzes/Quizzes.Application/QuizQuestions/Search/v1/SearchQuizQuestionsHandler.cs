using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Quizzes.Application.QuizQuestions.Get.v1;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Application.QuizQuestions.Search.v1;
[ExcludeFromCodeCoverage]
public sealed class SearchQuizQuestionsHandler(
    [FromKeyedServices("quizquestions:quizquestionReadOnly")] IReadRepository<Quizzes.Domain.QuizQuestion> repository)
    : IRequestHandler<SearchQuizQuestionsCommand, PagedList<QuizQuestionResponse>>
{
    public async Task<PagedList<QuizQuestionResponse>> Handle(SearchQuizQuestionsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchQuizQuestionSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<QuizQuestionResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}
