using Ardalis.Specification;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Specifications;
using TalentMesh.Module.Experties.Application.Rubrics.Get.v1;
using TalentMesh.Module.Experties.Domain;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Application.Rubrics.Search.v1;

[ExcludeFromCodeCoverage]
public class SearchRubricSpecs : EntitiesByPaginationFilterSpec<Experties.Domain.Rubric, RubricResponse>
{
    public SearchRubricSpecs(SearchRubricsCommand command)
        : base(command)
    {
        // Only fetch rubrics that are not deleted
        Query.Where(r => r.DeletedBy == null);

        // Filter by keyword in title
        if (!string.IsNullOrEmpty(command.Keyword))
        {
            Query.Where(r => r.Title.Contains(command.Keyword));
        }

        // Filter by SubSkillId
        if (command.SubSkillId.HasValue)
        {
            Query.Where(r => r.SubSkillId == command.SubSkillId);
        }

        // Filter by SeniorityId
        if (command.SeniorityId.HasValue)
        {
            Query.Where(r => r.SeniorityId == command.SeniorityId);
        }

        // Default ordering
        Query.OrderBy(r => r.Title, !command.HasOrderBy());
    }
}
