using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Application.Rubrics.Get.v1;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Application.Rubrics.Search.v1;

[ExcludeFromCodeCoverage]

public sealed class SearchRubricsHandler(
    [FromKeyedServices("rubrics:rubricReadOnly")] IReadRepository<Experties.Domain.Rubric> repository)
    : IRequestHandler<SearchRubricsCommand, PagedList<RubricResponse>>
{
    public async Task<PagedList<RubricResponse>> Handle(SearchRubricsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchRubricSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<RubricResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}
