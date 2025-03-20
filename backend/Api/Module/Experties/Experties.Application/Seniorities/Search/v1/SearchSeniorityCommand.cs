using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Experties.Application.Seniorities.Get.v1;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Application.Seniorities.Search.v1;

[ExcludeFromCodeCoverage]

public class SearchSenioritiesCommand : PaginationFilter, IRequest<PagedList<SeniorityResponse>>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
