using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Job.Application.JobApplication.Get.v1;
using MediatR;


namespace TalentMesh.Module.Job.Application.JobApplication.Search.v1;

public class SearchJobApplicationsCommand : PaginationFilter, IRequest<PagedList<JobApplicationResponse>>
{
    public int? JobId { get; set; }
    public int? CandidateId { get; set; }
    public DateTime? ApplicationDate { get; set; }
    public string? Status { get; set; }
    

}
