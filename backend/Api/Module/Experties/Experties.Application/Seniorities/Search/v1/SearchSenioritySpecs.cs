using Ardalis.Specification;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Specifications;
using TalentMesh.Module.Experties.Application.Seniorities.Get.v1;
using TalentMesh.Module.Experties.Domain;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Application.Seniorities.Search.v1;

[ExcludeFromCodeCoverage]
public class SearchSenioritySpecs : EntitiesByPaginationFilterSpec<Experties.Domain.Seniority, SeniorityResponse>
{
    public SearchSenioritySpecs(SearchSenioritiesCommand command)
        : base(command)
    {
        // Filter out deleted records
        Query.Where(b => b.DeletedBy == null);

        // Filter by keyword in name
        if (!string.IsNullOrEmpty(command.Keyword))
        {
            Query.Where(b => b.Name.Contains(command.Keyword));
        }

        // Apply default ordering if no custom order is specified
        Query.OrderBy(c => c.Name, !command.HasOrderBy());
    }
}
