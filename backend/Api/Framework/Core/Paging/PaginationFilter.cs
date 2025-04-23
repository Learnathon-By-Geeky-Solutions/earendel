using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Paging;
[ExcludeFromCodeCoverage]
public class PaginationFilter : BaseFilter
{
    public int PageNumber { get; set; }

    public int PageSize { get; set; } = int.MaxValue;
    public string[]? OrderBy { get; set; }
}
[ExcludeFromCodeCoverage]

public static class PaginationFilterExtensions
{
    public static bool HasOrderBy(this PaginationFilter filter) =>
    filter.OrderBy != null && filter.OrderBy.Length > 0;

}
