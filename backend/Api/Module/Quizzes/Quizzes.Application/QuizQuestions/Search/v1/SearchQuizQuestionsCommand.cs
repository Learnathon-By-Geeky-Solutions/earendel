using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Quizzes.Application.QuizQuestions.Get.v1;
using MediatR;

namespace TalentMesh.Module.Quizzes.Application.QuizQuestions.Search.v1;

public class SearchQuizQuestionsCommand : PaginationFilter, IRequest<PagedList<QuizQuestionResponse>>
{
    public string? QuestionText { get; set; }
}
