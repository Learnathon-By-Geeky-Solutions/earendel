using Ardalis.Specification;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Specifications;
using TalentMesh.Module.Interviews.Application.Interviews.Get.v1;
using TalentMesh.Module.Interviews.Domain;

namespace TalentMesh.Module.Interviews.Application.Interviews.Search.v1;

public class SearchInterviewSpecs : EntitiesByPaginationFilterSpec<Interview, InterviewResponse>
{
    public SearchInterviewSpecs(SearchInterviewsCommand command)
        : base(command) =>
        Query
            .OrderBy(c => c.Status, !command.HasOrderBy());
}
