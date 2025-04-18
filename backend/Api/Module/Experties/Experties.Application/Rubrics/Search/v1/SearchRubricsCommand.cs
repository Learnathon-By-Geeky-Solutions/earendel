using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Experties.Application.Rubrics.Get.v1;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Application.Rubrics.Search.v1;

[ExcludeFromCodeCoverage]
public class SearchRubricsCommand : PaginationFilter, IRequest<PagedList<RubricResponse>>
{
    public string? Title { get; set; }
    public string? RubricDescription { get; set; }

    public Guid? SubSkillId { get; set; }
    public Guid? SeniorityId { get; set; }
}
