using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Application.Seniorities.Get.v1;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Application.Seniorities.Search.v1;
[ExcludeFromCodeCoverage]
public sealed class SearchSenioritiesHandler(
    [FromKeyedServices("seniorities:seniorityReadOnly")] IReadRepository<Experties.Domain.Seniority> repository)
    : IRequestHandler<SearchSenioritiesCommand, PagedList<SeniorityResponse>>
{
    public async Task<PagedList<SeniorityResponse>> Handle(SearchSenioritiesCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchSenioritySpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<SeniorityResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}
