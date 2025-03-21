using Ardalis.Specification;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Specifications;
using TalentMesh.Module.Quizzes.Application.QuizQuestions.Get.v1;
using TalentMesh.Module.Quizzes.Domain;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Application.QuizQuestions.Search.v1;

[ExcludeFromCodeCoverage]
public class SearchQuizQuestionSpecs : EntitiesByPaginationFilterSpec<Quizzes.Domain.QuizQuestion, QuizQuestionResponse>
{
    public SearchQuizQuestionSpecs(SearchQuizQuestionsCommand command)
        : base(command)
    {
        Query.OrderBy(c => c.QuestionText, !command.HasOrderBy());

        if (!string.IsNullOrEmpty(command.QuestionText))
        {
            Query.Where(b => b.QuestionText.Contains(command.QuestionText));
        }

    }
}
