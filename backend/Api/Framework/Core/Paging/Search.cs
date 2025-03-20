using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Paging;
[ExcludeFromCodeCoverage]
public class Search
{
    public List<string> Fields { get; set; } = new();
    public string? Keyword { get; set; }
}
