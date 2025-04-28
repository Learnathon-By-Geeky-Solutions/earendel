using Ardalis.Specification;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Specifications;
using TalentMesh.Module.Experties.Application.SubSkills.Get.v1;
using TalentMesh.Module.Experties.Domain;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Application.SubSkills.Search.v1;

[ExcludeFromCodeCoverage]
public class SearchSubSkillSpecs : EntitiesByPaginationFilterSpec<SubSkill, SubSkillResponse>
{
    public SearchSubSkillSpecs(SearchSubSkillsCommand command)
        : base(command)
    {
        // Apply default ordering without branching
        Query.OrderBy(c => c.Name, !command.HasOrderBy());

        // Filter by keyword or skip filtering if keyword is empty
        Query.Where(b =>
            string.IsNullOrEmpty(command.Keyword) || (b.Name != null && b.Name.Contains(command.Keyword))
        );
    }
}
