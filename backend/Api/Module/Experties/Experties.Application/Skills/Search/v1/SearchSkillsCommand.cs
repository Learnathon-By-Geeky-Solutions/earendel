using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Experties.Application.Skills.Get.v1;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Application.Skills.Search.v1;

[ExcludeFromCodeCoverage]
public class SearchSkillsCommand : PaginationFilter, IRequest<PagedList<SkillResponse>>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
