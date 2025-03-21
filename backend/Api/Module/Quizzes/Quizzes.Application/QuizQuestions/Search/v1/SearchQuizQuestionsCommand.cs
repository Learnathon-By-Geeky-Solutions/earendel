using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Quizzes.Application.QuizQuestions.Get.v1;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Application.QuizQuestions.Search.v1;

[ExcludeFromCodeCoverage]
public class SearchQuizQuestionsCommand : PaginationFilter, IRequest<PagedList<QuizQuestionResponse>>
{
    public string? QuestionText { get; set; }
}
