using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Experties.Application.SubSkills.Get.v1;
using MediatR;

namespace TalentMesh.Module.Experties.Application.SubSkills.Search.v1;

public class SearchSubSkillsCommand : PaginationFilter, IRequest<PagedList<SubSkillResponse>>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
