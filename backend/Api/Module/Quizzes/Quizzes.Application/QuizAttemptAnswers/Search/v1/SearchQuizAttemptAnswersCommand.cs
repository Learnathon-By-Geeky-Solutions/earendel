using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Quizzes.Application.QuizAttemptAnswers.Get.v1;
using MediatR;
using System;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Application.QuizAttemptAnswers.Search.v1;

[ExcludeFromCodeCoverage]
public class SearchQuizAttemptAnswersCommand : PaginationFilter, IRequest<PagedList<QuizAttemptAnswerResponse>>
{
    public Guid? AttemptId { get; set; }
}
