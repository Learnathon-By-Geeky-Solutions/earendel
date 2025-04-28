using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Get.v1;
using MediatR;
using System.Diagnostics.CodeAnalysis;


namespace TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Search.v1;

[ExcludeFromCodeCoverage]
public class SearchInterviewFeedbacksCommand : PaginationFilter, IRequest<PagedList<InterviewFeedbackResponse>>
{
    public Guid? InterviewId { get; set; }       
    public string? InterviewQuestionText { get; set; } 
    public string? Response { get; set; }        
    public decimal? Score { get; set; }
}
