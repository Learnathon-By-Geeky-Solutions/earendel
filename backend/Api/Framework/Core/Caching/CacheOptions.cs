using System.Diagnostics.CodeAnalysis;
namespace TalentMesh.Framework.Core.Caching;
[ExcludeFromCodeCoverage]
public class CacheOptions
{
    public string Redis { get; set; } = string.Empty;
}
