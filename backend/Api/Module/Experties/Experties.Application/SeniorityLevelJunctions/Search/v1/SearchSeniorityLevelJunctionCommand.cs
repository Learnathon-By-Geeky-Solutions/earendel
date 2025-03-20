using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Get.v1;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Search.v1;
[ExcludeFromCodeCoverage]
public class SearchSeniorityLevelJunctionCommand : PaginationFilter, IRequest<PagedList<SeniorityLevelJunctionResponse>>
{
    public Guid? SeniorityLevelId { get; set; }
    public Guid? SkillId { get; set; }
}