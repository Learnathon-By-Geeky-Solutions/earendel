using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Job.Application.Jobs.Get.v1;
using MediatR;

namespace TalentMesh.Module.Job.Application.Jobs.Search.v1;

public class SearchJobsCommand : PaginationFilter, IRequest<PagedList<JobResponse>>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Requirments { get; set; }
    public string? Location { get; set; }
    public string? JobType { get; set; }
    public string? ExperienceLevel { get; set; }
}
