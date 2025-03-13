using Ardalis.Specification;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Specifications;
using TalentMesh.Module.Job.Application.JobRequiredSubskill.Get.v1;
using TalentMesh.Module.Job.Domain;

namespace TalentMesh.Module.Job.Application.JobRequiredSubskill.Search.v1
{
    public class SearchJobRequiredSubskillSpecs : EntitiesByPaginationFilterSpec<Domain.JobRequiredSubskill, JobRequiredSubskillResponse>
    {
        public SearchJobRequiredSubskillSpecs(SearchJobRequiredSubskillCommand command)
            : base(command)
        {
            Query.OrderBy(x => x.JobId, !command.HasOrderBy());

            if (command.JobId.HasValue)
            {
                Query.Where(x => x.JobId == command.JobId.Value);
            }
            if (command.SubskillId.HasValue)
            {
                Query.Where(x => x.SubskillId == command.SubskillId.Value);
            }
        }
    }
}
