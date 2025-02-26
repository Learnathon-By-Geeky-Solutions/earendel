using Ardalis.Specification;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Specifications;
using TalentMesh.Module.Experties.Application.Seniorities.Get.v1;
using TalentMesh.Module.Experties.Domain;

namespace TalentMesh.Module.Experties.Application.Seniorities.Search.v1;
public class SearchSenioritySpecs : EntitiesByPaginationFilterSpec<Experties.Domain.Seniority, SeniorityResponse>
{
    public SearchSenioritySpecs(SearchSenioritiesCommand command)
        : base(command) =>
        Query
            .OrderBy(c => c.Name, !command.HasOrderBy())
            .Where(b => b.Name.Contains(command.Keyword), !string.IsNullOrEmpty(command.Keyword));
}
