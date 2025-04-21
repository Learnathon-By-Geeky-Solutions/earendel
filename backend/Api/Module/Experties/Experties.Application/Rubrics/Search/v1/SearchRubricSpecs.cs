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
        // Combined filters into a single expression to eliminate branching
        Query.Where(r =>
            r.DeletedBy == null
            && (string.IsNullOrEmpty(command.Keyword) || r.Title.Contains(command.Keyword))
            && (!command.SubSkillId.HasValue || r.SubSkillId == command.SubSkillId)
            && (!command.SeniorityId.HasValue || r.SeniorityId == command.SeniorityId)
        );

        // Default ordering without conditional statements
        Query.OrderBy(r => r.Title, !command.HasOrderBy());
    }
}
