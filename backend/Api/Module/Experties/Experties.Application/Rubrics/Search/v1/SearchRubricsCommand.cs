using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Experties.Application.Rubrics.Get.v1;
using MediatR;

namespace TalentMesh.Module.Experties.Application.Rubrics.Search.v1;

public class SearchRubricsCommand : PaginationFilter, IRequest<PagedList<RubricResponse>>
{
    public string? Title { get; set; }
    public string? RubricDescription { get; set; }
}
