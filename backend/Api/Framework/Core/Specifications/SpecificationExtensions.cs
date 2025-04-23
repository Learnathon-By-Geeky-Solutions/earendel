using Ardalis.Specification;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Specifications;
[ExcludeFromCodeCoverage]
public static class SpecificationExtensions
{
    public static bool HasSelector<T, TResult>(
        this ISpecification<T, TResult> specification)
    {
        return specification.Selector is not null;
    }
}