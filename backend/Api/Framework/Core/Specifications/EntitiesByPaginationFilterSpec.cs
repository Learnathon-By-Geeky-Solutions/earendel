using TalentMesh.Framework.Core.Paging;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Specifications;
[ExcludeFromCodeCoverage]
public class EntitiesByPaginationFilterSpec<T, TResult> : EntitiesByBaseFilterSpec<T, TResult>
{
    public EntitiesByPaginationFilterSpec(PaginationFilter filter)
        : base(filter) =>
        Query.PaginateBy(filter);
}
[ExcludeFromCodeCoverage]

public class EntitiesByPaginationFilterSpec<T> : EntitiesByBaseFilterSpec<T>
{
    public EntitiesByPaginationFilterSpec(PaginationFilter filter)
        : base(filter) =>
        Query.PaginateBy(filter);
}
