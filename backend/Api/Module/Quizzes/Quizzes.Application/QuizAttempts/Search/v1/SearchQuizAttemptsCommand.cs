using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Quizzes.Application.QuizAttempts.Get.v1;
using MediatR;

namespace TalentMesh.Module.Quizzes.Application.QuizAttempts.Search.v1;

public class SearchQuizAttemptsCommand : PaginationFilter, IRequest<PagedList<QuizAttemptResponse>>
{
    public int? TotalQuestions { get; set; }
    public decimal? Score { get; set; }
}
