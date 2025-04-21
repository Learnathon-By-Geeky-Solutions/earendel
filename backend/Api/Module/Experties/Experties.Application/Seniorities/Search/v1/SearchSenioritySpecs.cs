using Ardalis.Specification;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Specifications;
using TalentMesh.Module.Experties.Application.Seniorities.Get.v1;
using TalentMesh.Module.Experties.Domain;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Application.Seniorities.Search.v1;

[ExcludeFromCodeCoverage]
public class SearchSenioritySpecs : EntitiesByPaginationFilterSpec<Seniority, SeniorityResponse>
{
    public SearchSenioritySpecs(SearchSenioritiesCommand command)
        : base(command)
    {
        // Combined filter to eliminate branching and achieve Cognitive Complexity = 0
        Query.Where(b =>
            b.DeletedBy == null
            && (string.IsNullOrEmpty(command.Keyword) || b.Name.Contains(command.Keyword))
        );

        // Apply default ordering without conditional statements
        Query.OrderBy(c => c.Name, !command.HasOrderBy());
    }
}
