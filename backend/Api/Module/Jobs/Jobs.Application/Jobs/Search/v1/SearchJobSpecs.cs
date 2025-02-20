using Ardalis.Specification;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Specifications;
using TalentMesh.Module.Job.Application.Jobs.Get.v1;
using TalentMesh.Module.Job.Domain;

namespace TalentMesh.Module.Job.Application.Jobs.Search.v1;
public class SearchJobSpecs : EntitiesByPaginationFilterSpec<Job.Domain.Jobs, JobResponse>
{
    public SearchJobSpecs(SearchJobsCommand command)
        : base(command) =>
        Query
            .OrderBy(c => c.Name, !command.HasOrderBy())
            .Where(b => b.Name.Contains(command.Keyword), !string.IsNullOrEmpty(command.Keyword));
}
