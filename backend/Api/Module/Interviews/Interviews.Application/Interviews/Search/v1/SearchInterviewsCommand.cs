using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Interviews.Application.Interviews.Get.v1;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Application.Interviews.Search.v1;

[ExcludeFromCodeCoverage]
public class SearchInterviewsCommand : PaginationFilter, IRequest<PagedList<InterviewResponse>>
{
    public Guid? ApplicationId { get; set; }  // Search by ApplicationId
    public Guid? InterviewerId { get; set; }  // Search by InterviewerId
    public Guid? CandidateId { get; set; }  // Search by CandidateId
    public Guid? JobId { get; set; }  // Search by JobId
    public DateTime? InterviewDate { get; set; }  // Search by InterviewDate
    public string? Status { get; set; }  // Search by Status
    public string? Notes { get; set; }  // Search by Notes
    public string? MeetingId { get; set; }  // Search by MeetingId
}
