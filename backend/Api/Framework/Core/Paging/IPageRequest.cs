using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Paging;
public interface IPageRequest
{
    int PageNumber { get; init; }
    int PageSize { get; init; }
    string? Filters { get; init; }
    string? SortOrder { get; init; }
}
