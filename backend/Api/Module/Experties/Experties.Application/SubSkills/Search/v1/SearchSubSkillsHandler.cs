using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Application.SubSkills.Get.v1;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace TalentMesh.Module.Experties.Application.SubSkills.Search.v1;
public sealed class SearchSubSkillsHandler(
    [FromKeyedServices("subskills:subskillReadOnly")] IReadRepository<Experties.Domain.SubSkill> repository)
    : IRequestHandler<SearchSubSkillsCommand, PagedList<SubSkillResponse>>
{
    public async Task<PagedList<SubSkillResponse>> Handle(SearchSubSkillsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchSubSkillSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<SubSkillResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}
