using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Get.v1;
using MediatR;

namespace TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Search.v1;

public class SearchSeniorityLevelJunctionCommand : PaginationFilter, IRequest<PagedList<SeniorityLevelJunctionResponse>>
{
    public Guid? SeniorityLevelId { get; set; }
    public Guid? SkillId { get; set; }
}