using Ardalis.Specification;
using TalentMesh.Framework.Core.Paging;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Specifications;
[ExcludeFromCodeCoverage]
public class EntitiesByBaseFilterSpec<T, TResult> : Specification<T, TResult>
{
    public EntitiesByBaseFilterSpec(BaseFilter filter) =>
        Query.SearchBy(filter);
}
[ExcludeFromCodeCoverage]

public class EntitiesByBaseFilterSpec<T> : Specification<T>
{
    public EntitiesByBaseFilterSpec(BaseFilter filter) =>
        Query.SearchBy(filter);
}
