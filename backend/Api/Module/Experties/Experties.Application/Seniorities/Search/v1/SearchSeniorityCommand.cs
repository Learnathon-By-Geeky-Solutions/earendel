using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Experties.Application.Seniorities.Get.v1;
using MediatR;

namespace TalentMesh.Module.Experties.Application.Seniorities.Search.v1;

public class SearchSenioritiesCommand : PaginationFilter, IRequest<PagedList<SeniorityResponse>>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
