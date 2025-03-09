using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Get.v1;
using MediatR;

namespace TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Search.v1;

public class SearchInterviewFeedbacksCommand : PaginationFilter, IRequest<PagedList<InterviewFeedbackResponse>>
{
    public Guid? InterviewId { get; set; }       
    public Guid? InterviewQuestionId { get; set; } 
    public string? Response { get; set; }        
    public decimal? Score { get; set; }
}
