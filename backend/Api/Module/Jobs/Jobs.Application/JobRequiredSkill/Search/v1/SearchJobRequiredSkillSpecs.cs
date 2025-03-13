using Ardalis.Specification;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Specifications;
using TalentMesh.Module.Job.Application.JobRequiredSkill.Get.v1;
using TalentMesh.Module.Job.Domain;

namespace TalentMesh.Module.Job.Application.JobRequiredSkill.Search.v1
{
    public class SearchJobRequiredSkillSpecs : EntitiesByPaginationFilterSpec<Domain.JobRequiredSkill, JobRequiredSkillResponse>
    {
        public SearchJobRequiredSkillSpecs(SearchJobRequiredSkillCommand command)
            : base(command)
        {
            Query.OrderBy(x => x.JobId, !command.HasOrderBy());

            if (command.JobId.HasValue)
            {
                Query.Where(x => x.JobId == command.JobId.Value);
            }
            if (command.SkillId.HasValue)
            {
                Query.Where(x => x.SkillId == command.SkillId.Value);
            }
        }
    }
}
