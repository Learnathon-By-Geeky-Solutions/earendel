using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Get.v1;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Search.v1;
public sealed class SearchSeniorityLevelJunctionHandler(
    [FromKeyedServices("seniorityleveljunctions:seniorityleveljunctionReadOnly")] IReadRepository<Experties.Domain.SeniorityLevelJunction> repository)
    : IRequestHandler<SearchSeniorityLevelJunctionCommand, PagedList<SeniorityLevelJunctionResponse>>
{
    public async Task<PagedList<SeniorityLevelJunctionResponse>> Handle(SearchSeniorityLevelJunctionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchSeniorityLevelJunctionSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<SeniorityLevelJunctionResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}