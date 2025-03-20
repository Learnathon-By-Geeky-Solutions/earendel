using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Quizzes.Application.QuizAttempts.Get.v1;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Application.QuizAttempts.Search.v1;

[ExcludeFromCodeCoverage]
public class SearchQuizAttemptsCommand : PaginationFilter, IRequest<PagedList<QuizAttemptResponse>>
{
    public int? TotalQuestions { get; set; }
    public decimal? Score { get; set; }
}
