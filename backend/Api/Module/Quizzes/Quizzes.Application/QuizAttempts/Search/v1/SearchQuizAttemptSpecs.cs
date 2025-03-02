using Ardalis.Specification;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Specifications;
using TalentMesh.Module.Quizzes.Application.QuizAttempts.Get.v1;
using TalentMesh.Module.Quizzes.Domain;

namespace TalentMesh.Module.Quizzes.Application.QuizAttempts.Search.v1;

public class SearchQuizAttemptSpecs : EntitiesByPaginationFilterSpec<Quizzes.Domain.QuizAttempt, QuizAttemptResponse>
{
    public SearchQuizAttemptSpecs(SearchQuizAttemptsCommand command)
        : base(command)
    {
        Query.OrderBy(c => c.TotalQuestions, !command.HasOrderBy());

        if (command.TotalQuestions.HasValue)
        {
            Query.Where(b => b.TotalQuestions == command.TotalQuestions.Value);
        }

        if (command.Score.HasValue)
        {
            Query.Where(b => b.Score == command.Score.Value);
        }
    }
}
