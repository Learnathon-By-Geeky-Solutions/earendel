using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Experties.Application.SubSkills.Get.v1;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Application.SubSkills.Search.v1;

[ExcludeFromCodeCoverage]

public class SearchSubSkillsCommand : PaginationFilter, IRequest<PagedList<SubSkillResponse>>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
