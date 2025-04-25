using Ardalis.Specification;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Specifications;
using TalentMesh.Module.Interviews.Application.Interviews.Get.v1;
using TalentMesh.Module.Interviews.Domain;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace TalentMesh.Module.Interviews.Application.Interviews.Search.v1;

[ExcludeFromCodeCoverage]
public class SearchInterviewSpecs : EntitiesByPaginationFilterSpec<Interview, InterviewResponse>
{
    public SearchInterviewSpecs(SearchInterviewsCommand command)
        : base(command)
    {
        Query
            .OrderBy(c => c.Status, !command.HasOrderBy())
            .Where(i => !command.InterviewerId.HasValue || i.InterviewerId == command.InterviewerId.Value)
            .Where(i => string.IsNullOrWhiteSpace(command.Status) || i.Status == command.Status);
    }
}

