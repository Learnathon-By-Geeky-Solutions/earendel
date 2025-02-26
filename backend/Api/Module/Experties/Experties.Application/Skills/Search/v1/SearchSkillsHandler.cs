using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Application.Skills.Get.v1;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace TalentMesh.Module.Experties.Application.Skills.Search.v1;
public sealed class SearchSkillsHandler(
    [FromKeyedServices("skills:skillReadOnly")] IReadRepository<Experties.Domain.Skill> repository)
    : IRequestHandler<SearchSkillsCommand, PagedList<SkillResponse>>
{
    public async Task<PagedList<SkillResponse>> Handle(SearchSkillsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchSkillSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<SkillResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}
