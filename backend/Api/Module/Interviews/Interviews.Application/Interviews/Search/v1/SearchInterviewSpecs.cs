using Ardalis.Specification;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Specifications;
using TalentMesh.Module.Interviews.Application.Interviews.Get.v1;
using TalentMesh.Module.Interviews.Domain;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Application.Interviews.Search.v1;

[ExcludeFromCodeCoverage]
public class SearchInterviewSpecs : EntitiesByPaginationFilterSpec<Interview, InterviewResponse>
{
    public SearchInterviewSpecs(SearchInterviewsCommand command)
        : base(command) =>
        Query
            .OrderBy(c => c.Status, !command.HasOrderBy());
}
