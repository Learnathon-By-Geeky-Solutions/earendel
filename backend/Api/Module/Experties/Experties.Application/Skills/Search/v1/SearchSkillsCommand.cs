using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Experties.Application.Skills.Get.v1;
using MediatR;

namespace TalentMesh.Module.Experties.Application.Skills.Search.v1;

public class SearchSkillsCommand : PaginationFilter, IRequest<PagedList<SkillResponse>>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
