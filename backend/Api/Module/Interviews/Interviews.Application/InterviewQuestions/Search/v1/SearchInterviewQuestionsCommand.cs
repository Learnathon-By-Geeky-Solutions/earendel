using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Interviews.Application.InterviewQuestions.Get.v1;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Application.InterviewQuestions.Search.v1;

[ExcludeFromCodeCoverage]
public class SearchInterviewQuestionsCommand : PaginationFilter, IRequest<PagedList<InterviewQuestionResponse>>
{
    public Guid? RubricId { get; set; }
    public Guid? InterviewId { get; set; }
    public string? QuestionText { get; set; }  
}
