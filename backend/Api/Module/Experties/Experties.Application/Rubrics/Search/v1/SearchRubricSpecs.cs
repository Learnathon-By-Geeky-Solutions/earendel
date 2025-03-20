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
        : base(command) =>
        Query
            .OrderBy(c => c.Title, !command.HasOrderBy())
            .Where(b => b.Title.Contains(command.Keyword), !string.IsNullOrEmpty(command.Keyword));
}
