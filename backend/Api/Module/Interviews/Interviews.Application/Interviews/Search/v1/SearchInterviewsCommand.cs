using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Interviews.Application.Interviews.Get.v1;
using MediatR;

namespace TalentMesh.Module.Interviews.Application.Interviews.Search.v1;

public class SearchInterviewsCommand : PaginationFilter, IRequest<PagedList<InterviewResponse>>
{
    public Guid? ApplicationId { get; set; }  // Search by ApplicationId
    public Guid? InterviewerId { get; set; }  // Search by InterviewerId
    public DateTime? InterviewDate { get; set; }  // Search by InterviewDate
    public string? Status { get; set; }  // Search by Status
    public string? Notes { get; set; }  // Search by Notes
    public string? MeetingId { get; set; }  // Search by MeetingId
}
