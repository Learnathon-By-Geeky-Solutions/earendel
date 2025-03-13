using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Job.Application.JobRequiredSkill.Get.v1;
using MediatR;
using System;

namespace TalentMesh.Module.Job.Application.JobRequiredSkill.Search.v1
{
    public class SearchJobRequiredSkillCommand : PaginationFilter, IRequest<PagedList<JobRequiredSkillResponse>>
    {
        public Guid? JobId { get; set; }
        public Guid? SkillId { get; set; }
    }
}
