using Ardalis.Specification;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Specifications;
using TalentMesh.Module.Quizzes.Application.QuizAttemptAnswers.Get.v1;
using TalentMesh.Module.Quizzes.Domain;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Application.QuizAttemptAnswers.Search.v1;

[ExcludeFromCodeCoverage]
public class SearchQuizAttemptAnswerSpecs : EntitiesByPaginationFilterSpec<Quizzes.Domain.QuizAttemptAnswer, QuizAttemptAnswerResponse>
{
    public SearchQuizAttemptAnswerSpecs(SearchQuizAttemptAnswersCommand command)
        : base(command)
    {
        Query.OrderBy(c => c.AttemptId, !command.HasOrderBy());

        if (command.AttemptId.HasValue)
        {
            Query.Where(b => b.AttemptId == command.AttemptId);
        }
    }
}
