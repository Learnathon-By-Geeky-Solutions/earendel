using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Job.Application.Jobs.Get.v1;

namespace TalentMesh.Module.CandidateLogic;

public sealed class JobViewFilters :PaginationFilter, IRequest<PagedList<JobResponse>>
{
    public string? Name { get; set; }
    public string? Location { get; set; }
    public string? JobType { get; set; }
    public string? ExperienceLevel { get; set; }
}

