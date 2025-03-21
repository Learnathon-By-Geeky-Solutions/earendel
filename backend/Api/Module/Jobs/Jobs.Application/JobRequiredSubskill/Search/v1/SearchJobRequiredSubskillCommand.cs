using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Job.Application.JobRequiredSubskill.Get.v1;
using MediatR;
using System;

namespace TalentMesh.Module.Job.Application.JobRequiredSubskill.Search.v1
{
    public class SearchJobRequiredSubskillCommand : PaginationFilter, IRequest<PagedList<JobRequiredSubskillResponse>>
    {
        public Guid? JobId { get; set; }
        public Guid? SubskillId { get; set; }
    }
}
