using Ardalis.Specification;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Specifications;
using TalentMesh.Module.Experties.Application.Rubrics.Get.v1;
using TalentMesh.Module.Experties.Domain;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Application.Rubrics.Search.v1;

[ExcludeFromCodeCoverage]
public class SearchRubricSpecs : EntitiesByPaginationFilterSpec<Rubric, RubricResponse>
{
    public SearchRubricSpecs(SearchRubricsCommand command)
        : base(command)
    {

        // Filter out deleted records
        Query.Where(r => r.DeletedBy == null);

        // Filter by keyword (no branching)
        Query.Where(r => string.IsNullOrEmpty(command.Keyword) || r.Title.Contains(command.Keyword));

        // Filter by SubSkillId (no branching)
        Query.Where(r => !command.SubSkillId.HasValue || r.SubSkillId == command.SubSkillId);

        // Filter by SeniorityId (no branching)
        Query.Where(r => !command.SeniorityId.HasValue || r.SeniorityId == command.SeniorityId);

        // Apply ordering without conditional statements
        Query.OrderBy(r => r.Title, !command.HasOrderBy());
    }
}
